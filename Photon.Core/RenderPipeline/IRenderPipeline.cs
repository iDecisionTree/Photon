namespace Photon.Core.RenderPipeline
{
    public interface IRenderPipeline
    {
        public void Initialize(FrameBuffer frameBuffer);
        public void Dispose();
    }
}
