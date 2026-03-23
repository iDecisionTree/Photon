using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Photon.Core.Texture
{
    public abstract class TextureFormatInfo
    {
        public abstract TextureFormat format { get; }
        public abstract int bytesPerPixel { get; }
        public abstract bool isFloat { get; }
        public abstract bool isNormalized { get; }

        public int GetByteLength(int width, int height)
        {
            return width * height * bytesPerPixel;
        }

        public abstract Vector4 Decode(byte[] data, int pixelIndex);
        public abstract void Encode(byte[] data, int pixelIndex, Vector4 color);
    }
}
