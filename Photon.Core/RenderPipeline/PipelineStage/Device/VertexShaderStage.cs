using Photon.Core.Geometry;
using Photon.Math.Matrix;
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class VertexShaderStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            for (int i = 0; i < context.geometryObjects.Count; i++)
            {
                Matrix4x4 worldMatrix = context.geometryObjects[i].worldMatrix;
                Matrix4x4 mvpMatrix = context.projectionMatrix * context.viewMatrix * context.geometryObjects[i].worldMatrix;
                for (int j = 0; j < context.geometryObjects[i].primitive.vertices.Length; j++)
                {
                    Vector3 positionOS = context.geometryObjects[i].primitive.vertices[j].position;

                    Vector3 positionWS = Matrix4x4.TransformPoint(worldMatrix, positionOS);
                    Vector4 positionCS = Matrix4x4.Transform(mvpMatrix, new Vector4(positionOS, 1f));
                    Vector3 positionNDC = new Vector3(positionCS.x, positionCS.y, positionCS.z) / positionCS.w;
                    Vector2 positionSS = new Vector2((positionNDC.x + 1f) * 0.5f * context.viewport.x, (1f - (positionNDC.y + 1f) * 0.5f) * context.viewport.y);

                    Vector3 normalOS = context.geometryObjects[i].primitive.vertices[j].normal;
                    Vector3 normalWS = Matrix4x4.TransformVector(worldMatrix, normalOS);

                    float depth = positionNDC.z;

                    context.geometryObjects[i].attributes["positionOS"][j] = new FragmentAttribute(FragmentAttributeType.Vector3, positionOS);
                    context.geometryObjects[i].attributes["positionWS"][j] = new FragmentAttribute(FragmentAttributeType.Vector3, positionWS);
                    context.geometryObjects[i].attributes["positionCS"][j] = new FragmentAttribute(FragmentAttributeType.Vector4, positionCS);
                    context.geometryObjects[i].attributes["positionNDC"][j] = new FragmentAttribute(FragmentAttributeType.Vector3, positionNDC);
                    context.geometryObjects[i].attributes["positionSS"][j] = new FragmentAttribute(FragmentAttributeType.Vector2, positionSS);
                    context.geometryObjects[i].attributes["normalOS"][j] = new FragmentAttribute(FragmentAttributeType.Vector3, normalOS);
                    context.geometryObjects[i].attributes["normalWS"][j] = new FragmentAttribute(FragmentAttributeType.Vector3, normalWS);
                    context.geometryObjects[i].attributes["depth"][j] = new FragmentAttribute(FragmentAttributeType.Float, depth);
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
