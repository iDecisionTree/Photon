using Photon.Math;
using Photon.Math.Vector;

namespace Photon.Core.Texture
{
    public class Texture_R8G8B8A8_UNorm : TextureFormatInfo
    {
        public Texture_R8G8B8A8_UNorm()
        {
        }

        public override TextureFormat format => TextureFormat.R8G8B8A8_UNorm;
        public override int bytesPerPixel => 4;
        public override bool isFloat => false;
        public override bool isNormalized => true;

        public override Vector4 Decode(byte[] data, int pixelIndex)
        {
            int byteIndex = pixelIndex * bytesPerPixel;
            return new Vector4(
                data[byteIndex + 0] / 255f,
                data[byteIndex + 1] / 255f,
                data[byteIndex + 2] / 255f,
                data[byteIndex + 3] / 255f
            );
        }

        public override void Encode(byte[] data, int pixelIndex, Vector4 color)
        {
            int byteIndex = pixelIndex * bytesPerPixel;
            data[byteIndex + 0] = (byte)Mathf.Clamp(color.x * 255f, 0f, 255f);
            data[byteIndex + 1] = (byte)Mathf.Clamp(color.y * 255f, 0f, 255f);
            data[byteIndex + 2] = (byte)Mathf.Clamp(color.z * 255f, 0f, 255f);
            data[byteIndex + 3] = (byte)Mathf.Clamp(color.w * 255f, 0f, 255f);
        }
    }
}
