using System.Diagnostics.CodeAnalysis;

namespace Photon.Core.Mathematics;

public struct Matrix3x3 : IEquatable<Matrix3x3>
{
    public float m11;
    public float m12;
    public float m13;
    public float m21;
    public float m22;
    public float m23;
    public float m31;
    public float m32;
    public float m33;

    public Matrix3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
    {
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
    }

    public Matrix3x3(float value) : this(value, value, value, value, value, value, value, value, value)
    {
    }

    public float this[int row, int column]
    {
        readonly get
        {
            if ((uint)row >= 3 || (uint)column >= 3)
            {
                throw new IndexOutOfRangeException("Matrix index is out of range.");
            }

            return (row * 3 + column) switch
            {
                0 => m11,
                1 => m12,
                2 => m13,
                3 => m21,
                4 => m22,
                5 => m23,
                6 => m31,
                7 => m32,
                8 => m33,
                _ => throw new IndexOutOfRangeException("Matrix index is out of range.")
            };
        }
        set
        {
            if ((uint)row >= 3 || (uint)column >= 3)
            {
                throw new IndexOutOfRangeException("Matrix index is out of range.");
            }

            switch (row * 3 + column)
            {
                case 0:
                    m11 = value;
                    break;
                case 1:
                    m12 = value;
                    break;
                case 2:
                    m13 = value;
                    break;
                case 3:
                    m21 = value;
                    break;
                case 4:
                    m22 = value;
                    break;
                case 5:
                    m23 = value;
                    break;
                case 6:
                    m31 = value;
                    break;
                case 7:
                    m32 = value;
                    break;
                case 8:
                    m33 = value;
                    break;
            }
        }
    }

    public float this[int index]
    {
        readonly get => this[index % 3, index / 3];
        set => this[index % 3, index / 3] = value;
    }

    public readonly float determinant => GetDeterminant(this);
    public readonly Matrix3x3 inverse => Inverse(this);
    public readonly Matrix3x3 transposed => Transpose(this);

    public readonly Vector3 GetRow(int index)
    {
        return new Vector3(this[index, 0], this[index, 1], this[index, 2]);
    }

    public readonly Vector3 GetColumn(int index)
    {
        return new Vector3(this[0, index], this[1, index], this[2, index]);
    }

    public void SetRow(int index, Vector3 row)
    {
        this[index, 0] = row.x;
        this[index, 1] = row.y;
        this[index, 2] = row.z;
    }

    public void SetColumn(int index, Vector3 column)
    {
        this[0, index] = column.x;
        this[1, index] = column.y;
        this[2, index] = column.z;
    }

    public static Matrix3x3 zero => new Matrix3x3(
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f);

    public static Matrix3x3 one => new Matrix3x3(
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f);

    public static Matrix3x3 identity => new Matrix3x3(
        1f,
        0f,
        0f,
        0f,
        1f,
        0f,
        0f,
        0f,
        1f);

    public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.m11 + b.m11,
            a.m12 + b.m12,
            a.m13 + b.m13,
            a.m21 + b.m21,
            a.m22 + b.m22,
            a.m23 + b.m23,
            a.m31 + b.m31,
            a.m32 + b.m32,
            a.m33 + b.m33);
    }

    public static Matrix3x3 operator -(Matrix3x3 v)
    {
        return new Matrix3x3(
            -v.m11,
            -v.m12,
            -v.m13,
            -v.m21,
            -v.m22,
            -v.m23,
            -v.m31,
            -v.m32,
            -v.m33);
    }

    public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.m11 - b.m11,
            a.m12 - b.m12,
            a.m13 - b.m13,
            a.m21 - b.m21,
            a.m22 - b.m22,
            a.m23 - b.m23,
            a.m31 - b.m31,
            a.m32 - b.m32,
            a.m33 - b.m33);
    }

    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
            a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
            a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,
            a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
            a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
            a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,
            a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
            a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
            a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33);
    }

    public static Vector3 operator *(Matrix3x3 a, Vector3 b)
    {
        return new Vector3(
            a.m11 * b.x + a.m12 * b.y + a.m13 * b.z,
            a.m21 * b.x + a.m22 * b.y + a.m23 * b.z,
            a.m31 * b.x + a.m32 * b.y + a.m33 * b.z);
    }

    public static Vector2 operator *(Matrix3x3 a, Vector2 b)
    {
        return Vector2.TransformPoint(b, a);
    }

    public static Matrix3x3 operator *(Matrix3x3 a, float b)
    {
        return new Matrix3x3(
            a.m11 * b,
            a.m12 * b,
            a.m13 * b,
            a.m21 * b,
            a.m22 * b,
            a.m23 * b,
            a.m31 * b,
            a.m32 * b,
            a.m33 * b);
    }

    public static Matrix3x3 operator *(float a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a * b.m11,
            a * b.m12,
            a * b.m13,
            a * b.m21,
            a * b.m22,
            a * b.m23,
            a * b.m31,
            a * b.m32,
            a * b.m33);
    }

    public static Matrix3x3 operator /(Matrix3x3 a, float b)
    {
        return new Matrix3x3(
            a.m11 / b,
            a.m12 / b,
            a.m13 / b,
            a.m21 / b,
            a.m22 / b,
            a.m23 / b,
            a.m31 / b,
            a.m32 / b,
            a.m33 / b);
    }

    public static bool operator ==(Matrix3x3 a, Matrix3x3 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Matrix3x3 a, Matrix3x3 b)
    {
        return !a.Equals(b);
    }

    public readonly bool Equals(Matrix3x3 other)
    {
        return m11.Equals(other.m11) && m12.Equals(other.m12) && m13.Equals(other.m13) &&
               m21.Equals(other.m21) && m22.Equals(other.m22) && m23.Equals(other.m23) &&
               m31.Equals(other.m31) && m32.Equals(other.m32) && m33.Equals(other.m33);
    }

    public static bool Approximately(Matrix3x3 a, Matrix3x3 b)
    {
        return MathF.Approximately(a.m11, b.m11) && MathF.Approximately(a.m12, b.m12) && MathF.Approximately(a.m13, b.m13) &&
               MathF.Approximately(a.m21, b.m21) && MathF.Approximately(a.m22, b.m22) && MathF.Approximately(a.m23, b.m23) &&
               MathF.Approximately(a.m31, b.m31) && MathF.Approximately(a.m32, b.m32) && MathF.Approximately(a.m33, b.m33);
    }

    public static Matrix3x3 CreateRotation(float angle)
    {
        (float sin, float cos) = MathF.SinCos(angle);
        return new Matrix3x3(
            cos,
            -sin,
            0f,
            sin,
            cos,
            0f,
            0f,
            0f,
            1f);
    }

    public static Matrix3x3 CreateScale(Vector2 scale)
    {
        return CreateScale(new Vector3(scale.x, scale.y, 1f));
    }

    public static Matrix3x3 CreateScale(Vector3 scale)
    {
        return new Matrix3x3(
            scale.x,
            0f,
            0f,
            0f,
            scale.y,
            0f,
            0f,
            0f,
            scale.z);
    }

    public static Matrix3x3 CreateTranslation(Vector2 translation)
    {
        return new Matrix3x3(
            1f,
            0f,
            translation.x,
            0f,
            1f,
            translation.y,
            0f,
            0f,
            1f);
    }

    public static Matrix3x3 CreateTRS(Vector2 translation, float rotation, Vector2 scale)
    {
        (float sin, float cos) = MathF.SinCos(rotation);
        return new Matrix3x3(
            cos * scale.x,
            -sin * scale.y,
            translation.x,
            sin * scale.x,
            cos * scale.y,
            translation.y,
            0f,
            0f,
            1f);
    }

    public static Matrix3x3 FromQuaternion(Quaternion rotation)
    {
        rotation = rotation.normalized;
        float xx = rotation.x * rotation.x;
        float yy = rotation.y * rotation.y;
        float zz = rotation.z * rotation.z;
        float xy = rotation.x * rotation.y;
        float xz = rotation.x * rotation.z;
        float yz = rotation.y * rotation.z;
        float wx = rotation.w * rotation.x;
        float wy = rotation.w * rotation.y;
        float wz = rotation.w * rotation.z;

        return new Matrix3x3(
            1f - 2f * (yy + zz),
            2f * (xy - wz),
            2f * (xz + wy),
            2f * (xy + wz),
            1f - 2f * (xx + zz),
            2f * (yz - wx),
            2f * (xz - wy),
            2f * (yz + wx),
            1f - 2f * (xx + yy));
    }

    public static float GetDeterminant(Matrix3x3 v)
    {
        return (float)(v.m11 * ((double)v.m22 * v.m33 - (double)v.m23 * v.m32)
                       - v.m12 * ((double)v.m21 * v.m33 - (double)v.m23 * v.m31)
                       + v.m13 * ((double)v.m21 * v.m32 - (double)v.m22 * v.m31));
    }

    public static Matrix3x3 Inverse(Matrix3x3 v)
    {
        if (!TryInverse(v, out Matrix3x3 inverse))
        {
            throw new InvalidOperationException("Matrix is not invertible.");
        }

        return inverse;
    }

    public static Matrix3x3 Lerp(Matrix3x3 a, Matrix3x3 b, float t)
    {
        return a + (b - a) * MathF.Clamp01(t);
    }

    public static Matrix3x3 LerpUnclamped(Matrix3x3 a, Matrix3x3 b, float t)
    {
        return a + (b - a) * t;
    }

    public static Matrix3x3 Transpose(Matrix3x3 v)
    {
        return new Matrix3x3(
            v.m11,
            v.m21,
            v.m31,
            v.m12,
            v.m22,
            v.m32,
            v.m13,
            v.m23,
            v.m33);
    }

    public static bool TryInverse(Matrix3x3 v, out Matrix3x3 inverse)
    {
        inverse = zero;
        double c11 = (double)v.m22 * v.m33 - (double)v.m23 * v.m32;
        double c12 = (double)v.m23 * v.m31 - (double)v.m21 * v.m33;
        double c13 = (double)v.m21 * v.m32 - (double)v.m22 * v.m31;
        double c21 = (double)v.m13 * v.m32 - (double)v.m12 * v.m33;
        double c22 = (double)v.m11 * v.m33 - (double)v.m13 * v.m31;
        double c23 = (double)v.m12 * v.m31 - (double)v.m11 * v.m32;
        double c31 = (double)v.m12 * v.m23 - (double)v.m13 * v.m22;
        double c32 = (double)v.m13 * v.m21 - (double)v.m11 * v.m23;
        double c33 = (double)v.m11 * v.m22 - (double)v.m12 * v.m21;
        double determinant = v.m11 * c11 + v.m12 * c12 + v.m13 * c13;
        if (determinant == 0d || !double.IsFinite(determinant))
        {
            return false;
        }

        Matrix3x3 result = new Matrix3x3(
            (float)(c11 / determinant),
            (float)(c21 / determinant),
            (float)(c31 / determinant),
            (float)(c12 / determinant),
            (float)(c22 / determinant),
            (float)(c32 / determinant),
            (float)(c13 / determinant),
            (float)(c23 / determinant),
            (float)(c33 / determinant));
        if (!float.IsFinite(result.m11) || !float.IsFinite(result.m12) || !float.IsFinite(result.m13) ||
            !float.IsFinite(result.m21) || !float.IsFinite(result.m22) || !float.IsFinite(result.m23) ||
            !float.IsFinite(result.m31) || !float.IsFinite(result.m32) || !float.IsFinite(result.m33))
        {
            return false;
        }

        inverse = result;
        return true;
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Matrix3x3 other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(
            HashCode.Combine(m11, m12, m13),
            HashCode.Combine(m21, m22, m23),
            HashCode.Combine(m31, m32, m33));
    }

    public readonly override string ToString()
    {
        return $"Matrix3x3({m11}, {m12}, {m13}, {m21}, {m22}, {m23}, {m31}, {m32}, {m33})";
    }
}
