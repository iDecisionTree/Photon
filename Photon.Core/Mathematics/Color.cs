using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Photon.Core.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public struct Color : IEquatable<Color>, IFormattable
{
    public float r;
    public float g;
    public float b;
    public float a;

    public Color(float r, float g, float b, float a = 1f)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public float this[int index]
    {
        readonly get => index switch
        {
            0 => r,
            1 => g,
            2 => b,
            3 => a,
            _ => throw new IndexOutOfRangeException("Color index is out of range.")
        };
        set
        {
            switch (index)
            {
                case 0:
                    r = value;
                    break;
                case 1:
                    g = value;
                    break;
                case 2:
                    b = value;
                    break;
                case 3:
                    a = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Color index is out of range.");
            }
        }
    }

    public readonly float grayscale => 0.299f * r + 0.587f * g + 0.114f * b;
    public readonly float maxColorComponent => MathF.Max(MathF.Max(r, g), b);
    public readonly Color linear => new Color(SrgbToLinear(r), SrgbToLinear(g), SrgbToLinear(b), a);
    public readonly Color gamma => new Color(LinearToSrgb(r), LinearToSrgb(g), LinearToSrgb(b), a);

    public static Color clear => new Color(0f, 0f, 0f, 0f);
    public static Color black => new Color(0f, 0f, 0f);
    public static Color white => new Color(1f, 1f, 1f);
    public static Color red => new Color(1f, 0f, 0f);
    public static Color green => new Color(0f, 1f, 0f);
    public static Color blue => new Color(0f, 0f, 1f);
    public static Color yellow => new Color(1f, 1f, 0f);
    public static Color cyan => new Color(0f, 1f, 1f);
    public static Color magenta => new Color(1f, 0f, 1f);

    public static Color operator +(Color a, Color b)
    {
        return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
    }

    public static Color operator -(Color a, Color b)
    {
        return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
    }

    public static Color operator *(Color a, Color b)
    {
        return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a);
    }

    public static Color operator *(Color color, float scale)
    {
        return new Color(color.r * scale, color.g * scale, color.b * scale, color.a * scale);
    }

    public static Color operator *(float scale, Color color)
    {
        return color * scale;
    }

    public static Color operator /(Color color, float scale)
    {
        return new Color(color.r / scale, color.g / scale, color.b / scale, color.a / scale);
    }

    public static bool operator ==(Color a, Color b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Color a, Color b)
    {
        return !a.Equals(b);
    }

    public static explicit operator Vector4(Color color)
    {
        return new Vector4(color.r, color.g, color.b, color.a);
    }

    public static explicit operator Color(Vector4 vector)
    {
        return new Color(vector.x, vector.y, vector.z, vector.w);
    }

    public static bool Approximately(Color a, Color b)
    {
        return MathF.Approximately(a.r, b.r) && MathF.Approximately(a.g, b.g) &&
               MathF.Approximately(a.b, b.b) && MathF.Approximately(a.a, b.a);
    }

    public static Color Lerp(Color a, Color b, float t)
    {
        return LerpUnclamped(a, b, MathF.Clamp01(t));
    }

    public static Color LerpUnclamped(Color a, Color b, float t)
    {
        return new Color(
            LerpComponent(a.r, b.r, t),
            LerpComponent(a.g, b.g, t),
            LerpComponent(a.b, b.b, t),
            LerpComponent(a.a, b.a, t));
    }

    public static float SrgbToLinear(float value)
    {
        return value <= 0.04045f ? value / 12.92f : MathF.Pow((value + 0.055f) / 1.055f, 2.4f);
    }

    public static float LinearToSrgb(float value)
    {
        return value <= 0.0031308f ? value * 12.92f : 1.055f * MathF.Pow(value, 1f / 2.4f) - 0.055f;
    }

    public readonly bool Equals(Color other)
    {
        return r.Equals(other.r) && g.Equals(other.g) && b.Equals(other.b) && a.Equals(other.a);
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Color other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(r, g, b, a);
    }

    public readonly override string ToString()
    {
        return ToString(null, CultureInfo.InvariantCulture);
    }

    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        formatProvider ??= CultureInfo.InvariantCulture;
        return $"Color({r.ToString(format, formatProvider)}, {g.ToString(format, formatProvider)}, " +
               $"{b.ToString(format, formatProvider)}, {a.ToString(format, formatProvider)})";
    }

    private static float LerpComponent(float a, float b, float t)
    {
        float result = MathF.LerpUnclamped(a, b, t);
        if (float.IsNaN(result) && float.IsFinite(t) && (float.IsInfinity(a) || float.IsInfinity(b)))
        {
            return (float)((1d - t) * a + (double)t * b);
        }

        return result;
    }
}
