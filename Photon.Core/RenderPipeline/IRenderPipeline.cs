using Photon.Math.Vector;

namespace Photon.Core.RenderPipeline
{
    public interface IRenderPipeline
    {
        public void Initialize(Vector2 viewportSize);
        public void OnViewportResize(Vector2 newSize);
        public void RenderFrame(RenderContext context);
        public void Dispose();
    }
}
