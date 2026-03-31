using Photon.Core.Texture;

namespace Photon.Core.RenderPipeline.PipelineStage.Application
{
    public class PresentationStage : PipelineStageBase
    {
        public override void Initialize()
        {
        }

        /// <summary>
        /// 需要帧缓冲
        /// </summary>
        public override void Execute(RenderContext context, FrameBuffer? frameBuffer = null)
        {
            if (frameBuffer == null || frameBuffer.colorBuffer == null)
            {
                throw new ArgumentNullException(nameof(frameBuffer), "帧缓冲不能为空");
            }

            context.renderTarget = Texture2D.ConvertTo(frameBuffer.colorBuffer, TextureFormat.B8G8R8A8_UNorm);
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
