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

        public virtual void Initialize(FrameBuffer frameBuffer)
        {
            _frameBuffer = frameBuffer;
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
