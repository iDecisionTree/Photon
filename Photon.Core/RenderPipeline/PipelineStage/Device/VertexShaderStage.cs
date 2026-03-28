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
                Matrix4x4 mvpMatrix = context.projectionMatrix * context.viewMatrix * context.geometryObjects[i].worldMatrix;
                // Matrix4x4 mvpMatrix = context.geometryObjects[i].worldMatrix * context.viewMatrix * context.projectionMatrix;
                for (int j = 0; j < context.geometryObjects[i].primitive.vertices.Length; j++)
                {
                    Vector4 position = new Vector4(context.geometryObjects[i].primitive.vertices[j].position, 1f);
                    Vector4 positionCS = Matrix4x4.Transform(mvpMatrix, position);
                    context.geometryObjects[i].positionCS![j] = positionCS;
                    context.geometryObjects[i].positionNDC![j] = new Vector3(positionCS.x, positionCS.y, positionCS.z) / positionCS.w;
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
