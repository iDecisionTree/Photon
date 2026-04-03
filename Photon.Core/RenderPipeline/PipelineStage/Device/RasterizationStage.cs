using Photon.Core.Geometry;
using Photon.Core.Geometry.Fragment;
using Photon.Math;
using Photon.Math.Vector;
using System.Buffers;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class RasterizationStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            throw new NotSupportedException("未实现的方法");
        }

        public IEnumerable<Fragment> Execute(RenderContext context)
        {
            for (int i = 0; i < context.geometryObjects.Count; i++)
            {
                GeometryObject geometryObject = context.geometryObjects[i];

                int positionCSIndex = geometryObject.propertyIndexMap["positionCS"];
                int positionSSIndex = geometryObject.propertyIndexMap["positionSS"];

                for (int j = 0; j < geometryObject.primitive.triangles.Length; j += 3)
                {
                    int index0 = geometryObject.primitive.triangles[j];
                    int index1 = geometryObject.primitive.triangles[j + 1];
                    int index2 = geometryObject.primitive.triangles[j + 2];

                    foreach (Fragment fragment in RasterizeTriangle(context, geometryObject, positionCSIndex, positionSSIndex, index0, index1, index2))
                    {
                        yield return fragment;
                    }
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private IEnumerable<Fragment> RasterizeTriangle(RenderContext context, GeometryObject geometryObject, int positionCSIndex, int positionSSIndex, int index0, int index1, int index2)
        {
            Vector4 positionCS0 = geometryObject.attributes![positionCSIndex][index0].vector4Value;
            Vector4 positionCS1 = geometryObject.attributes![positionCSIndex][index1].vector4Value;
            Vector4 positionCS2 = geometryObject.attributes![positionCSIndex][index2].vector4Value;

            Vector2 positionSS0 = geometryObject.attributes![positionSSIndex][index0].vector2Value;
            Vector2 positionSS1 = geometryObject.attributes![positionSSIndex][index1].vector2Value;
            Vector2 positionSS2 = geometryObject.attributes![positionSSIndex][index2].vector2Value;

            Vector2 edge1 = positionSS1 - positionSS0;
            Vector2 edge2 = positionSS2 - positionSS0;
            if (Vector2.Cross(edge1, edge2) > 0f)
            {
                yield break;
            }

            int minX = (int)Mathf.Max(0f, (float)Mathf.Floor(Mathf.Min(positionSS0.x, Mathf.Min(positionSS1.x, positionSS2.x))));
            int maxX = (int)Mathf.Min(context.viewport.x - 1f, (float)Mathf.Ceiling(Mathf.Max(positionSS0.x, Mathf.Max(positionSS1.x, positionSS2.x))));
            int minY = (int)Mathf.Max(0f, (float)Mathf.Floor(Mathf.Min(positionSS0.y, Mathf.Min(positionSS1.y, positionSS2.y))));
            int maxY = (int)Mathf.Min(context.viewport.y - 1f, (float)Mathf.Ceiling(Mathf.Max(positionSS0.y, Mathf.Max(positionSS1.y, positionSS2.y))));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Vector2 pixelPosition = new Vector2(x + 0.5f, y + 0.5f);

                    (float alpha, float beta, float gamma) = CalculateBarycentric(pixelPosition, positionSS0, positionSS1, positionSS2);
                    if (alpha < 0f || beta < 0f || gamma < 0f)
                    {
                        continue;
                    }

                    float invW0 = 1f / positionCS0.w;
                    float invW1 = 1f / positionCS1.w;
                    float invW2 = 1f / positionCS2.w;

                    float alphaInvW0 = alpha * invW0;
                    float beteInvW1 = beta * invW1;
                    float gammaInvW2 = gamma * invW2;

                    float perspectiveDenom = 1f / (alphaInvW0 + beteInvW1 + gammaInvW2);

                    GeometryAttribute[] interpolatedAttributes = ArrayPool<GeometryAttribute>.Shared.Rent(geometryObject.attributes.GetLength(0));
                    try
                    {
                        for (int i = 0; i < geometryObject.attributes.GetLength(0); i++)
                        {
                            GeometryAttribute interpolated = Interpolate(geometryObject.attributes[i][index0], geometryObject.attributes[i][index1], geometryObject.attributes[i][index2], alphaInvW0, beteInvW1, gammaInvW2, perspectiveDenom);
                            interpolatedAttributes[i] = interpolated;
                        }

                        yield return new Fragment(pixelPosition, Vector4.zero, interpolatedAttributes, geometryObject.propertyIndexMap);
                    }
                    finally
                    {
                        ArrayPool<GeometryAttribute>.Shared.Return(interpolatedAttributes);
                    }
                }
            }
        }

        private (float alpha, float beta, float gamma) CalculateBarycentric(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 v0 = c - a;
            Vector2 v1 = b - a;
            Vector2 v2 = p - a;

            float dot00 = Vector2.Dot(v0, v0);
            float dot01 = Vector2.Dot(v0, v1);
            float dot02 = Vector2.Dot(v0, v2);
            float dot11 = Vector2.Dot(v1, v1);
            float dot12 = Vector2.Dot(v1, v2);

            float denom = dot00 * dot11 - dot01 * dot01;
            if (Mathf.Abs(denom) < Mathf.Epsilon)
            {
                return (-1f, -1f, -1f);
            }

            float invDenom = 1f / denom;
            float gamma = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float beta = (dot00 * dot12 - dot01 * dot02) * invDenom;
            float alpha = 1f - beta - gamma;

            return (alpha, beta, gamma);
        }

        private GeometryAttribute Interpolate(GeometryAttribute a, GeometryAttribute b, GeometryAttribute c, float alphaInvW0, float betaInvW1, float gammaInvW2, float perspectiveDenom)
        {
            if (a.type != b.type || a.type != c.type)
            {
                throw new InvalidOperationException("片元属性类型不匹配");
            }

            switch (a.type)
            {
                case GeometryAttributeType.Float:
                    return new GeometryAttribute((a.floatValue * alphaInvW0 + b.floatValue * betaInvW1 + c.floatValue * gammaInvW2) * perspectiveDenom);
                case GeometryAttributeType.Vector2:
                    return new GeometryAttribute((a.vector2Value * alphaInvW0 + b.vector2Value * betaInvW1 + c.vector2Value * gammaInvW2) * perspectiveDenom);
                case GeometryAttributeType.Vector3:
                    return new GeometryAttribute((a.vector3Value * alphaInvW0 + b.vector3Value * betaInvW1 + c.vector3Value * gammaInvW2) * perspectiveDenom);
                case GeometryAttributeType.Vector4:
                    return new GeometryAttribute((a.vector4Value * alphaInvW0 + b.vector4Value * betaInvW1 + c.vector4Value * gammaInvW2) * perspectiveDenom);
                default:
                    throw new InvalidOperationException("不支持的片元属性类型");
            }
        }
    }
}
