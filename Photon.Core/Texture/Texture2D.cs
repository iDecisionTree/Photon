using Photon.Math.Vector;

namespace Photon.Core.Texture
{
    public class Texture2D : IDisposable
    {
        public int width => _width;
        public int height => _height;
        public TextureFormat format => _format;
        public TextureFormatInfo formatInfo => _formatInfo;
        public bool isDisposed => _isDisposed;

        private int _width;
        private int _height;
        private TextureFormat _format;
        private TextureFormatInfo _formatInfo;
        private byte[]? _data;
        private bool _isDisposed;

        public Texture2D(int width, int height, TextureFormat format)
        {
            _width = width;
            _height = height;
            _format = format;
            if (!TextureFormatHelper.TryGetFormatInfo(format, out _formatInfo))
            {
                throw new NotSupportedException($"不支持纹理格式{format}");
            }
            _data = new byte[GetByteLength()];
            _isDisposed = false;
        }

        public Texture2D(int width, int height, TextureFormat format, byte[] data)
        {
            _width = width;
            _height = height;
            _format = format;
            if (!TextureFormatHelper.TryGetFormatInfo(format, out _formatInfo))
            {
                throw new NotSupportedException($"不支持纹理格式{format}");
            }
            if (data.Length != GetByteLength())
            {
                throw new ArgumentException("数据长度与纹理格式不匹配");
            }
            _data = data;
            _isDisposed = false;
        }

        public int GetByteLength()
        {
            return formatInfo.GetByteLength(width, height);
        }

        public Vector4 GetPixel(int x, int y)
        {
            if (!IsValidPixel(x, y))
            {
                throw new ArgumentException($"像素坐标({x},{y})超出纹理范围({_width}x{_height})");
            }
            if (_data == null)
            {
                throw new InvalidOperationException("纹理数据未初始化");
            }

            int pixelIndex = GetPixelIndex(x, y);
            return formatInfo.Decode(_data, pixelIndex);
        }

        public void SetPixel(int x, int y, Vector4 color)
        {
            if (!IsValidPixel(x, y))
            {
                throw new ArgumentException($"像素坐标({x},{y})超出纹理范围({_width}x{_height})");
            }
            if (_data == null)
            {
                throw new InvalidOperationException("纹理数据未初始化");
            }

            int pixelIndex = GetPixelIndex(x, y);
            formatInfo.Encode(_data, pixelIndex, color);
        }

        private int GetPixelIndex(int x, int y)
        {
            return x + y * width;
        }

        private bool IsValidPixel(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            _data = null;
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
