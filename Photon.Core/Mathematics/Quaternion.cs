using System.Diagnostics.CodeAnalysis;

namespace Photon.Core.Mathematics;

public struct Quaternion : IEquatable<Quaternion>
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Quaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public Quaternion(float value) : this(value, value, value, value)
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
            _ => throw new IndexOutOfRangeException("Quaternion index is out of range.")
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
                    throw new IndexOutOfRangeException("Quaternion index is out of range.");
            }
        }
    }

    public readonly Vector3 eulerAngles => ToEulerAngles(this);
    public readonly float length => GetLength(this);
    public readonly float lengthSquared => GetLengthSquared(this);
    public readonly Quaternion normalized => Normalize(this);

    public static Quaternion identity => new Quaternion(0f, 0f, 0f, 1f);

    public static Quaternion operator +(Quaternion a, Quaternion b)
    {
        return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    }

    public static Quaternion operator -(Quaternion v)
    {
        return new Quaternion(-v.x, -v.y, -v.z, -v.w);
    }

    public static Quaternion operator -(Quaternion a, Quaternion b)
    {
        return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    }

    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
            a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
            a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
    }

    public static Vector3 operator *(Quaternion a, Vector3 b)
    {
        float lengthSquared = a.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return b;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            a = Normalize(a);
            lengthSquared = a.lengthSquared;
        }

        float scale = 2f / lengthSquared;
        float x = a.x * scale;
        float y = a.y * scale;
        float z = a.z * scale;
        float xx = a.x * x;
        float yy = a.y * y;
        float zz = a.z * z;
        float xy = a.x * y;
        float xz = a.x * z;
        float yz = a.y * z;
        float wx = a.w * x;
        float wy = a.w * y;
        float wz = a.w * z;
        return new Vector3(
            (1f - yy - zz) * b.x + (xy - wz) * b.y + (xz + wy) * b.z,
            (xy + wz) * b.x + (1f - xx - zz) * b.y + (yz - wx) * b.z,
            (xz - wy) * b.x + (yz + wx) * b.y + (1f - xx - yy) * b.z);
    }

    public static Quaternion operator *(Quaternion a, float b)
    {
        return new Quaternion(a.x * b, a.y * b, a.z * b, a.w * b);
    }

    public static Quaternion operator *(float a, Quaternion b)
    {
        return new Quaternion(a * b.x, a * b.y, a * b.z, a * b.w);
    }

    public static Quaternion operator /(Quaternion a, float b)
    {
        return new Quaternion(a.x / b, a.y / b, a.z / b, a.w / b);
    }

    public static bool operator ==(Quaternion a, Quaternion b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Quaternion a, Quaternion b)
    {
        return !a.Equals(b);
    }

    public readonly bool Equals(Quaternion other)
    {
        return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
    }

    public static bool Approximately(Quaternion a, Quaternion b)
    {
        if (Dot(a, b) < 0f)
        {
            b = -b;
        }

        return MathF.Approximately(a.x, b.x) && MathF.Approximately(a.y, b.y) &&
               MathF.Approximately(a.z, b.z) && MathF.Approximately(a.w, b.w);
    }

    public static float Angle(Quaternion a, Quaternion b)
    {
        Quaternion delta = Conjugate(Normalize(a)) * Normalize(b);
        float sinHalfAngle = new Vector3(delta.x, delta.y, delta.z).length;
        return 2f * MathF.Atan2(sinHalfAngle, MathF.Abs(delta.w));
    }

    public static Quaternion Conjugate(Quaternion v)
    {
        return new Quaternion(-v.x, -v.y, -v.z, v.w);
    }

    public static float Dot(Quaternion a, Quaternion b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public static Quaternion FromAxisAngle(Vector3 axis, float angle)
    {
        Vector3 normal = axis.normalized;
        if (normal == Vector3.zero)
        {
            return identity;
        }

        float halfAngle = angle * 0.5f;
        (float sin, float cos) = MathF.SinCos(halfAngle);
        return new Quaternion(normal.x * sin, normal.y * sin, normal.z * sin, cos);
    }

    public static Quaternion FromEulerAngles(float x, float y, float z)
    {
        float halfX = x * 0.5f;
        float halfY = y * 0.5f;
        float halfZ = z * 0.5f;
        (float sinX, float cosX) = MathF.SinCos(halfX);
        (float sinY, float cosY) = MathF.SinCos(halfY);
        (float sinZ, float cosZ) = MathF.SinCos(halfZ);

        return new Quaternion(
            sinX * cosY * cosZ - cosX * sinY * sinZ,
            cosX * sinY * cosZ + sinX * cosY * sinZ,
            cosX * cosY * sinZ - sinX * sinY * cosZ,
            cosX * cosY * cosZ + sinX * sinY * sinZ);
    }

    public static Quaternion FromEulerAngles(Vector3 angles)
    {
        return FromEulerAngles(angles.x, angles.y, angles.z);
    }

    public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        Vector3 from = fromDirection.normalized;
        Vector3 to = toDirection.normalized;
        if (from == Vector3.zero || to == Vector3.zero)
        {
            return identity;
        }

        Vector3 cross = Vector3.Cross(from, to);
        float dot = MathF.Clamp(Vector3.Dot(from, to), -1f, 1f);
        float sin = cross.length;
        if (sin <= MathF.epsilon)
        {
            if (dot >= 0f)
            {
                return identity;
            }

            Vector3 axis = MathF.Abs(from.x) < MathF.Abs(from.z) ? Vector3.unitX : Vector3.unitZ;
            return FromAxisAngle(Vector3.Cross(from, axis), MathF.PI);
        }

        return FromAxisAngle(cross / sin, MathF.Atan2(sin, dot));
    }

    public static Quaternion FromMatrix(Matrix3x3 matrix)
    {
        float trace = matrix.m11 + matrix.m22 + matrix.m33;
        Quaternion rotation;
        if (trace > 0f)
        {
            float scale = MathF.Sqrt(trace + 1f) * 2f;
            rotation = new Quaternion(
                (matrix.m32 - matrix.m23) / scale,
                (matrix.m13 - matrix.m31) / scale,
                (matrix.m21 - matrix.m12) / scale,
                scale * 0.25f);
        }
        else if (matrix.m11 > matrix.m22 && matrix.m11 > matrix.m33)
        {
            float scale = MathF.Sqrt(1f + matrix.m11 - matrix.m22 - matrix.m33) * 2f;
            rotation = new Quaternion(
                scale * 0.25f,
                (matrix.m12 + matrix.m21) / scale,
                (matrix.m13 + matrix.m31) / scale,
                (matrix.m32 - matrix.m23) / scale);
        }
        else if (matrix.m22 > matrix.m33)
        {
            float scale = MathF.Sqrt(1f + matrix.m22 - matrix.m11 - matrix.m33) * 2f;
            rotation = new Quaternion(
                (matrix.m12 + matrix.m21) / scale,
                scale * 0.25f,
                (matrix.m23 + matrix.m32) / scale,
                (matrix.m13 - matrix.m31) / scale);
        }
        else
        {
            float scale = MathF.Sqrt(1f + matrix.m33 - matrix.m11 - matrix.m22) * 2f;
            rotation = new Quaternion(
                (matrix.m13 + matrix.m31) / scale,
                (matrix.m23 + matrix.m32) / scale,
                scale * 0.25f,
                (matrix.m21 - matrix.m12) / scale);
        }

        return Normalize(rotation);
    }

    public static Quaternion FromMatrix(Matrix4x4 matrix)
    {
        return FromMatrix(new Matrix3x3(
            matrix.m11,
            matrix.m12,
            matrix.m13,
            matrix.m21,
            matrix.m22,
            matrix.m23,
            matrix.m31,
            matrix.m32,
            matrix.m33));
    }

    public static Quaternion Inverse(Quaternion v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return identity;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            double preciseLengthSquared = (double)v.x * v.x + (double)v.y * v.y + (double)v.z * v.z + (double)v.w * v.w;
            return new Quaternion(
                (float)(-v.x / preciseLengthSquared),
                (float)(-v.y / preciseLengthSquared),
                (float)(-v.z / preciseLengthSquared),
                (float)(v.w / preciseLengthSquared));
        }

        return Conjugate(v) / lengthSquared;
    }

    public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
    {
        return LerpUnclamped(a, b, MathF.Clamp01(t));
    }

    public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
    {
        a = Normalize(a);
        b = Normalize(b);
        if (Dot(a, b) < 0f)
        {
            b = -b;
        }

        return Normalize(a + (b - a) * t);
    }

    public static float GetLength(Quaternion v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared > 0f && float.IsFinite(lengthSquared))
        {
            return MathF.Sqrt(lengthSquared);
        }

        return (float)Math.Sqrt((double)v.x * v.x + (double)v.y * v.y + (double)v.z * v.z + (double)v.w * v.w);
    }

    public static float GetLengthSquared(Quaternion v)
    {
        return v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w;
    }

    public static Quaternion Normalize(Quaternion v)
    {
        float lengthSquared = v.lengthSquared;
        if (lengthSquared <= MathF.epsilon * MathF.epsilon)
        {
            return identity;
        }

        if (float.IsPositiveInfinity(lengthSquared))
        {
            float maxComponent = MathF.Max(MathF.Max(MathF.Abs(v.x), MathF.Abs(v.y)), MathF.Max(MathF.Abs(v.z), MathF.Abs(v.w)));
            v /= maxComponent;
            lengthSquared = v.lengthSquared;
        }

        return v * (1f / MathF.Sqrt(lengthSquared));
    }

    public void Normalize()
    {
        this = Normalize(this);
    }

    public static Quaternion LookRotation(Vector3 forward)
    {
        return LookRotation(forward, Vector3.unitY);
    }

    public static Quaternion LookRotation(Vector3 forward, Vector3 up)
    {
        forward = forward.normalized;
        if (forward == Vector3.zero)
        {
            return identity;
        }

        Vector3 right = Vector3.Cross(up.normalized, forward).normalized;
        if (right == Vector3.zero)
        {
            return FromToRotation(Vector3.unitZ, forward);
        }

        up = Vector3.Cross(forward, right);
        return FromMatrix(new Matrix3x3(
            right.x,
            up.x,
            forward.x,
            right.y,
            up.y,
            forward.y,
            right.z,
            up.z,
            forward.z));
    }

    public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxAngleDelta)
    {
        float angle = Angle(from, to);
        if (angle == 0f)
        {
            return Normalize(to);
        }

        float delta = MathF.Max(maxAngleDelta, angle - MathF.PI);
        return SlerpUnclamped(from, to, MathF.Min(1f, delta / angle));
    }

    public void Set(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
    {
        return SlerpUnclamped(a, b, MathF.Clamp01(t));
    }

    public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
    {
        a = Normalize(a);
        b = Normalize(b);
        float dot = Dot(a, b);
        if (dot < 0f)
        {
            b = -b;
            dot = -dot;
        }

        dot = MathF.Clamp(dot, -1f, 1f);
        if (dot > 0.9995f)
        {
            if (t < 0f || t > 1f)
            {
                Quaternion delta = Conjugate(a) * b;
                float sinHalfAngle = new Vector3(delta.x, delta.y, delta.z).length;
                if (sinHalfAngle == 0f)
                {
                    return a;
                }

                float halfAngle = MathF.Atan2(sinHalfAngle, delta.w);
                (float sinT, float cosT) = MathF.SinCos(t * halfAngle);
                float scale = sinT / sinHalfAngle;
                return Normalize(a * new Quaternion(delta.x * scale, delta.y * scale, delta.z * scale, cosT));
            }

            return Normalize(a + (b - a) * t);
        }

        float angle = MathF.Acos(dot);
        float sin = MathF.Sin(angle);
        float aWeight = MathF.Sin((1f - t) * angle) / sin;
        float bWeight = MathF.Sin(t * angle) / sin;
        return Normalize(a * aWeight + b * bWeight);
    }

    public static void ToAxisAngle(Quaternion v, out Vector3 axis, out float angle)
    {
        v = Normalize(v);
        if (v.w < 0f)
        {
            v = -v;
        }

        axis = new Vector3(v.x, v.y, v.z);
        float sinHalfAngle = axis.length;
        angle = 2f * MathF.Atan2(sinHalfAngle, v.w);
        axis = sinHalfAngle == 0f ? Vector3.unitX : axis / sinHalfAngle;
    }

    public static Vector3 ToEulerAngles(Quaternion v)
    {
        v = Normalize(v);
        double x = v.x;
        double y = v.y;
        double z = v.z;
        double w = v.w;
        double sinY = 2d * (w * y - z * x);
        double cosYCosZ = w * w + x * x - y * y - z * z;
        double cosYSinZ = 2d * (x * y + w * z);
        double cosY = Math.Sqrt(cosYCosZ * cosYCosZ + cosYSinZ * cosYSinZ);
        float angleY = (float)Math.Atan2(sinY, cosY);
        if (cosY <= MathF.epsilon)
        {
            float angleX = (float)Math.Atan2(2d * (w * x - y * z), w * w - x * x + y * y - z * z);
            return new Vector3(angleX, angleY, 0f);
        }

        return new Vector3(
            (float)Math.Atan2(2d * (w * x + y * z), w * w - x * x - y * y + z * z),
            angleY,
            (float)Math.Atan2(cosYSinZ, cosYCosZ));
    }

    public static Matrix3x3 ToMatrix3x3(Quaternion v)
    {
        return Matrix3x3.FromQuaternion(v);
    }

    public static Matrix4x4 ToMatrix4x4(Quaternion v)
    {
        return Matrix4x4.FromQuaternion(v);
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Quaternion other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }

    public readonly override string ToString()
    {
        return $"Quaternion({x}, {y}, {z}, {w})";
    }
}
