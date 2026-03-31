namespace Photon.Core.RenderPipeline.PipelineStage
{
    public abstract class PipelineStageBase : IDisposable
    {
        public abstract void Initialize();
        public abstract void Execute(RenderContext context, FrameBuffer? frameBuffer = null);
        public abstract void Dispose();
    }
}
