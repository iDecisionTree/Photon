using Photon.Math;
using Photon.Math.Vector;

namespace Photon.Core.Texture
{
    public class Texture_B8G8R8A8_UNorm : TextureFormatInfo
    {
        public override TextureFormat format => TextureFormat.B8G8R8A8_UNorm;
        public override int bytesPerPixel => 4;
        public override bool isFloat => false;
        public override bool isNormalized => true;

        public override Vector4 Decode(byte[] data, int pixelIndex)
        {
            int byteIndex = pixelIndex * bytesPerPixel;

            byte b = data[byteIndex];
            byte g = data[byteIndex + 1];
            byte r = data[byteIndex + 2];
            byte a = data[byteIndex + 3];

            return new Vector4(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        public override void Encode(byte[] data, int pixelIndex, Vector4 color)
        {
            int byteIndex = pixelIndex * bytesPerPixel;

            byte r = (byte)Mathf.Clamp(color.x * 255f, 0f, 255f);
            byte g = (byte)Mathf.Clamp(color.y * 255f, 0f, 255f);
            byte b = (byte)Mathf.Clamp(color.z * 255f, 0f, 255f);
            byte a = (byte)Mathf.Clamp(color.w * 255f, 0f, 255f);

            data[byteIndex] = b;
            data[byteIndex + 1] = g;
            data[byteIndex + 2] = r;
            data[byteIndex + 3] = a;
        }
    }
}