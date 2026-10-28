using System.Diagnostics.CodeAnalysis;

namespace Photon.Core.Mathematics;

public struct Vector2 : IEquatable<Vector2>
{
    public float x;
    public float y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2(float value) : this(value, value)
    {
    }

    public float this[int index]
    {
        readonly get => index switch
        {
            0 => x,
            1 => y,
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
                default:
                    throw new IndexOutOfRangeException("Vector index is out of range.");
            }
        }
    }

    public readonly float length => GetLength(this);
    public readonly float lengthSquared => GetLengthSquared(this);
    public readonly Vector2 normalized => Normalize(this);

    public static Vector2 zero => new Vector2(0f, 0f);
    public static Vector2 one => new Vector2(1f, 1f);
    public static Vector2 unitX => new Vector2(1f, 0f);
    public static Vector2 unitY => new Vector2(0f, 1f);

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x + b.x, a.y + b.y);
    }

    public static Vector2 operator +(Vector2 a, float b)
    {
        return new Vector2(a.x + b, a.y + b);
    }

    public static Vector2 operator +(float a, Vector2 b)
    {
        return new Vector2(a + b.x, a + b.y);
    }

    public static Vector2 operator -(Vector2 v)
    {
        return new Vector2(-v.x, -v.y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x - b.x, a.y - b.y);
    }

    public static Vector2 operator -(Vector2 a, float b)
    {
        return new Vector2(a.x - b, a.y - b);
    }

    public static Vector2 operator -(float a, Vector2 b)
    {
        return new Vector2(a - b.x, a - b.y);
    }

    public static Vector2 operator *(Vector2 a, float b)
    {
        return new Vector2(a.x * b, a.y * b);
    }

    public static Vector2 operator *(float a, Vector2 b)
    {
        return new Vector2(a * b.x, a * b.y);
    }

    public static Vector2 operator /(Vector2 a, float b)
    {
        return new Vector2(a.x / b, a.y / b);
    }

    public static Vector2 operator /(float a, Vector2 b)
    {
        return new Vector2(a / b.x, a / b.y);
    }

    public static bool operator ==(Vector2 a, Vector2 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector2 a, Vector2 b)
    {
        return !a.Equals(b);
    }

    public readonly bool Equals(Vector2 other)
    {
        return x.Equals(other.x) && y.Equals(other.y);
    }

    public static bool Approximately(Vector2 a, Vector2 b)
    {
        return MathF.Approximately(a.x, b.x) && MathF.Approximately(a.y, b.y);
    }

    public static float Angle(Vector2 a, Vector2 b)
    {
        return MathF.Abs(SignedAngle(a, b));
    }

    public static float SignedAngle(Vector2 a, Vector2 b)
    {
        a = Normalize(a);
        b = Normalize(b);
        if (a == zero || b == zero)
        {
            return 0f;
        }

        return MathF.Atan2(Cross(a, b), Dot(a, b));
    }

    public static Vector2 Perpendicular(Vector2 v)
    {
        return new Vector2(-v.y, v.x);
    }

    public static Vector2 Clamp(Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(MathF.Clamp(v.x, min.x, max.x), MathF.Clamp(v.y, min.y, max.y));
    }

    public static Vector2 Clamp01(Vector2 v)
    {
        return new Vector2(MathF.Clamp01(v.x), MathF.Clamp01(v.y));
    }

    public static Vector2 ClampMagnitude(Vector2 v, float maxLength)
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

    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public static float Distance(Vector2 a, Vector2 b)
    {
        return (a - b).length;
    }

    public static float DistanceSquared(Vector2 a, Vector2 b)
    {
        return (a - b).lengthSquared;
    }

    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * MathF.Clamp01(t);
    }

    public static Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * t;
    }

    public static float GetLength(Vector2 v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared > 0f && float.IsFinite(lengthSquared))
        {
            return MathF.Sqrt(lengthSquared);
        }

        return (float)Math.Sqrt((double)v.x * v.x + (double)v.y * v.y);
    }

    public static float GetLengthSquared(Vector2 v)
    {
        return v.x * v.x + v.y * v.y;
    }

    public static Vector2 Min(Vector2 a, Vector2 b)
    {
        return new Vector2(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y));
    }

    public static Vector2 Max(Vector2 a, Vector2 b)
    {
        return new Vector2(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y));
    }

    public static Vector2 Normalize(Vector2 v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return zero;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            float maxComponent = MathF.Max(MathF.Abs(v.x), MathF.Abs(v.y));
            v /= maxComponent;
            lengthSquared = v.lengthSquared;
        }

        return v * (1f / MathF.Sqrt(lengthSquared));
    }

    public void Normalize()
    {
        this = Normalize(this);
    }

    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
    {
        Vector2 delta = target - current;
        float distance = delta.length;
        if (distance == 0f || distance <= maxDistanceDelta)
        {
            return target;
        }

        if (float.IsPositiveInfinity(distance))
        {
            double dx = (double)target.x - current.x;
            double dy = (double)target.y - current.y;
            double preciseDistance = Math.Sqrt(dx * dx + dy * dy);
            double factor = maxDistanceDelta / preciseDistance;
            return new Vector2((float)(current.x + dx * factor), (float)(current.y + dy * factor));
        }

        return current + delta * (maxDistanceDelta / distance);
    }

    public static Vector2 Project(Vector2 v, Vector2 normal)
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

    public static Vector2 Reflect(Vector2 inDirection, Vector2 normal)
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

    public static Vector2 Scale(Vector2 a, Vector2 b)
    {
        return new Vector2(a.x * b.x, a.y * b.y);
    }

    public void Set(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2 Transform(Vector2 v, Matrix3x3 matrix)
    {
        return TransformPoint(v, matrix);
    }

    public static Vector2 TransformDirection(Vector2 v, Matrix3x3 matrix)
    {
        return new Vector2(matrix.m11 * v.x + matrix.m12 * v.y, matrix.m21 * v.x + matrix.m22 * v.y);
    }

    public static Vector2 TransformNormal(Vector2 v, Matrix3x3 matrix)
    {
        Matrix3x3 linear = new Matrix3x3(
            matrix.m11,
            matrix.m12,
            0f,
            matrix.m21,
            matrix.m22,
            0f,
            0f,
            0f,
            1f);
        Matrix3x3 inverse = linear.inverse;
        return new Vector2(inverse.m11 * v.x + inverse.m21 * v.y, inverse.m12 * v.x + inverse.m22 * v.y);
    }

    public static Vector2 TransformPoint(Vector2 v, Matrix3x3 matrix)
    {
        float w = matrix.m31 * v.x + matrix.m32 * v.y + matrix.m33;
        return new Vector2(
            (matrix.m11 * v.x + matrix.m12 * v.y + matrix.m13) / w,
            (matrix.m21 * v.x + matrix.m22 * v.y + matrix.m23) / w);
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vector2 other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public readonly override string ToString()
    {
        return $"Vector2({x}, {y})";
    }
}
