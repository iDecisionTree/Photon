using Photon.Core.Geometry;
using Photon.Math;

namespace Photon.Core.RenderPipeline.PipelineStage.Device
{
    public class OutputMergeStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        /// <summary>
        /// 需要帧缓冲
        /// </summary>
        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            if (frameBuffer == null)
            {
                throw new ArgumentNullException(nameof(frameBuffer), "帧缓冲不能为空");
            }

            for (int i = 0; i < context.fragments.Count; i++)
            {
                Fragment fragment = context.fragments[i];

                int pixelX = (int)Mathf.Floor(fragment.positionSS.x);
                int pixelY = (int)Mathf.Floor(fragment.positionSS.y);

                float depth = fragment.attributes["depth"].floatValue;

                if (depth > frameBuffer.GetDepth(pixelX, pixelY))
                {
                    continue;
                }

                frameBuffer.SetDepth(pixelX, pixelY, depth);

                frameBuffer.SetColor(pixelX, pixelY, fragment.color);
            }
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
