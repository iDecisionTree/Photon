using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Photon.Core.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public struct Color32 : IEquatable<Color32>
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public Color32(byte r, byte g, byte b, byte a = byte.MaxValue)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public static implicit operator Color(Color32 color)
    {
        return new Color(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
    }

    public static explicit operator Color32(Color color)
    {
        return new Color32(Quantize(color.r), Quantize(color.g), Quantize(color.b), Quantize(color.a));
    }

    public static bool operator ==(Color32 a, Color32 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Color32 a, Color32 b)
    {
        return !a.Equals(b);
    }

    public readonly bool Equals(Color32 other)
    {
        return r == other.r && g == other.g && b == other.b && a == other.a;
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Color32 other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(r, g, b, a);
    }

    public readonly override string ToString()
    {
        return $"Color32({r}, {g}, {b}, {a})";
    }

    internal static byte Quantize(float value)
    {
        if (!(value > 0f))
        {
            return 0;
        }

        return value >= 1f ? byte.MaxValue : (byte)Math.Round(value * 255d, MidpointRounding.ToEven);
    }
}
