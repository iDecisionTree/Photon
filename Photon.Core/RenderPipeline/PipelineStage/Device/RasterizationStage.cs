using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                for (int j = 0; j < context.geometryObjects[i].positionNDC!.Length; j++)
                {
                    Vector3 positionNDC = context.geometryObjects[i].positionNDC![j];
                    float screenX = (positionNDC.x + 1f) * 0.5f * context.viewportSize.x;
                    float screenY = (1f - positionNDC.y) * 0.5f * context.viewportSize.y;

                    if (screenX < 0f || screenX >= context.viewportSize.x || screenY < 0f || screenY >= context.viewportSize.y)
                    {
                        continue;
                    }
                    frameBuffer?.SetColor((int)screenX, (int)screenY, new Vector4(1f, 1f, 1f, 1f));
                }
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
