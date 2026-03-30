using Photon.Core.Texture;
using Photon.Math.Vector;

namespace Photon.Core.RenderPipeline
{
    public class FrameBuffer : IDisposable
    {
        public int width => _width;
        public int height => _height;
        public Texture2D? colorBuffer => _colorBuffer;
        public Texture2D? depthBuffer => _depthBuffer;
        public bool isDisposed => _isDisposed;

        private int _width;
        private int _height;
        private Texture2D? _colorBuffer;
        private Texture2D? _depthBuffer;
        private bool _isDisposed;

        public FrameBuffer(int width, int height)
        {
            _width = width;
            _height = height;
            _colorBuffer = new Texture2D(width, height, TextureFormat.R11G11B10_UFloat);
            _depthBuffer = new Texture2D(width, height, TextureFormat.D24_UNorm);
            _isDisposed = false;
        }

        public void Clear(Vector4 color)
        {
            if (colorBuffer == null || depthBuffer == null)
            {
                throw new InvalidOperationException("缓冲区未初始化");
            }

            colorBuffer.Clear(color);
            depthBuffer.Clear(new Vector4(1f, 0f, 0f, 0f));
        }

        public Vector4 GetColor(int x, int y)
        {
            if (colorBuffer == null)
            {
                throw new InvalidOperationException("颜色缓冲区未初始化");
            }

            return colorBuffer.GetPixel(x, y);
        }

        public float GetDepth(int x, int y)
        {
            if (depthBuffer == null)
            {
                throw new InvalidOperationException("深度缓冲区未初始化");
            }

            return depthBuffer.GetPixel(x, y).x;
        }

        public void SetColor(int x, int y, Vector4 color)
        {
            if (colorBuffer == null)
            {
                throw new InvalidOperationException("颜色缓冲区未初始化");
            }

            colorBuffer.SetPixel(x, y, color);
        }

        public void SetDepth(int x, int y, float depth)
        {
            if (depthBuffer == null)
            {
                throw new InvalidOperationException("深度缓冲区未初始化");
            }

            depthBuffer.SetPixel(x, y, new Vector4(depth, 0f, 0f, 0f));
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            colorBuffer?.Dispose();
            _colorBuffer = null;
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
