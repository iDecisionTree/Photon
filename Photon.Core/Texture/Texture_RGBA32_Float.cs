using Photon.Math.Vector;

namespace Photon.Core.Texture
{
    public class Texture_RGBA32_Float : TextureFormatInfo
    {
        public override TextureFormat format => TextureFormat.RGBA32_Float;
        public override int bytesPerPixel => 16;
        public override bool isFloat => true;
        public override bool isNormalized => false;

        public override Vector4 Decode(byte[] data, int pixelIndex)
        {
            int byteIndex = pixelIndex * bytesPerPixel;
            float r = BitConverter.ToSingle(data, byteIndex);
            float g = BitConverter.ToSingle(data, byteIndex + 4);
            float b = BitConverter.ToSingle(data, byteIndex + 8);
            float a = BitConverter.ToSingle(data, byteIndex + 12);

            return new Vector4(r, g, b, a);
        }

        public override void Encode(byte[] data, int pixelIndex, Vector4 color)
        {
            int byteIndex = pixelIndex * bytesPerPixel;
            byte[] rBytes = BitConverter.GetBytes(color.x);
            byte[] gBytes = BitConverter.GetBytes(color.y);
            byte[] bBytes = BitConverter.GetBytes(color.z);
            byte[] aBytes = BitConverter.GetBytes(color.w);

            Array.Copy(rBytes, 0, data, byteIndex + 0, 4);
            Array.Copy(gBytes, 0, data, byteIndex + 4, 4);
            Array.Copy(bBytes, 0, data, byteIndex + 8, 4);
            Array.Copy(aBytes, 0, data, byteIndex + 12, 4);
        }
    }
}