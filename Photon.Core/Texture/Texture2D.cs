using Photon.Math;
using Photon.Math.Vector;
using System.Text;

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

        public void Clear(Vector4 color)
        {
            if (_data == null)
            {
                throw new InvalidOperationException("纹理数据未初始化");
            }

            int bytesPerPixel = formatInfo.bytesPerPixel;
            int pixelCount = width * height;

            byte[] cache = new byte[bytesPerPixel];
            formatInfo.Encode(cache, 0, color);

            for (int i = 0; i < pixelCount; i++)
            {
                Buffer.BlockCopy(cache, 0, _data, i * bytesPerPixel, bytesPerPixel);
            }
        }

        public void ConvertTo(TextureFormat format)
        {
            if (!TextureFormatHelper.TryGetFormatInfo(format, out TextureFormatInfo newFormatInfo))
            {
                throw new NotSupportedException($"不支持纹理格式{format}");
            }
            if (_data == null)
            {
                throw new InvalidOperationException("纹理数据未初始化");
            }

            if (this.format == format)
            {
                return;
            }

            byte[] newData = new byte[width * height * newFormatInfo.bytesPerPixel];
            for (int i = 0; i < width * height; i++)
            {
                Vector4 color = formatInfo.Decode(_data, i);
                newFormatInfo.Encode(newData, i, color);
            }

            _format = format;
            _formatInfo = newFormatInfo;
            _data = newData;
        }

        public void CopyTo(Texture2D destination)
        {
            Copy(this, destination);
        }

        public int GetByteLength()
        {
            return formatInfo.GetByteLength(width, height);
        }

        public byte[] GetData()
        {
            if (_data == null)
            {
                throw new InvalidOperationException("纹理数据未初始化");
            }

            return _data;
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

        public void Save(string filePath)
        {
            if (_data == null)
            {
                throw new InvalidOperationException("纹理数据未初始化");
            }

            byte[] rgb = new byte[width * height * 3];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector4 color = GetPixel(x, y);
                    int index = (y * width + x) * 3;

                    rgb[index] = (byte)Mathf.Clamp(color.x * 255f, 0f, 255f);
                    rgb[index + 1] = (byte)Mathf.Clamp(color.y * 255f, 0f, 255f);
                    rgb[index + 2] = (byte)Mathf.Clamp(color.z * 255f, 0f, 255f);
                }
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                string header = $"P6\n{width} {height}\n255\n";
                byte[] headerBytes = Encoding.ASCII.GetBytes(header);

                fs.Write(headerBytes, 0, headerBytes.Length);
                fs.Write(rgb, 0, rgb.Length);
            }
        }

        public void SetData(byte[] data)
        {
            _data = data;
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

        public static Texture2D ConvertTo(Texture2D source, TextureFormat format)
        {
            Texture2D newTexture = new Texture2D(source.width, source.height, format);
            for (int y = 0; y < source.height; y++)
            {
                for (int x = 0; x < source.width; x++)
                {
                    Vector4 color = source.GetPixel(x, y);
                    newTexture.SetPixel(x, y, color);
                }
            }

            return newTexture;
        }

        public static void Copy(Texture2D source, Texture2D destination)
        {
            if (source.width != destination.width || source.height != destination.height)
            {
                throw new ArgumentException("源纹理和目标纹理的尺寸不匹配");
            }
            if (source._data == null || destination._data == null)
            {
                throw new InvalidOperationException("纹理数据未初始化");
            }

            if (source.format == destination.format)
            {
                Array.Copy(source._data, 0, destination._data, 0, source._data.Length);
                return;
            }

            int pixelCount = source.width * source.height;
            TextureFormatInfo sourceInfo = source.formatInfo;
            TextureFormatInfo destinationInfo = destination.formatInfo;

            for (int i = 0; i < pixelCount; i++)
            {
                Vector4 color = sourceInfo.Decode(source._data, i);
                destinationInfo.Encode(destination._data, i, color);
            }
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
