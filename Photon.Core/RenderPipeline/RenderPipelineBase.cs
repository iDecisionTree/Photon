
using Photon.Math.Vector;

namespace Photon.Core.RenderPipeline
{
    public abstract class RenderPipelineBase : IRenderPipeline, IDisposable
    {
        public bool isDisposed => _isDisposed;

        protected FrameBuffer? _frameBuffer = null;
        private bool _isDisposed;

        protected RenderPipelineBase()
        {
            _isDisposed = false;
        }

        public virtual void Initialize(Vector2 viewportSize)
        {
            _frameBuffer = new FrameBuffer((int)viewportSize.x, (int)viewportSize.y);
        }

        public virtual void OnViewportResize(Vector2 newSize)
        {
            _frameBuffer = new FrameBuffer((int)newSize.x, (int)newSize.y);
        }

        public virtual void RenderFrame(RenderContext context)
        {
        }

        public virtual void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            _frameBuffer?.Dispose();
            _frameBuffer = null;
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
