using Photon.Math;
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.Texture
{
    public class Texture_D24_UNorm : TextureFormatInfo
    {
        public override TextureFormat format => TextureFormat.D24_UNorm;
        public override int bytesPerPixel => 3;
        public override bool isFloat => false;
        public override bool isNormalized => true;

        public override Vector4 Decode(byte[] data, int pixelIndex)
        {
            int byteIndex = pixelIndex * bytesPerPixel;

            uint depthBits = (uint)(data[byteIndex] | (data[byteIndex + 1] << 8) | (data[byteIndex + 2] << 16));
            float depth = depthBits / ((1u << 24) - 1f);

            return new Vector4(depth, 0f, 0f, 1f);
        }

        public override void Encode(byte[] data, int pixelIndex, Vector4 color)
        {
            int byteIndex = pixelIndex * bytesPerPixel;

            float depth = Mathf.Clamp(color.x, 0f, 1f);
            uint depthBits = (uint)Mathf.Round(depth * ((1u << 24) - 1u));

            data[byteIndex + 0] = (byte)(depthBits & 0xFF);
            data[byteIndex + 1] = (byte)((depthBits >> 8) & 0xFF);
            data[byteIndex + 2] = (byte)((depthBits >> 16) & 0xFF);
        }
    }
}
