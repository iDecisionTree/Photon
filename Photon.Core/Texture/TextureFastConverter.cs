using System.Runtime.CompilerServices;

namespace Photon.Core.Texture
{
    internal static class TextureFastConverter
    {
        private static readonly float[] UFloatExponentScale =
        [
            0f,
            3.0517578e-05f,
            6.1035156e-05f,
            0.00012207031f,
            0.00024414062f,
            0.00048828125f,
            0.0009765625f,
            0.001953125f,
            0.00390625f,
            0.0078125f,
            0.015625f,
            0.03125f,
            0.0625f,
            0.125f,
            0.25f,
            0.5f,
            1f,
            2f,
            4f,
            8f,
            16f,
            32f,
            64f,
            128f,
            256f,
            512f,
            1024f,
            2048f,
            4096f,
            8192f,
            16384f,
            32768f,
        ];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryConvert(TextureFormat sourceFormat, TextureFormat destinationFormat, byte[] sourceData, byte[] destinationData, int pixelCount)
        {
            if (sourceFormat == TextureFormat.R11G11B10_UFloat && destinationFormat == TextureFormat.B8G8R8A8_UNorm)
            {
                FastConvertR11G11B10UFloatToB8G8R8A8UNorm(sourceData, destinationData, pixelCount);
                return true;
            }

            return false;
        }

        private static void FastConvertR11G11B10UFloatToB8G8R8A8UNorm(byte[] sourceData, byte[] destinationData, int pixelCount)
        {
            const float invMantissa11 = 1f / 64f;
            const float invMantissa10 = 1f / 32f;
            const float subnormal11 = 9.536743e-07f;
            const float subnormal10 = 1.9073486e-06f;

            for (int i = 0; i < pixelCount; i++)
            {
                int sourceByteIndex = i * 4;
                uint packed = (uint)(sourceData[sourceByteIndex]
                    | (sourceData[sourceByteIndex + 1] << 8)
                    | (sourceData[sourceByteIndex + 2] << 16)
                    | (sourceData[sourceByteIndex + 3] << 24));

                uint rBits = (packed >> 21) & 0x7FF;
                uint gBits = (packed >> 10) & 0x7FF;
                uint bBits = packed & 0x3FF;

                byte r = ConvertUFloatToByte(rBits, 6, invMantissa11, subnormal11);
                byte g = ConvertUFloatToByte(gBits, 6, invMantissa11, subnormal11);
                byte b = ConvertUFloatToByte(bBits, 5, invMantissa10, subnormal10);

                int destinationByteIndex = i * 4;
                destinationData[destinationByteIndex] = b;
                destinationData[destinationByteIndex + 1] = g;
                destinationData[destinationByteIndex + 2] = r;
                destinationData[destinationByteIndex + 3] = byte.MaxValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ConvertUFloatToByte(uint bits, int mantissaBits, float invMantissaScale, float subnormalScale)
        {
            uint exponent = bits >> mantissaBits;
            uint mantissa = bits & ((1u << mantissaBits) - 1u);

            if (exponent == 0)
            {
                if (mantissa == 0)
                {
                    return 0;
                }

                return (byte)(mantissa * subnormalScale * 255f);
            }

            if (exponent == 0x1F)
            {
                return mantissa == 0 ? byte.MaxValue : (byte)0;
            }

            if (exponent >= 15)
            {
                return byte.MaxValue;
            }

            float value = (1f + mantissa * invMantissaScale) * UFloatExponentScale[exponent];
            return (byte)(value * 255f);
        }
    }
}
