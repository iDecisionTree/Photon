using System.Diagnostics.CodeAnalysis;

namespace Photon.Core.Mathematics;

public struct Vector3 : IEquatable<Vector3>
{
    public float x;
    public float y;
    public float z;

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3(float value) : this(value, value, value)
    {
    }

    public float this[int index]
    {
        readonly get => index switch
        {
            0 => x,
            1 => y,
            2 => z,
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
                default:
                    throw new IndexOutOfRangeException("Vector index is out of range.");
            }
        }
    }

    public readonly float length => GetLength(this);
    public readonly float lengthSquared => GetLengthSquared(this);
    public readonly Vector3 normalized => Normalize(this);

    public static Vector3 zero => new Vector3(0f, 0f, 0f);
    public static Vector3 one => new Vector3(1f, 1f, 1f);
    public static Vector3 unitX => new Vector3(1f, 0f, 0f);
    public static Vector3 unitY => new Vector3(0f, 1f, 0f);
    public static Vector3 unitZ => new Vector3(0f, 0f, 1f);

    public static Vector3 operator +(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Vector3 operator +(Vector3 a, float b)
    {
        return new Vector3(a.x + b, a.y + b, a.z + b);
    }

    public static Vector3 operator +(float a, Vector3 b)
    {
        return new Vector3(a + b.x, a + b.y, a + b.z);
    }

    public static Vector3 operator -(Vector3 v)
    {
        return new Vector3(-v.x, -v.y, -v.z);
    }

    public static Vector3 operator -(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vector3 operator -(Vector3 a, float b)
    {
        return new Vector3(a.x - b, a.y - b, a.z - b);
    }

    public static Vector3 operator -(float a, Vector3 b)
    {
        return new Vector3(a - b.x, a - b.y, a - b.z);
    }

    public static Vector3 operator *(Vector3 a, float b)
    {
        return new Vector3(a.x * b, a.y * b, a.z * b);
    }

    public static Vector3 operator *(float a, Vector3 b)
    {
        return new Vector3(a * b.x, a * b.y, a * b.z);
    }

    public static Vector3 operator /(Vector3 a, float b)
    {
        return new Vector3(a.x / b, a.y / b, a.z / b);
    }

    public static Vector3 operator /(float a, Vector3 b)
    {
        return new Vector3(a / b.x, a / b.y, a / b.z);
    }

    public static bool operator ==(Vector3 a, Vector3 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector3 a, Vector3 b)
    {
        return !a.Equals(b);
    }

    public readonly bool Equals(Vector3 other)
    {
        return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
    }

    public static bool Approximately(Vector3 a, Vector3 b)
    {
        return MathF.Approximately(a.x, b.x) && MathF.Approximately(a.y, b.y) && MathF.Approximately(a.z, b.z);
    }

    public static float Angle(Vector3 a, Vector3 b)
    {
        a = Normalize(a);
        b = Normalize(b);
        if (a == zero || b == zero)
        {
            return 0f;
        }

        return MathF.Atan2(Cross(a, b).length, Dot(a, b));
    }

    public static float SignedAngle(Vector3 a, Vector3 b, Vector3 axis)
    {
        a = Normalize(a);
        b = Normalize(b);
        if (a == zero || b == zero)
        {
            return 0f;
        }

        Vector3 cross = Cross(a, b);
        float angle = MathF.Atan2(cross.length, Dot(a, b));
        float sign = Dot(axis, cross);
        return sign < 0f ? -angle : angle;
    }

    public static Vector3 ProjectOnPlane(Vector3 v, Vector3 normal)
    {
        return v - Project(v, normal);
    }

    public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(MathF.Clamp(v.x, min.x, max.x), MathF.Clamp(v.y, min.y, max.y), MathF.Clamp(v.z, min.z, max.z));
    }

    public static Vector3 Clamp01(Vector3 v)
    {
        return new Vector3(MathF.Clamp01(v.x), MathF.Clamp01(v.y), MathF.Clamp01(v.z));
    }

    public static Vector3 ClampMagnitude(Vector3 v, float maxLength)
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

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
        return (a - b).length;
    }

    public static float DistanceSquared(Vector3 a, Vector3 b)
    {
        return (a - b).lengthSquared;
    }

    public static float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * MathF.Clamp01(t);
    }

    public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }

    public static float GetLength(Vector3 v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared > 0f && float.IsFinite(lengthSquared))
        {
            return MathF.Sqrt(lengthSquared);
        }

        return (float)Math.Sqrt((double)v.x * v.x + (double)v.y * v.y + (double)v.z * v.z);
    }

    public static float GetLengthSquared(Vector3 v)
    {
        return v.x * v.x + v.y * v.y + v.z * v.z;
    }

    public static Vector3 Min(Vector3 a, Vector3 b)
    {
        return new Vector3(MathF.Min(a.x, b.x), MathF.Min(a.y, b.y), MathF.Min(a.z, b.z));
    }

    public static Vector3 Max(Vector3 a, Vector3 b)
    {
        return new Vector3(MathF.Max(a.x, b.x), MathF.Max(a.y, b.y), MathF.Max(a.z, b.z));
    }

    public static Vector3 Normalize(Vector3 v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return zero;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            float maxComponent = MathF.Max(MathF.Max(MathF.Abs(v.x), MathF.Abs(v.y)), MathF.Abs(v.z));
            v /= maxComponent;
            lengthSquared = v.lengthSquared;
        }

        return v * (1f / MathF.Sqrt(lengthSquared));
    }

    public void Normalize()
    {
        this = Normalize(this);
    }

    public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    {
        Vector3 delta = target - current;
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
            double preciseDistance = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double factor = maxDistanceDelta / preciseDistance;
            return new Vector3((float)(current.x + dx * factor), (float)(current.y + dy * factor), (float)(current.z + dz * factor));
        }

        return current + delta * (maxDistanceDelta / distance);
    }

    public static Vector3 Project(Vector3 v, Vector3 normal)
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

    public static Vector3 Reflect(Vector3 inDirection, Vector3 normal)
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

    public static Vector3 Scale(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public void Set(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static Vector3 Transform(Vector3 v, Matrix3x3 matrix)
    {
        return matrix * v;
    }

    public static Vector3 Transform(Vector3 v, Matrix4x4 matrix)
    {
        return TransformPoint(v, matrix);
    }

    public static Vector3 TransformDirection(Vector3 v, Matrix3x3 matrix)
    {
        return matrix * v;
    }

    public static Vector3 TransformDirection(Vector3 v, Matrix4x4 matrix)
    {
        return new Vector3(
            matrix.m11 * v.x + matrix.m12 * v.y + matrix.m13 * v.z,
            matrix.m21 * v.x + matrix.m22 * v.y + matrix.m23 * v.z,
            matrix.m31 * v.x + matrix.m32 * v.y + matrix.m33 * v.z);
    }

    public static Vector3 TransformNormal(Vector3 v, Matrix3x3 matrix)
    {
        return matrix.inverse.transposed * v;
    }

    public static Vector3 TransformNormal(Vector3 v, Matrix4x4 matrix)
    {
        Matrix3x3 linear = new Matrix3x3(
            matrix.m11,
            matrix.m12,
            matrix.m13,
            matrix.m21,
            matrix.m22,
            matrix.m23,
            matrix.m31,
            matrix.m32,
            matrix.m33);
        return TransformNormal(v, linear);
    }

    public static Vector3 TransformPoint(Vector3 v, Matrix4x4 matrix)
    {
        float w = matrix.m41 * v.x + matrix.m42 * v.y + matrix.m43 * v.z + matrix.m44;
        return new Vector3(
            (matrix.m11 * v.x + matrix.m12 * v.y + matrix.m13 * v.z + matrix.m14) / w,
            (matrix.m21 * v.x + matrix.m22 * v.y + matrix.m23 * v.z + matrix.m24) / w,
            (matrix.m31 * v.x + matrix.m32 * v.y + matrix.m33 * v.z + matrix.m34) / w);
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vector3 other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    public readonly override string ToString()
    {
        return $"Vector3({x}, {y}, {z})";
    }
}
