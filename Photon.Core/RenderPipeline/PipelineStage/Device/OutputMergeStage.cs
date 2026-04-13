using Photon.Core.Geometry;
using Photon.Core.Geometry.Fragment;
using Photon.Math;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class OutputMergeStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            throw new NotSupportedException("未实现的方法");
        }

        public void Execute(Fragment fragment, FrameBuffer frameBuffer)
        {
            int pixelX = (int)Mathf.Floor(fragment.positionSS.x);
            int pixelY = (int)Mathf.Floor(fragment.positionSS.y);
            Execute(fragment, frameBuffer, pixelX, pixelY);
        }

        public void Execute(Fragment fragment, FrameBuffer frameBuffer, int pixelX, int pixelY)
        {
            float depth = fragment.properties[(int)BuildinGeometryPropertyType.Depth].floatValue;
            if (depth > frameBuffer.GetDepthUnchecked(pixelX, pixelY))
            {
                return;
            }

            frameBuffer.SetDepthUnchecked(pixelX, pixelY, depth);
            frameBuffer.SetColorUnchecked(pixelX, pixelY, fragment.color);
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
