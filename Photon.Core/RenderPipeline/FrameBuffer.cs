using Photon.Core.Texture;
using Photon.Math.Vector;

namespace Photon.Core.RenderPipeline
{
    public class FrameBuffer : IDisposable
    {
        public int width => _width;
        public int height => _height;
        public Texture2D? colorBuffer => _colorBuffer;
        public bool isDisposed => _isDisposed;

        private int _width;
        private int _height;
        private Texture2D? _colorBuffer;
        private bool _isDisposed;

        public FrameBuffer(int width, int height)
        {
            _width = width;
            _height = height;
            _colorBuffer = new Texture2D(width, height, TextureFormat.R11G11B10_UFloat);
            _isDisposed = false;
        }

        public Vector4 GetColor(int x, int y)
        {
            if (colorBuffer == null)
            {
                throw new InvalidOperationException("颜色缓冲区未初始化");
            }

            return colorBuffer.GetPixel(x, y);
        }

        public void SetColor(int x, int y, Vector4 color)
        {
            if (colorBuffer == null)
            {
                throw new InvalidOperationException("颜色缓冲区未初始化");
            }

            colorBuffer.SetPixel(x, y, color);
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
