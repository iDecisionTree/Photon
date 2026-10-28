using System.Diagnostics.CodeAnalysis;

namespace Photon.Core.Mathematics;

public struct Vector4 : IEquatable<Vector4>
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Vector4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Vector4(float value) : this(value, value, value, value)
    {
    }

    public float this[int index]
    {
        readonly get => index switch
        {
            0 => x,
            1 => y,
            2 => z,
            3 => w,
            _ => throw new IndexOutOfRangeException("Vector index is out of range.")
        };
        set
        {
            switch (index)
            {
                case 0:
                    x = value;
                    break;
                case 1:
                    y = value;
                    break;
                case 2:
                    z = value;
                    break;
                case 3:
                    w = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Vector index is out of range.");
            }
        }
    }

    public readonly float length => GetLength(this);
    public readonly float lengthSquared => GetLengthSquared(this);
    public readonly Vector4 normalized => Normalize(this);

    public static Vector4 zero => new Vector4(0f, 0f, 0f, 0f);
    public static Vector4 one => new Vector4(1f, 1f, 1f, 1f);
    public static Vector4 unitX => new Vector4(1f, 0f, 0f, 0f);
    public static Vector4 unitY => new Vector4(0f, 1f, 0f, 0f);
    public static Vector4 unitZ => new Vector4(0f, 0f, 1f, 0f);
    public static Vector4 unitW => new Vector4(0f, 0f, 0f, 1f);

    public static Vector4 operator +(Vector4 a, Vector4 b)
    {
        return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    }

    public static Vector4 operator +(Vector4 a, float b)
    {
        return new Vector4(a.x + b, a.y + b, a.z + b, a.w + b);
    }

    public static Vector4 operator +(float a, Vector4 b)
    {
        return new Vector4(a + b.x, a + b.y, a + b.z, a + b.w);
    }

    public static Vector4 operator -(Vector4 v)
    {
        return new Vector4(-v.x, -v.y, -v.z, -v.w);
    }

    public static Vector4 operator -(Vector4 a, Vector4 b)
    {
        return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    }

    public static Vector4 operator -(Vector4 a, float b)
    {
        return new Vector4(a.x - b, a.y - b, a.z - b, a.w - b);
    }

    public static Vector4 operator -(float a, Vector4 b)
    {
        return new Vector4(a - b.x, a - b.y, a - b.z, a - b.w);
    }

    public static Vector4 operator *(Vector4 a, float b)
    {
        return new Vector4(a.x * b, a.y * b, a.z * b, a.w * b);
    }

    public static Vector4 operator *(float a, Vector4 b)
    {
        return new Vector4(a * b.x, a * b.y, a * b.z, a * b.w);
    }

    public static Vector4 operator /(Vector4 a, float b)
    {
        return new Vector4(a.x / b, a.y / b, a.z / b, a.w / b);
    }

    public static Vector4 operator /(float a, Vector4 b)
    {
        return new Vector4(a / b.x, a / b.y, a / b.z, a / b.w);
    }

    public static bool operator ==(Vector4 a, Vector4 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector4 a, Vector4 b)
    {
        return !a.Equals(b);
    }

    public readonly bool Equals(Vector4 other)
    {
        return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
    }

    public static bool Approximately(Vector4 a, Vector4 b)
    {
        return MathF.Approximately(a.x, b.x) && MathF.Approximately(a.y, b.y) &&
               MathF.Approximately(a.z, b.z) && MathF.Approximately(a.w, b.w);
    }

    public static Vector4 Clamp(Vector4 v, Vector4 min, Vector4 max)
    {
        return new Vector4(
            MathF.Clamp(v.x, min.x, max.x),
            MathF.Clamp(v.y, min.y, max.y),
            MathF.Clamp(v.z, min.z, max.z),
            MathF.Clamp(v.w, min.w, max.w));
    }

    public static Vector4 Clamp01(Vector4 v)
    {
        return new Vector4(MathF.Clamp01(v.x), MathF.Clamp01(v.y), MathF.Clamp01(v.z), MathF.Clamp01(v.w));
    }

    public static Vector4 ClampMagnitude(Vector4 v, float maxLength)
    {
        if (float.IsNaN(maxLength) || maxLength < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(maxLength));
        }

        float length = v.length;
        if (length <= maxLength || length == 0f)
        {
            return v;
        }

        if (float.IsPositiveInfinity(length))
        {
            return v.normalized * maxLength;
        }

        return v * (maxLength / length);
    }

    public static float Distance(Vector4 a, Vector4 b)
    {
        return (a - b).length;
    }

    public static float DistanceSquared(Vector4 a, Vector4 b)
    {
        return (a - b).lengthSquared;
    }

    public static float Dot(Vector4 a, Vector4 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
    {
        return a + (b - a) * MathF.Clamp01(t);
    }

    public static Vector4 LerpUnclamped(Vector4 a, Vector4 b, float t)
    {
        return a + (b - a) * t;
    }

    public static float GetLength(Vector4 v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared > 0f && float.IsFinite(lengthSquared))
        {
            return MathF.Sqrt(lengthSquared);
        }

        return (float)Math.Sqrt((double)v.x * v.x + (double)v.y * v.y + (double)v.z * v.z + (double)v.w * v.w);
    }

    public static float GetLengthSquared(Vector4 v)
    {
        return v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w;
    }

    public static Vector4 Min(Vector4 a, Vector4 b)
    {
        return new Vector4(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y), MathF.Min(a.z, b.z), MathF.Min(a.w, b.w));
    }

    public static Vector4 Max(Vector4 a, Vector4 b)
    {
        return new Vector4(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y), MathF.Max(a.z, b.z), MathF.Max(a.w, b.w));
    }

    public static Vector4 Normalize(Vector4 v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return zero;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            float maxComponent = MathF.Max(MathF.Max(MathF.Max(MathF.Abs(v.x), MathF.Abs(v.y)), MathF.Abs(v.z)), MathF.Abs(v.w));
            v /= maxComponent;
            lengthSquared = v.lengthSquared;
        }

        return v * (1f / MathF.Sqrt(lengthSquared));
    }

    public void Normalize()
    {
        this = Normalize(this);
    }

    public static Vector4 MoveTowards(Vector4 current, Vector4 target, float maxDistanceDelta)
    {
        Vector4 delta = target - current;
        float distance = delta.length;
        if (distance == 0f || distance <= maxDistanceDelta)
        {
            return target;
        }

        if (float.IsPositiveInfinity(distance))
        {
            double dx = (double)target.x - current.x;
            double dy = (double)target.y - current.y;
            double dz = (double)target.z - current.z;
            double dw = (double)target.w - current.w;
            double preciseDistance = Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
            double factor = maxDistanceDelta / preciseDistance;
            return new Vector4(
                (float)(current.x + dx * factor),
                (float)(current.y + dy * factor),
                (float)(current.z + dz * factor),
                (float)(current.w + dw * factor));
        }

        return current + delta * (maxDistanceDelta / distance);
    }

    public static Vector4 Project(Vector4 v, Vector4 normal)
    {
        float lengthSquared = normal.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return zero;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            normal = Normalize(normal);
            lengthSquared = normal.lengthSquared;
        }

        return normal * (Dot(v, normal) / lengthSquared);
    }

    public static Vector4 Reflect(Vector4 inDirection, Vector4 normal)
    {
        float lengthSquared = normal.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return inDirection;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            normal = Normalize(normal);
            lengthSquared = normal.lengthSquared;
        }

        float factor = -2f * Dot(inDirection, normal) / lengthSquared;
        return inDirection + normal * factor;
    }

    public static Vector4 Scale(Vector4 a, Vector4 b)
    {
        return new Vector4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
    }

    public void Set(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Vector4 Transform(Vector4 v, Matrix4x4 matrix)
    {
        return matrix * v;
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vector4 other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }

    public readonly override string ToString()
    {
        return $"Vector4({x}, {y}, {z}, {w})";
    }
}
