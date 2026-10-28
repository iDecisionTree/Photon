using System.Diagnostics.CodeAnalysis;

namespace Photon.Core.Mathematics;

public struct Matrix4x4 : IEquatable<Matrix4x4>
{
    public float m11;
    public float m12;
    public float m13;
    public float m14;
    public float m21;
    public float m22;
    public float m23;
    public float m24;
    public float m31;
    public float m32;
    public float m33;
    public float m34;
    public float m41;
    public float m42;
    public float m43;
    public float m44;

    public Matrix4x4(
        float m11,
        float m12,
        float m13,
        float m14,
        float m21,
        float m22,
        float m23,
        float m24,
        float m31,
        float m32,
        float m33,
        float m34,
        float m41,
        float m42,
        float m43,
        float m44)
    {
        this.m11 = m11;
        this.m12 = m12;
        this.m13 = m13;
        this.m14 = m14;
        this.m21 = m21;
        this.m22 = m22;
        this.m23 = m23;
        this.m24 = m24;
        this.m31 = m31;
        this.m32 = m32;
        this.m33 = m33;
        this.m34 = m34;
        this.m41 = m41;
        this.m42 = m42;
        this.m43 = m43;
        this.m44 = m44;
    }

    public Matrix4x4(float value) : this(
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value,
        value)
    {
    }

    public float this[int row, int column]
    {
        readonly get
        {
            if ((uint)row >= 4 || (uint)column >= 4)
            {
                throw new IndexOutOfRangeException("Matrix index is out of range.");
            }

            return (row * 4 + column) switch
            {
                0 => m11,
                1 => m12,
                2 => m13,
                3 => m14,
                4 => m21,
                5 => m22,
                6 => m23,
                7 => m24,
                8 => m31,
                9 => m32,
                10 => m33,
                11 => m34,
                12 => m41,
                13 => m42,
                14 => m43,
                15 => m44,
                _ => throw new IndexOutOfRangeException("Matrix index is out of range.")
            };
        }
        set
        {
            if ((uint)row >= 4 || (uint)column >= 4)
            {
                throw new IndexOutOfRangeException("Matrix index is out of range.");
            }

            switch (row * 4 + column)
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
                    m14 = value;
                    break;
                case 4:
                    m21 = value;
                    break;
                case 5:
                    m22 = value;
                    break;
                case 6:
                    m23 = value;
                    break;
                case 7:
                    m24 = value;
                    break;
                case 8:
                    m31 = value;
                    break;
                case 9:
                    m32 = value;
                    break;
                case 10:
                    m33 = value;
                    break;
                case 11:
                    m34 = value;
                    break;
                case 12:
                    m41 = value;
                    break;
                case 13:
                    m42 = value;
                    break;
                case 14:
                    m43 = value;
                    break;
                case 15:
                    m44 = value;
                    break;
            }
        }
    }

    public float this[int index]
    {
        readonly get => this[index % 4, index / 4];
        set => this[index % 4, index / 4] = value;
    }

    public readonly float determinant => GetDeterminant(this);
    public readonly Matrix4x4 inverse => Inverse(this);
    public readonly Matrix4x4 transposed => Transpose(this);

    public readonly Vector4 GetRow(int index)
    {
        return new Vector4(this[index, 0], this[index, 1], this[index, 2], this[index, 3]);
    }

    public readonly Vector4 GetColumn(int index)
    {
        return new Vector4(this[0, index], this[1, index], this[2, index], this[3, index]);
    }

    public void SetRow(int index, Vector4 row)
    {
        this[index, 0] = row.x;
        this[index, 1] = row.y;
        this[index, 2] = row.z;
        this[index, 3] = row.w;
    }

    public void SetColumn(int index, Vector4 column)
    {
        this[0, index] = column.x;
        this[1, index] = column.y;
        this[2, index] = column.z;
        this[3, index] = column.w;
    }

    public static Matrix4x4 zero => new Matrix4x4(
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f,
        0f);

    public static Matrix4x4 one => new Matrix4x4(
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f,
        1f);

    public static Matrix4x4 identity => new Matrix4x4(
        1f,
        0f,
        0f,
        0f,
        0f,
        1f,
        0f,
        0f,
        0f,
        0f,
        1f,
        0f,
        0f,
        0f,
        0f,
        1f);

    public static Matrix4x4 operator +(Matrix4x4 a, Matrix4x4 b)
    {
        return new Matrix4x4(
            a.m11 + b.m11,
            a.m12 + b.m12,
            a.m13 + b.m13,
            a.m14 + b.m14,
            a.m21 + b.m21,
            a.m22 + b.m22,
            a.m23 + b.m23,
            a.m24 + b.m24,
            a.m31 + b.m31,
            a.m32 + b.m32,
            a.m33 + b.m33,
            a.m34 + b.m34,
            a.m41 + b.m41,
            a.m42 + b.m42,
            a.m43 + b.m43,
            a.m44 + b.m44);
    }

    public static Matrix4x4 operator -(Matrix4x4 v)
    {
        return new Matrix4x4(
            -v.m11,
            -v.m12,
            -v.m13,
            -v.m14,
            -v.m21,
            -v.m22,
            -v.m23,
            -v.m24,
            -v.m31,
            -v.m32,
            -v.m33,
            -v.m34,
            -v.m41,
            -v.m42,
            -v.m43,
            -v.m44);
    }

    public static Matrix4x4 operator -(Matrix4x4 a, Matrix4x4 b)
    {
        return new Matrix4x4(
            a.m11 - b.m11,
            a.m12 - b.m12,
            a.m13 - b.m13,
            a.m14 - b.m14,
            a.m21 - b.m21,
            a.m22 - b.m22,
            a.m23 - b.m23,
            a.m24 - b.m24,
            a.m31 - b.m31,
            a.m32 - b.m32,
            a.m33 - b.m33,
            a.m34 - b.m34,
            a.m41 - b.m41,
            a.m42 - b.m42,
            a.m43 - b.m43,
            a.m44 - b.m44);
    }

    public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
    {
        return new Matrix4x4(
            a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31 + a.m14 * b.m41,
            a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32 + a.m14 * b.m42,
            a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33 + a.m14 * b.m43,
            a.m11 * b.m14 + a.m12 * b.m24 + a.m13 * b.m34 + a.m14 * b.m44,
            a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31 + a.m24 * b.m41,
            a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32 + a.m24 * b.m42,
            a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33 + a.m24 * b.m43,
            a.m21 * b.m14 + a.m22 * b.m24 + a.m23 * b.m34 + a.m24 * b.m44,
            a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31 + a.m34 * b.m41,
            a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32 + a.m34 * b.m42,
            a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33 + a.m34 * b.m43,
            a.m31 * b.m14 + a.m32 * b.m24 + a.m33 * b.m34 + a.m34 * b.m44,
            a.m41 * b.m11 + a.m42 * b.m21 + a.m43 * b.m31 + a.m44 * b.m41,
            a.m41 * b.m12 + a.m42 * b.m22 + a.m43 * b.m32 + a.m44 * b.m42,
            a.m41 * b.m13 + a.m42 * b.m23 + a.m43 * b.m33 + a.m44 * b.m43,
            a.m41 * b.m14 + a.m42 * b.m24 + a.m43 * b.m34 + a.m44 * b.m44);
    }

    public static Vector4 operator *(Matrix4x4 a, Vector4 b)
    {
        return new Vector4(
            a.m11 * b.x + a.m12 * b.y + a.m13 * b.z + a.m14 * b.w,
            a.m21 * b.x + a.m22 * b.y + a.m23 * b.z + a.m24 * b.w,
            a.m31 * b.x + a.m32 * b.y + a.m33 * b.z + a.m34 * b.w,
            a.m41 * b.x + a.m42 * b.y + a.m43 * b.z + a.m44 * b.w);
    }

    public static Vector3 operator *(Matrix4x4 a, Vector3 b)
    {
        return Vector3.TransformPoint(b, a);
    }

    public static Matrix4x4 operator *(Matrix4x4 a, float b)
    {
        return new Matrix4x4(
            a.m11 * b,
            a.m12 * b,
            a.m13 * b,
            a.m14 * b,
            a.m21 * b,
            a.m22 * b,
            a.m23 * b,
            a.m24 * b,
            a.m31 * b,
            a.m32 * b,
            a.m33 * b,
            a.m34 * b,
            a.m41 * b,
            a.m42 * b,
            a.m43 * b,
            a.m44 * b);
    }

    public static Matrix4x4 operator *(float a, Matrix4x4 b)
    {
        return new Matrix4x4(
            a * b.m11,
            a * b.m12,
            a * b.m13,
            a * b.m14,
            a * b.m21,
            a * b.m22,
            a * b.m23,
            a * b.m24,
            a * b.m31,
            a * b.m32,
            a * b.m33,
            a * b.m34,
            a * b.m41,
            a * b.m42,
            a * b.m43,
            a * b.m44);
    }

    public static Matrix4x4 operator /(Matrix4x4 a, float b)
    {
        return new Matrix4x4(
            a.m11 / b,
            a.m12 / b,
            a.m13 / b,
            a.m14 / b,
            a.m21 / b,
            a.m22 / b,
            a.m23 / b,
            a.m24 / b,
            a.m31 / b,
            a.m32 / b,
            a.m33 / b,
            a.m34 / b,
            a.m41 / b,
            a.m42 / b,
            a.m43 / b,
            a.m44 / b);
    }

    public static bool operator ==(Matrix4x4 a, Matrix4x4 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Matrix4x4 a, Matrix4x4 b)
    {
        return !a.Equals(b);
    }

    public readonly bool Equals(Matrix4x4 other)
    {
        return m11.Equals(other.m11) && m12.Equals(other.m12) && m13.Equals(other.m13) && m14.Equals(other.m14) &&
               m21.Equals(other.m21) && m22.Equals(other.m22) && m23.Equals(other.m23) && m24.Equals(other.m24) &&
               m31.Equals(other.m31) && m32.Equals(other.m32) && m33.Equals(other.m33) && m34.Equals(other.m34) &&
               m41.Equals(other.m41) && m42.Equals(other.m42) && m43.Equals(other.m43) && m44.Equals(other.m44);
    }

    public static bool Approximately(Matrix4x4 a, Matrix4x4 b)
    {
        return MathF.Approximately(a.m11, b.m11) && MathF.Approximately(a.m12, b.m12) &&
               MathF.Approximately(a.m13, b.m13) && MathF.Approximately(a.m14, b.m14) &&
               MathF.Approximately(a.m21, b.m21) && MathF.Approximately(a.m22, b.m22) &&
               MathF.Approximately(a.m23, b.m23) && MathF.Approximately(a.m24, b.m24) &&
               MathF.Approximately(a.m31, b.m31) && MathF.Approximately(a.m32, b.m32) &&
               MathF.Approximately(a.m33, b.m33) && MathF.Approximately(a.m34, b.m34) &&
               MathF.Approximately(a.m41, b.m41) && MathF.Approximately(a.m42, b.m42) &&
               MathF.Approximately(a.m43, b.m43) && MathF.Approximately(a.m44, b.m44);
    }

    public static Matrix4x4 CreateLookAtLeftHanded(Vector3 position, Vector3 target, Vector3 up)
    {
        if (!float.IsFinite(position.x) || !float.IsFinite(position.y) || !float.IsFinite(position.z))
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        if (!float.IsFinite(target.x) || !float.IsFinite(target.y) || !float.IsFinite(target.z))
        {
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        if (!float.IsFinite(up.x) || !float.IsFinite(up.y) || !float.IsFinite(up.z))
        {
            throw new ArgumentOutOfRangeException(nameof(up));
        }

        Vector3 direction = target - position;
        if (!float.IsFinite(direction.x) || !float.IsFinite(direction.y) || !float.IsFinite(direction.z))
        {
            double x = (double)target.x - position.x;
            double y = (double)target.y - position.y;
            double z = (double)target.z - position.z;
            double maxComponent = Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z));
            direction = new Vector3((float)(x / maxComponent), (float)(y / maxComponent), (float)(z / maxComponent));
        }

        Vector3 forward = direction.normalized;
        if (forward == Vector3.zero || !float.IsFinite(forward.lengthSquared))
        {
            throw new ArgumentException("Target must define a nonzero finite direction from position.", nameof(target));
        }

        Vector3 right = Vector3.Cross(up.normalized, forward).normalized;
        if (right == Vector3.zero || !float.IsFinite(right.lengthSquared))
        {
            throw new ArgumentException(
                "Up must define a nonzero finite direction that is not parallel to the viewing direction.",
                nameof(up));
        }

        Vector3 cameraUp = Vector3.Cross(forward, right);
        float xOffset = (float)(-(double)right.x * position.x - (double)right.y * position.y - (double)right.z * position.z);
        float yOffset = (float)(-(double)cameraUp.x * position.x - (double)cameraUp.y * position.y - (double)cameraUp.z * position.z);
        float zOffset = (float)(-(double)forward.x * position.x - (double)forward.y * position.y - (double)forward.z * position.z);
        if (!float.IsFinite(xOffset) || !float.IsFinite(yOffset) || !float.IsFinite(zOffset))
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        return new Matrix4x4(
            right.x,
            right.y,
            right.z,
            xOffset,
            cameraUp.x,
            cameraUp.y,
            cameraUp.z,
            yOffset,
            forward.x,
            forward.y,
            forward.z,
            zOffset,
            0f,
            0f,
            0f,
            1f);
    }

    public static Matrix4x4 CreateOrthographicLeftHanded(float width, float height, float nearPlane, float farPlane)
    {
        if (!float.IsFinite(width) || width <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (!float.IsFinite(height) || height <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        if (!float.IsFinite(nearPlane))
        {
            throw new ArgumentOutOfRangeException(nameof(nearPlane));
        }

        if (!float.IsFinite(farPlane) || farPlane <= nearPlane)
        {
            throw new ArgumentOutOfRangeException(nameof(farPlane));
        }

        double depthRange = (double)farPlane - nearPlane;
        float xScale = 2f / width;
        float yScale = 2f / height;
        float depthScale = (float)(1d / depthRange);
        float depthOffset = (float)(-nearPlane / depthRange);
        if (!float.IsFinite(xScale))
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (!float.IsFinite(yScale))
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        if (!float.IsFinite(depthScale) || !float.IsFinite(depthOffset))
        {
            throw new ArgumentOutOfRangeException(nameof(farPlane));
        }

        return new Matrix4x4(
            xScale,
            0f,
            0f,
            0f,
            0f,
            yScale,
            0f,
            0f,
            0f,
            0f,
            depthScale,
            depthOffset,
            0f,
            0f,
            0f,
            1f);
    }

    public static Matrix4x4 CreatePerspectiveFieldOfViewLeftHanded(float fieldOfView, float aspectRatio, float nearPlane, float farPlane)
    {
        if (!float.IsFinite(fieldOfView) || fieldOfView <= 0f || fieldOfView >= MathF.PI)
        {
            throw new ArgumentOutOfRangeException(nameof(fieldOfView));
        }

        if (!float.IsFinite(aspectRatio) || aspectRatio <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(aspectRatio));
        }

        if (!float.IsFinite(nearPlane) || nearPlane <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(nearPlane));
        }

        if (float.IsNaN(farPlane) || farPlane <= nearPlane)
        {
            throw new ArgumentOutOfRangeException(nameof(farPlane));
        }

        float yScale = 1f / MathF.Tan(fieldOfView * 0.5f);
        float xScale = yScale / aspectRatio;
        double depthRange = (double)farPlane - nearPlane;
        float depthScale = float.IsPositiveInfinity(farPlane) ? 1f : (float)(farPlane / depthRange);
        float depthOffset = (float)(-nearPlane * (double)depthScale);
        if (!float.IsFinite(yScale))
        {
            throw new ArgumentOutOfRangeException(nameof(fieldOfView));
        }

        if (!float.IsFinite(xScale))
        {
            throw new ArgumentOutOfRangeException(nameof(aspectRatio));
        }

        if (!float.IsFinite(depthScale) || !float.IsFinite(depthOffset))
        {
            throw new ArgumentOutOfRangeException(nameof(farPlane));
        }

        return new Matrix4x4(
            xScale,
            0f,
            0f,
            0f,
            0f,
            yScale,
            0f,
            0f,
            0f,
            0f,
            depthScale,
            depthOffset,
            0f,
            0f,
            1f,
            0f);
    }

    public static Matrix4x4 CreateScale(Vector3 scale)
    {
        return new Matrix4x4(
            scale.x,
            0f,
            0f,
            0f,
            0f,
            scale.y,
            0f,
            0f,
            0f,
            0f,
            scale.z,
            0f,
            0f,
            0f,
            0f,
            1f);
    }

    public static Matrix4x4 CreateTranslation(Vector3 translation)
    {
        return new Matrix4x4(
            1f,
            0f,
            0f,
            translation.x,
            0f,
            1f,
            0f,
            translation.y,
            0f,
            0f,
            1f,
            translation.z,
            0f,
            0f,
            0f,
            1f);
    }

    public static Matrix4x4 CreateTRS(Vector3 translation, Quaternion rotation, Vector3 scale)
    {
        Matrix3x3 matrix = Matrix3x3.FromQuaternion(rotation);
        return new Matrix4x4(
            matrix.m11 * scale.x,
            matrix.m12 * scale.y,
            matrix.m13 * scale.z,
            translation.x,
            matrix.m21 * scale.x,
            matrix.m22 * scale.y,
            matrix.m23 * scale.z,
            translation.y,
            matrix.m31 * scale.x,
            matrix.m32 * scale.y,
            matrix.m33 * scale.z,
            translation.z,
            0f,
            0f,
            0f,
            1f);
    }

    public static Matrix4x4 FromQuaternion(Quaternion rotation)
    {
        Matrix3x3 matrix = Matrix3x3.FromQuaternion(rotation);
        return new Matrix4x4(
            matrix.m11,
            matrix.m12,
            matrix.m13,
            0f,
            matrix.m21,
            matrix.m22,
            matrix.m23,
            0f,
            matrix.m31,
            matrix.m32,
            matrix.m33,
            0f,
            0f,
            0f,
            0f,
            1f);
    }

    public static float GetDeterminant(Matrix4x4 v)
    {
        double c1 = v.m22 * ((double)v.m33 * v.m44 - (double)v.m34 * v.m43)
                    - v.m23 * ((double)v.m32 * v.m44 - (double)v.m34 * v.m42)
                    + v.m24 * ((double)v.m32 * v.m43 - (double)v.m33 * v.m42);
        double c2 = v.m21 * ((double)v.m33 * v.m44 - (double)v.m34 * v.m43)
                    - v.m23 * ((double)v.m31 * v.m44 - (double)v.m34 * v.m41)
                    + v.m24 * ((double)v.m31 * v.m43 - (double)v.m33 * v.m41);
        double c3 = v.m21 * ((double)v.m32 * v.m44 - (double)v.m34 * v.m42)
                    - v.m22 * ((double)v.m31 * v.m44 - (double)v.m34 * v.m41)
                    + v.m24 * ((double)v.m31 * v.m42 - (double)v.m32 * v.m41);
        double c4 = v.m21 * ((double)v.m32 * v.m43 - (double)v.m33 * v.m42)
                    - v.m22 * ((double)v.m31 * v.m43 - (double)v.m33 * v.m41)
                    + v.m23 * ((double)v.m31 * v.m42 - (double)v.m32 * v.m41);

        return (float)(v.m11 * c1 - v.m12 * c2 + v.m13 * c3 - v.m14 * c4);
    }

    public static Matrix4x4 Inverse(Matrix4x4 v)
    {
        if (!TryInverse(v, out Matrix4x4 inverse))
        {
            throw new InvalidOperationException("Matrix is not invertible.");
        }

        return inverse;
    }

    public static Matrix4x4 Lerp(Matrix4x4 a, Matrix4x4 b, float t)
    {
        return a + (b - a) * MathF.Clamp01(t);
    }

    public static Matrix4x4 LerpUnclamped(Matrix4x4 a, Matrix4x4 b, float t)
    {
        return a + (b - a) * t;
    }

    public static Matrix4x4 Transpose(Matrix4x4 v)
    {
        return new Matrix4x4(
            v.m11,
            v.m21,
            v.m31,
            v.m41,
            v.m12,
            v.m22,
            v.m32,
            v.m42,
            v.m13,
            v.m23,
            v.m33,
            v.m43,
            v.m14,
            v.m24,
            v.m34,
            v.m44);
    }

    public static bool TryInverse(Matrix4x4 v, out Matrix4x4 inverse)
    {
        inverse = zero;
        double s0 = (double)v.m11 * v.m22 - (double)v.m21 * v.m12;
        double s1 = (double)v.m11 * v.m23 - (double)v.m21 * v.m13;
        double s2 = (double)v.m11 * v.m24 - (double)v.m21 * v.m14;
        double s3 = (double)v.m12 * v.m23 - (double)v.m22 * v.m13;
        double s4 = (double)v.m12 * v.m24 - (double)v.m22 * v.m14;
        double s5 = (double)v.m13 * v.m24 - (double)v.m23 * v.m14;
        double c0 = (double)v.m31 * v.m42 - (double)v.m41 * v.m32;
        double c1 = (double)v.m31 * v.m43 - (double)v.m41 * v.m33;
        double c2 = (double)v.m31 * v.m44 - (double)v.m41 * v.m34;
        double c3 = (double)v.m32 * v.m43 - (double)v.m42 * v.m33;
        double c4 = (double)v.m32 * v.m44 - (double)v.m42 * v.m34;
        double c5 = (double)v.m33 * v.m44 - (double)v.m43 * v.m34;
        double determinant = s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0;
        if (determinant == 0d || !double.IsFinite(determinant))
        {
            return false;
        }

        Matrix4x4 result = new Matrix4x4(
            (float)((v.m22 * c5 - v.m23 * c4 + v.m24 * c3) / determinant),
            (float)((-v.m12 * c5 + v.m13 * c4 - v.m14 * c3) / determinant),
            (float)((v.m42 * s5 - v.m43 * s4 + v.m44 * s3) / determinant),
            (float)((-v.m32 * s5 + v.m33 * s4 - v.m34 * s3) / determinant),
            (float)((-v.m21 * c5 + v.m23 * c2 - v.m24 * c1) / determinant),
            (float)((v.m11 * c5 - v.m13 * c2 + v.m14 * c1) / determinant),
            (float)((-v.m41 * s5 + v.m43 * s2 - v.m44 * s1) / determinant),
            (float)((v.m31 * s5 - v.m33 * s2 + v.m34 * s1) / determinant),
            (float)((v.m21 * c4 - v.m22 * c2 + v.m24 * c0) / determinant),
            (float)((-v.m11 * c4 + v.m12 * c2 - v.m14 * c0) / determinant),
            (float)((v.m41 * s4 - v.m42 * s2 + v.m44 * s0) / determinant),
            (float)((-v.m31 * s4 + v.m32 * s2 - v.m34 * s0) / determinant),
            (float)((-v.m21 * c3 + v.m22 * c1 - v.m23 * c0) / determinant),
            (float)((v.m11 * c3 - v.m12 * c1 + v.m13 * c0) / determinant),
            (float)((-v.m41 * s3 + v.m42 * s1 - v.m43 * s0) / determinant),
            (float)((v.m31 * s3 - v.m32 * s1 + v.m33 * s0) / determinant));
        if (!float.IsFinite(result.m11) || !float.IsFinite(result.m12) || !float.IsFinite(result.m13) || !float.IsFinite(result.m14) ||
            !float.IsFinite(result.m21) || !float.IsFinite(result.m22) || !float.IsFinite(result.m23) || !float.IsFinite(result.m24) ||
            !float.IsFinite(result.m31) || !float.IsFinite(result.m32) || !float.IsFinite(result.m33) || !float.IsFinite(result.m34) ||
            !float.IsFinite(result.m41) || !float.IsFinite(result.m42) || !float.IsFinite(result.m43) || !float.IsFinite(result.m44))
        {
            return false;
        }

        inverse = result;
        return true;
    }

    public readonly override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Matrix4x4 other && Equals(other);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(
            HashCode.Combine(m11, m12, m13, m14),
            HashCode.Combine(m21, m22, m23, m24),
            HashCode.Combine(m31, m32, m33, m34),
            HashCode.Combine(m41, m42, m43, m44));
    }

    public readonly override string ToString()
    {
        return $"Matrix4x4({m11}, {m12}, {m13}, {m14}, {m21}, {m22}, {m23}, {m24}, " +
               $"{m31}, {m32}, {m33}, {m34}, {m41}, {m42}, {m43}, {m44})";
    }
}
