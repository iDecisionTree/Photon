using Photon.Core.Geometry;
using Photon.Math;
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Vector2 = Photon.Math.Vector.Vector2;
using Vector3 = Photon.Math.Vector.Vector3;
using Vector4 = Photon.Math.Vector.Vector4;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class RasterizationStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            for (int i = 0; i < context.geometryObjects.Count; i++)
            {
                GeometryObject geometryObject = context.geometryObjects[i];
                for (int j = 0; j < geometryObject.primitive.triangles.Length; j += 3)
                {
                    int index0 = geometryObject.primitive.triangles[j];
                    int index1 = geometryObject.primitive.triangles[j + 1];
                    int index2 = geometryObject.primitive.triangles[j + 2];

                    Dictionary<string, (FragmentAttribute a, FragmentAttribute b, FragmentAttribute c)> attributes = new Dictionary<string, (FragmentAttribute a, FragmentAttribute b, FragmentAttribute c)>();
                    foreach (KeyValuePair<string, FragmentAttribute[]> kvp in geometryObject.attributes)
                    {
                        string attributeName = kvp.Key;
                        FragmentAttribute[] attributeArray = kvp.Value;
                        attributes[attributeName] = (attributeArray[index0], attributeArray[index1], attributeArray[index2]);
                    }

                    RasterizeTriangle(context, attributes);
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private void RasterizeTriangle(RenderContext context, Dictionary<string, (FragmentAttribute a, FragmentAttribute b, FragmentAttribute c)> attributes)
        {
            (FragmentAttribute a, FragmentAttribute b, FragmentAttribute c) positionCS = attributes["positionCS"];
            Vector4 positionCS0 = positionCS.a.vector4Value;
            Vector4 positionCS1 = positionCS.b.vector4Value;
            Vector4 positionCS2 = positionCS.c.vector4Value;

            (FragmentAttribute a, FragmentAttribute b, FragmentAttribute c) positionSS = attributes["positionSS"];
            Vector2 positionSS0 = positionSS.a.vector2Value;
            Vector2 positionSS1 = positionSS.b.vector2Value;
            Vector2 positionSS2 = positionSS.c.vector2Value;

            Vector2 edge1 = positionSS1 - positionSS0;
            Vector2 edge2 = positionSS2 - positionSS0;
            if (Vector2.Cross(edge1, edge2) > 0f)
            {
                return;
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

                    Dictionary<string, FragmentAttribute> interpolatedAttributes = new Dictionary<string, FragmentAttribute>();

                    foreach (KeyValuePair<string, (FragmentAttribute a, FragmentAttribute b, FragmentAttribute c)> kvp in attributes)
                    {
                        FragmentAttribute interpolated = Interpolate(kvp.Value.a, kvp.Value.b, kvp.Value.c, alphaInvW0, beteInvW1, gammaInvW2, perspectiveDenom);
                        interpolatedAttributes.Add(kvp.Key, interpolated);
                    }

                    context.fragments.Add(new Fragment(pixelPosition, Vector4.zero, interpolatedAttributes));
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

        private FragmentAttribute Interpolate(FragmentAttribute a, FragmentAttribute b, FragmentAttribute c, float alphaInvW0, float betaInvW1, float gammaInvW2, float perspectiveDenom)
        {
            if (a.type != b.type || a.type != c.type)
            {
                throw new InvalidOperationException("片元属性类型不匹配");
            }

            switch (a.type)
            {
                case FragmentAttributeType.Float:
                    return new FragmentAttribute((a.floatValue * alphaInvW0 + b.floatValue * betaInvW1 + c.floatValue * gammaInvW2) * perspectiveDenom);
                case FragmentAttributeType.Vector2:
                    return new FragmentAttribute((a.vector2Value * alphaInvW0 + b.vector2Value * betaInvW1 + c.vector2Value * gammaInvW2) * perspectiveDenom);
                case FragmentAttributeType.Vector3:                             
                    return new FragmentAttribute((a.vector3Value * alphaInvW0 + b.vector3Value * betaInvW1 + c.vector3Value * gammaInvW2) * perspectiveDenom);
                case FragmentAttributeType.Vector4:                             
                    return new FragmentAttribute((a.vector4Value * alphaInvW0 + b.vector4Value * betaInvW1 + c.vector4Value * gammaInvW2) * perspectiveDenom);
                default:
                    throw new InvalidOperationException("不支持的片元属性类型");
            }
        }
    }
}
