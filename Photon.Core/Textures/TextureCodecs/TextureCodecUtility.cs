using Photon.Core.Mathematics;

namespace Photon.Core.Textures.TextureCodecs;

internal static class TextureCodecUtility
{
    internal static byte PackUNorm8(float value)
    {
        return Color32.Quantize(value);
    }

    internal static ushort PackUNorm16(float value)
    {
        if (!(value > 0f))
        {
            return 0;
        }

        return value >= 1f ? ushort.MaxValue : (ushort)Math.Round(value * 65535d, MidpointRounding.ToEven);
    }

    internal static uint PackUnsignedFloat(float value, int mantissaBits)
    {
        uint bits = BitConverter.SingleToUInt32Bits(value);
        uint magnitude = bits & 0x7FFFFFFFu;
        uint infinity = 31u << mantissaBits;
        uint mantissaMask = (1u << mantissaBits) - 1u;
        if (magnitude >= 0x7F800000u)
        {
            if (magnitude > 0x7F800000u)
            {
                return infinity | mantissaMask;
            }

            return (bits & 0x80000000u) == 0 ? infinity : 0u;
        }

        if ((bits & 0x80000000u) != 0)
        {
            return 0;
        }

        float maxValue = mantissaBits == 6 ? 65024f : 64512f;
        if (value >= maxValue)
        {
            return infinity - 1u;
        }

        if (magnitude < 0x38800000u)
        {
            return (uint)MathF.Round(MathF.ScaleB(value, 14 + mantissaBits), MidpointRounding.ToEven);
        }

        uint rebased = magnitude - 0x38000000u;
        int shift = 23 - mantissaBits;
        uint bias = (1u << (shift - 1)) - 1u;
        return (rebased + bias + ((rebased >> shift) & 1u)) >> shift;
    }

    internal static float UnpackUnsignedFloat(uint value, int mantissaBits)
    {
        uint exponent = value >> mantissaBits;
        uint mantissa = value & ((1u << mantissaBits) - 1u);
        if (exponent == 0)
        {
            return MathF.ScaleB(mantissa, -14 - mantissaBits);
        }

        uint floatExponent = exponent == 31 ? 255u : exponent + 112u;
        return BitConverter.UInt32BitsToSingle((floatExponent << 23) | (mantissa << (23 - mantissaBits)));
    }
}
