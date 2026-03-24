using Photon.Math;
using Photon.Math.Vector;

namespace Photon.Core.Texture
{
    public class Texture_R11G11B10_UFloat : TextureFormatInfo
    {
        public override TextureFormat format => TextureFormat.R11G11B10_UFloat;
        public override int bytesPerPixel => 4;
        public override bool isFloat => true;
        public override bool isNormalized => false;

        public override Vector4 Decode(byte[] data, int pixelIndex)
        {
            int byteIndex = pixelIndex * bytesPerPixel;
            uint packed = BitConverter.ToUInt32(data, byteIndex);

            float r = DecodeUFloat((packed >> 21) & 0x7FF, 6);
            float g = DecodeUFloat((packed >> 10) & 0x7FF, 6);
            float b = DecodeUFloat(packed & 0x3FF, 5);

            return new Vector4(r, g, b, 1.0f);
        }

        public override void Encode(byte[] data, int pixelIndex, Vector4 color)
        {
            int byteIndex = pixelIndex * bytesPerPixel;

            uint rBits = EncodeUFloat(Mathf.Max(color.x, 0f), 6, 65024f);
            uint gBits = EncodeUFloat(Mathf.Max(color.y, 0f), 6, 65024f);
            uint bBits = EncodeUFloat(Mathf.Max(color.z, 0f), 5, 64512f);

            uint packed = (rBits << 21) | (gBits << 10) | bBits;
            byte[] packedBytes = BitConverter.GetBytes(packed);
            Array.Copy(packedBytes, 0, data, byteIndex, 4);
        }

        private float DecodeUFloat(uint bits, int mantissaBits)
        {
            const int exponentBits = 5;
            const int bias = 15;

            uint exponent = bits >> mantissaBits;
            uint mantissa = bits & ((1u << mantissaBits) - 1u);

            if (exponent == 0)
            {
                if (mantissa == 0)
                    return 0f;

                return mantissa * Mathf.Pow(2f, 1f - bias - mantissaBits);
            }

            if (exponent == (1u << exponentBits) - 1u)
            {
                return mantissa == 0 ? float.PositiveInfinity : float.NaN;
            }

            return (1f + mantissa / (float)(1u << mantissaBits)) * Mathf.Pow(2f, (int)exponent - bias);
        }

        private uint EncodeUFloat(float value, int mantissaBits, float maxFinite)
        {
            const int exponentBits = 5;
            const int bias = 15;

            if (value <= 0f || float.IsNaN(value))
                return 0u;

            if (value >= maxFinite)
            {
                uint maxExp = (1u << exponentBits) - 2u;
                uint maxMantissa = (1u << mantissaBits) - 1u;
                return (maxExp << mantissaBits) | maxMantissa;
            }

            float minNormal = Mathf.Pow(2f, -14f);

            if (value < minNormal)
            {
                uint mantissa = (uint)Mathf.Round(value / minNormal * (1u << mantissaBits));
                uint maxMantissa = (1u << mantissaBits) - 1u;
                if (mantissa > maxMantissa) mantissa = maxMantissa;
                return mantissa;
            }

            int exp = (int)Mathf.Floor(Mathf.Log(value, 2f));
            float normalized = value / Mathf.Pow(2f, exp);

            uint expBits = (uint)(exp + bias);
            uint mantissaBitsValue = (uint)Mathf.Round((normalized - 1f) * (1u << mantissaBits));

            if (mantissaBitsValue == (1u << mantissaBits))
            {
                mantissaBitsValue = 0u;
                expBits++;
            }

            if (expBits >= (1u << exponentBits) - 1u)
            {
                uint maxExp = (1u << exponentBits) - 2u;
                uint maxMantissa = (1u << mantissaBits) - 1u;
                return (maxExp << mantissaBits) | maxMantissa;
            }

            return (expBits << mantissaBits) | mantissaBitsValue;
        }
    }
}