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

                foreach (Fragment fragment in Execute(context, geometryObject))
                {
                    yield return fragment;
                }
            }
        }

        public void Execute(RenderContext context, Action<Fragment> fragmentHandler)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(fragmentHandler);

            for (int i = 0; i < context.geometryObjects.Count; i++)
            {
                Execute(context, context.geometryObjects[i], fragmentHandler);
            }
        }

        public IEnumerable<Fragment> Execute(RenderContext context, GeometryObject geometryObject)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(geometryObject);

            int positionSSIndex = geometryObject.propertyIndexMap["PositionSS"];

            for (int j = 0; j < geometryObject.primitive.triangles.Length; j += 3)
            {
                int index0 = geometryObject.primitive.triangles[j];
                int index1 = geometryObject.primitive.triangles[j + 1];
                int index2 = geometryObject.primitive.triangles[j + 2];

                foreach (Fragment fragment in RasterizeTriangle(context, geometryObject, positionSSIndex, index0, index1, index2))
                {
                    yield return fragment;
                }
            }
        }

        public void Execute(RenderContext context, GeometryObject geometryObject, Action<Fragment> fragmentHandler)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(geometryObject);
            ArgumentNullException.ThrowIfNull(fragmentHandler);

            int positionSSIndex = geometryObject.propertyIndexMap["PositionSS"];

            for (int j = 0; j < geometryObject.primitive.triangles.Length; j += 3)
            {
                int index0 = geometryObject.primitive.triangles[j];
                int index1 = geometryObject.primitive.triangles[j + 1];
                int index2 = geometryObject.primitive.triangles[j + 2];

                RasterizeTriangle(context, geometryObject, positionSSIndex, index0, index1, index2, fragmentHandler);
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private IEnumerable<Fragment> RasterizeTriangle(RenderContext context, GeometryObject geometryObject, int positionSSIndex, int index0, int index1, int index2)
        {
            GeometryProperty[][] properties = geometryObject.properties!;
            int propertyCount = properties.GetLength(0);

            Vector4 positionSS0 = properties[positionSSIndex][index0].vector4Value;
            Vector4 positionSS1 = properties[positionSSIndex][index1].vector4Value;
            Vector4 positionSS2 = properties[positionSSIndex][index2].vector4Value;

            Vector2 positionSS0XY = new Vector2(positionSS0.x, positionSS0.y);
            Vector2 positionSS1XY = new Vector2(positionSS1.x, positionSS1.y);
            Vector2 positionSS2XY = new Vector2(positionSS2.x, positionSS2.y);

            Vector2 edge1 = positionSS1XY - positionSS0XY;
            Vector2 edge2 = positionSS2XY - positionSS0XY;
            if (Vector2.Cross(edge1, edge2) > 0f)
            {
                yield break;
            }

            int minX = (int)Mathf.Max(0f, (float)Mathf.Floor(Mathf.Min(positionSS0XY.x, Mathf.Min(positionSS1XY.x, positionSS2XY.x))));
            int maxX = (int)Mathf.Min(context.viewport.x - 1f, (float)Mathf.Ceiling(Mathf.Max(positionSS0XY.x, Mathf.Max(positionSS1XY.x, positionSS2XY.x))));
            int minY = (int)Mathf.Max(0f, (float)Mathf.Floor(Mathf.Min(positionSS0XY.y, Mathf.Min(positionSS1XY.y, positionSS2XY.y))));
            int maxY = (int)Mathf.Min(context.viewport.y - 1f, (float)Mathf.Ceiling(Mathf.Max(positionSS0XY.y, Mathf.Max(positionSS1XY.y, positionSS2XY.y))));

            float invW0 = 1f / positionSS0.w;
            float invW1 = 1f / positionSS1.w;
            float invW2 = 1f / positionSS2.w;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Vector2 pixelPosition = new Vector2(x + 0.5f, y + 0.5f);

                    (float alpha, float beta, float gamma) = CalculateBarycentric(pixelPosition, positionSS0XY, positionSS1XY, positionSS2XY);
                    if (alpha < 0f || beta < 0f || gamma < 0f)
                    {
                        continue;
                    }

                    float alphaInvW0 = alpha * invW0;
                    float beteInvW1 = beta * invW1;
                    float gammaInvW2 = gamma * invW2;

                    float perspectiveDenom = 1f / (alphaInvW0 + beteInvW1 + gammaInvW2);

                    GeometryProperty[] interpolatedProperties = ArrayPool<GeometryProperty>.Shared.Rent(propertyCount);
                    try
                    {
                        for (int i = 0; i < propertyCount; i++)
                        {
                            GeometryProperty interpolated = Interpolate(properties[i][index0], properties[i][index1], properties[i][index2], alphaInvW0, beteInvW1, gammaInvW2, perspectiveDenom);
                            interpolatedProperties[i] = interpolated;
                        }

                        yield return new Fragment(pixelPosition, Vector4.zero, interpolatedProperties, geometryObject.propertyIndexMap, geometryObject.material);
                    }
                    finally
                    {
                        ArrayPool<GeometryProperty>.Shared.Return(interpolatedProperties);
                    }
                }
            }
        }

        private void RasterizeTriangle(RenderContext context, GeometryObject geometryObject, int positionSSIndex, int index0, int index1, int index2, Action<Fragment> fragmentHandler)
        {
            GeometryProperty[][] properties = geometryObject.properties!;
            int propertyCount = properties.GetLength(0);

            Vector4 positionSS0 = properties[positionSSIndex][index0].vector4Value;
            Vector4 positionSS1 = properties[positionSSIndex][index1].vector4Value;
            Vector4 positionSS2 = properties[positionSSIndex][index2].vector4Value;

            Vector2 positionSS0XY = new Vector2(positionSS0.x, positionSS0.y);
            Vector2 positionSS1XY = new Vector2(positionSS1.x, positionSS1.y);
            Vector2 positionSS2XY = new Vector2(positionSS2.x, positionSS2.y);

            Vector2 edge1 = positionSS1XY - positionSS0XY;
            Vector2 edge2 = positionSS2XY - positionSS0XY;
            if (Vector2.Cross(edge1, edge2) > 0f)
            {
                return;
            }

            int minX = (int)Mathf.Max(0f, (float)Mathf.Floor(Mathf.Min(positionSS0XY.x, Mathf.Min(positionSS1XY.x, positionSS2XY.x))));
            int maxX = (int)Mathf.Min(context.viewport.x - 1f, (float)Mathf.Ceiling(Mathf.Max(positionSS0XY.x, Mathf.Max(positionSS1XY.x, positionSS2XY.x))));
            int minY = (int)Mathf.Max(0f, (float)Mathf.Floor(Mathf.Min(positionSS0XY.y, Mathf.Min(positionSS1XY.y, positionSS2XY.y))));
            int maxY = (int)Mathf.Min(context.viewport.y - 1f, (float)Mathf.Ceiling(Mathf.Max(positionSS0XY.y, Mathf.Max(positionSS1XY.y, positionSS2XY.y))));

            float invW0 = 1f / positionSS0.w;
            float invW1 = 1f / positionSS1.w;
            float invW2 = 1f / positionSS2.w;

            GeometryProperty[] interpolatedProperties = ArrayPool<GeometryProperty>.Shared.Rent(propertyCount);
            try
            {
                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        Vector2 pixelPosition = new Vector2(x + 0.5f, y + 0.5f);

                        (float alpha, float beta, float gamma) = CalculateBarycentric(pixelPosition, positionSS0XY, positionSS1XY, positionSS2XY);
                        if (alpha < 0f || beta < 0f || gamma < 0f)
                        {
                            continue;
                        }

                        float alphaInvW0 = alpha * invW0;
                        float beteInvW1 = beta * invW1;
                        float gammaInvW2 = gamma * invW2;

                        float perspectiveDenom = 1f / (alphaInvW0 + beteInvW1 + gammaInvW2);

                        for (int i = 0; i < propertyCount; i++)
                        {
                            GeometryProperty interpolated = Interpolate(properties[i][index0], properties[i][index1], properties[i][index2], alphaInvW0, beteInvW1, gammaInvW2, perspectiveDenom);
                            interpolatedProperties[i] = interpolated;
                        }

                        fragmentHandler(new Fragment(pixelPosition, Vector4.zero, interpolatedProperties, geometryObject.propertyIndexMap, geometryObject.material));
                    }
                }
            }
            finally
            {
                ArrayPool<GeometryProperty>.Shared.Return(interpolatedProperties);
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

        private GeometryProperty Interpolate(GeometryProperty a, GeometryProperty b, GeometryProperty c, float alphaInvW0, float betaInvW1, float gammaInvW2, float perspectiveDenom)
        {
            if (a.type != b.type || a.type != c.type)
            {
                throw new InvalidOperationException("几何属性类型不匹配");
            }

            switch (a.type)
            {
                case GeometryPropertyType.Float:
                    return new GeometryProperty((a.floatValue * alphaInvW0 + b.floatValue * betaInvW1 + c.floatValue * gammaInvW2) * perspectiveDenom);
                case GeometryPropertyType.Vector2:
                    return new GeometryProperty((a.vector2Value * alphaInvW0 + b.vector2Value * betaInvW1 + c.vector2Value * gammaInvW2) * perspectiveDenom);
                case GeometryPropertyType.Vector3:
                    return new GeometryProperty((a.vector3Value * alphaInvW0 + b.vector3Value * betaInvW1 + c.vector3Value * gammaInvW2) * perspectiveDenom);
                case GeometryPropertyType.Vector4:
                    return new GeometryProperty((a.vector4Value * alphaInvW0 + b.vector4Value * betaInvW1 + c.vector4Value * gammaInvW2) * perspectiveDenom);
                default:
                    throw new InvalidOperationException("不支持的几何属性类型");
            }
        }
    }
}
