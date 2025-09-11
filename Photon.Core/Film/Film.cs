/// 胶片由相机类管理

using System.Text;

namespace Photon.Core
{
    public class Film
    {
        public int width
        {
            get => _width;
            set => _width = value;
        }

        public int height
        {
            get => _height;
            set => _height = value;
        }

        private int _width;
        private int _height;
        private byte[] _pixels;

        public Film(int width = 1920, int height = 1080)
        {
            _width = width;
            _height = height;
            _pixels = new byte[width * height * 3];
        }

        public void SetPixel(int x, int y, Color color)
        {
            int index = (y * _width + x) * 3;

            _pixels[index] = color.r;
            _pixels[index + 1] = color.g;
            _pixels[index + 2] = color.b;
        }

        public Color GetPixel(int x, int y)
        {
            int index = (y * _width + x) * 3;

            return new Color(_pixels[index], _pixels[index + 1], _pixels[index + 2]);
        }

        public void Clear()
        {
            Array.Clear(_pixels);
        }

        public void Save(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(Encoding.UTF8.GetBytes("P6\n"));
                fs.Write(Encoding.UTF8.GetBytes($"{_width} {_height}\n"));
                fs.Write(Encoding.UTF8.GetBytes("255\n"));
                fs.Write(_pixels);
            }
        }
    }
}
