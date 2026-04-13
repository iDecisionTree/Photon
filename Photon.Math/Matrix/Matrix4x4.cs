using Photon.Math.Vector;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Photon.Math.Matrix
{
    /// <summary>
    /// 行主序, 列向量, 左手系
    /// </summary>
    public readonly struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        public readonly float m00, m01, m02, m03;
        public readonly float m10, m11, m12, m13;
        public readonly float m20, m21, m22, m23;
        public readonly float m30, m31, m32, m33;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;

            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;

            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;

            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public float determinant => Determinant(this);
        public Matrix4x4 inverted => Invert(this);
        public Matrix4x4 transposed => Transpose(this);

        public static readonly Matrix4x4 identity = new Matrix4x4(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
        public static readonly Matrix4x4 zero = new Matrix4x4(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator +(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02, a.m03 + b.m03,
                a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12, a.m13 + b.m13,
                a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22, a.m23 + b.m23,
                a.m30 + b.m30, a.m31 + b.m31, a.m32 + b.m32, a.m33 + b.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator +(Matrix4x4 a, float b)
        {
            return new Matrix4x4(
                a.m00 + b, a.m01 + b, a.m02 + b, a.m03 + b,
                a.m10 + b, a.m11 + b, a.m12 + b, a.m13 + b,
                a.m20 + b, a.m21 + b, a.m22 + b, a.m23 + b,
                a.m30 + b, a.m31 + b, a.m32 + b, a.m33 + b
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator +(float a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a + b.m00, a + b.m01, a + b.m02, a + b.m03,
                a + b.m10, a + b.m11, a + b.m12, a + b.m13,
                a + b.m20, a + b.m21, a + b.m22, a + b.m23,
                a + b.m30, a + b.m31, a + b.m32, a + b.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator -(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02, a.m03 - b.m03,
                a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12, a.m13 - b.m13,
                a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22, a.m23 - b.m23,
                a.m30 - b.m30, a.m31 - b.m31, a.m32 - b.m32, a.m33 - b.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator -(Matrix4x4 a, float b)
        {
            return new Matrix4x4(
                a.m00 - b, a.m01 - b, a.m02 - b, a.m03 - b,
                a.m10 - b, a.m11 - b, a.m12 - b, a.m13 - b,
                a.m20 - b, a.m21 - b, a.m22 - b, a.m23 - b,
                a.m30 - b, a.m31 - b, a.m32 - b, a.m33 - b
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator -(float a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a - b.m00, a - b.m01, a - b.m02, a - b.m03,
                a - b.m10, a - b.m11, a - b.m12, a - b.m13,
                a - b.m20, a - b.m21, a - b.m22, a - b.m23,
                a - b.m30, a - b.m31, a - b.m32, a - b.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator -(Matrix4x4 a)
        {
            return new Matrix4x4(
                -a.m00, -a.m01, -a.m02, -a.m03,
                -a.m10, -a.m11, -a.m12, -a.m13,
                -a.m20, -a.m21, -a.m22, -a.m23,
                -a.m30, -a.m31, -a.m32, -a.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20 + a.m03 * b.m30,
                a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21 + a.m03 * b.m31,
                a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22 + a.m03 * b.m32,
                a.m00 * b.m03 + a.m01 * b.m13 + a.m02 * b.m23 + a.m03 * b.m33,

                a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20 + a.m13 * b.m30,
                a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
                a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
                a.m10 * b.m03 + a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,

                a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20 + a.m23 * b.m30,
                a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
                a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
                a.m20 * b.m03 + a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,

                a.m30 * b.m00 + a.m31 * b.m10 + a.m32 * b.m20 + a.m33 * b.m30,
                a.m30 * b.m01 + a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
                a.m30 * b.m02 + a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
                a.m30 * b.m03 + a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 operator *(Matrix4x4 m, Vector4 v)
        {
            return Transform(m, v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Matrix4x4 m, Vector3 v)
        {
            return TransformPoint(m, v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(Matrix4x4 a, float b)
        {
            return new Matrix4x4(
                a.m00 * b, a.m01 * b, a.m02 * b, a.m03 * b,
                a.m10 * b, a.m11 * b, a.m12 * b, a.m13 * b,
                a.m20 * b, a.m21 * b, a.m22 * b, a.m23 * b,
                a.m30 * b, a.m31 * b, a.m32 * b, a.m33 * b
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(float a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a * b.m00, a * b.m01, a * b.m02, a * b.m03,
                a * b.m10, a * b.m11, a * b.m12, a * b.m13,
                a * b.m20, a * b.m21, a * b.m22, a * b.m23,
                a * b.m30, a * b.m31, a * b.m32, a * b.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator /(Matrix4x4 a, float b)
        {
            if (Mathf.Approximately(b, 0f))
            {
                throw new DivideByZeroException($"{a.ToString()}不能除以0");
            }

            float inv = 1f / b;
            return new Matrix4x4(
                a.m00 * inv, a.m01 * inv, a.m02 * inv, a.m03 * inv,
                a.m10 * inv, a.m11 * inv, a.m12 * inv, a.m13 * inv,
                a.m20 * inv, a.m21 * inv, a.m22 * inv, a.m23 * inv,
                a.m30 * inv, a.m31 * inv, a.m32 * inv, a.m33 * inv
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Matrix4x4 a, Matrix4x4 b)
        {
            return Mathf.Approximately(a.m00, b.m00) && Mathf.Approximately(a.m01, b.m01) && Mathf.Approximately(a.m02, b.m02) && Mathf.Approximately(a.m03, b.m03) &&
                   Mathf.Approximately(a.m10, b.m10) && Mathf.Approximately(a.m11, b.m11) && Mathf.Approximately(a.m12, b.m12) && Mathf.Approximately(a.m13, b.m13) &&
                   Mathf.Approximately(a.m20, b.m20) && Mathf.Approximately(a.m21, b.m21) && Mathf.Approximately(a.m22, b.m22) && Mathf.Approximately(a.m23, b.m23) &&
                   Mathf.Approximately(a.m30, b.m30) && Mathf.Approximately(a.m31, b.m31) && Mathf.Approximately(a.m32, b.m32) && Mathf.Approximately(a.m33, b.m33);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Matrix4x4 a, Matrix4x4 b)
        {
            return !(a == b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Matrix4x4 other)
        {
            return this == other;
        }

        /// <summary>
        /// Z-X-Y 内旋, 接受弧度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateFromEulerAngles(float x, float y, float z)
        {
            Matrix4x4 rz = CreateRotationZ(z);
            Matrix4x4 rx = CreateRotationX(x);
            Matrix4x4 ry = CreateRotationY(y);

            return ry * rx * rz;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateFromQuaternion(Quaternion q)
        {
            q = q.normalized;

            float xx = q.x * q.x;
            float yy = q.y * q.y;
            float zz = q.z * q.z;
            float xy = q.x * q.y;
            float xz = q.x * q.z;
            float yz = q.y * q.z;
            float wx = q.w * q.x;
            float wy = q.w * q.y;
            float wz = q.w * q.z;

            return new Matrix4x4(
                1f - 2f * (yy + zz), 2f * (xy - wz), 2f * (xz + wy), 0f,
                2f * (xy + wz), 1f - 2f * (xx + zz), 2f * (yz - wx), 0f,
                2f * (xz - wy), 2f * (yz + wx), 1f - 2f * (xx + yy), 0f,
                0f, 0f, 0f, 1f
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateLookAt(Vector3 position, Vector3 forward, Vector3 right, Vector3 up)
        {
            return new Matrix4x4(
                right.x, right.y, right.z, -Vector3.Dot(right, position),
                up.x, up.y, up.z, -Vector3.Dot(up, position),
                forward.x, forward.y, forward.z, -Vector3.Dot(forward, position),
                0f, 0f, 0f, 1f
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreatePerspectiveFieldOfView(float fov, float aspect, float near, float far)
        {
            float yScale = 1f / Mathf.Tan(fov * 0.5f);
            float xScale = yScale / aspect;
            float zScale = far / (far - near);
            float zOffset = (-near * far) / (far - near);

            return new Matrix4x4(
                xScale, 0f, 0f, 0f,
                0f, yScale, 0f, 0f,
                0f, 0f, zScale, zOffset,
                0f, 0f, 1f, 0f
            );
        }

        /// <summary>
        /// 接受弧度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateRotationX(float x)
        {
            float cos = Mathf.Cos(x);
            float sin = Mathf.Sin(x);

            return new Matrix4x4(
                1f, 0f, 0f, 0f,
                0f, cos, -sin, 0f,
                0f, sin, cos, 0f,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// 接受弧度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateRotationY(float y)
        {
            float cos = Mathf.Cos(y);
            float sin = Mathf.Sin(y);

            return new Matrix4x4(
                cos, 0f, sin, 0f,
                0f, 1f, 0f, 0f,
               -sin, 0f, cos, 0f,
                0f, 0f, 0f, 1f
            );
        }

        /// <summary>
        /// 接受弧度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateRotationZ(float z)
        {
            float cos = Mathf.Cos(z);
            float sin = Mathf.Sin(z);

            return new Matrix4x4(
                cos, -sin, 0f, 0f,
                sin, cos, 0f, 0f,
                0f, 0f, 1f, 0f,
                0f, 0f, 0f, 1f
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateScale(Vector3 scale)
        {
            return new Matrix4x4(
                scale.x, 0f, 0f, 0f,
                0f, scale.y, 0f, 0f,
                0f, 0f, scale.z, 0f,
                0f, 0f, 0f, 1f
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTranslation(Vector3 translation)
        {
            return new Matrix4x4(
                1f, 0f, 0f, translation.x,
                0f, 1f, 0f, translation.y,
                0f, 0f, 1f, translation.z,
                0f, 0f, 0f, 1f
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTRS(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            return CreateTranslation(translation) * CreateFromQuaternion(rotation) * CreateScale(scale);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Determinant(Matrix4x4 m)
        {
            float det0 = Determinant3(
                m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33
            );
            float det1 = Determinant3(
                m.m10, m.m12, m.m13,
                m.m20, m.m22, m.m23,
                m.m30, m.m32, m.m33
            );
            float det2 = Determinant3(
                m.m10, m.m11, m.m13,
                m.m20, m.m21, m.m23,
                m.m30, m.m31, m.m33
            );
            float det3 = Determinant3(
                m.m10, m.m11, m.m12,
                m.m20, m.m21, m.m22,
                m.m30, m.m31, m.m32
            );

            return m.m00 * det0 - m.m01 * det1 + m.m02 * det2 - m.m03 * det3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Transform(Matrix4x4 m, Vector4 v)
        {
            return new Vector4(
                m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03 * v.w,
                m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13 * v.w,
                m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23 * v.w,
                m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33 * v.w
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TransformPoint(Matrix4x4 m, Vector3 v)
        {
            float x = m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03;
            float y = m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13;
            float z = m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23;
            float w = m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33;

            if (!Mathf.Approximately(w, 0f))
            {
                float invW = 1f / w;
                x *= invW;
                y *= invW;
                z *= invW;
            }

            return new Vector3(x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 TransformVector(Matrix4x4 m, Vector3 v)
        {
            return new Vector3(
                m.m00 * v.x + m.m01 * v.y + m.m02 * v.z,
                m.m10 * v.x + m.m11 * v.y + m.m12 * v.z,
                m.m20 * v.x + m.m21 * v.y + m.m22 * v.z
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            return new Matrix4x4(
                m.m00, m.m10, m.m20, m.m30,
                m.m01, m.m11, m.m21, m.m31,
                m.m02, m.m12, m.m22, m.m32,
                m.m03, m.m13, m.m23, m.m33
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Invert(Matrix4x4 m)
        {
            float a00 = m.m00, a01 = m.m01, a02 = m.m02, a03 = m.m03;
            float a10 = m.m10, a11 = m.m11, a12 = m.m12, a13 = m.m13;
            float a20 = m.m20, a21 = m.m21, a22 = m.m22, a23 = m.m23;
            float a30 = m.m30, a31 = m.m31, a32 = m.m32, a33 = m.m33;

            float b00 = a00 * a11 - a01 * a10;
            float b01 = a00 * a12 - a02 * a10;
            float b02 = a00 * a13 - a03 * a10;
            float b03 = a01 * a12 - a02 * a11;
            float b04 = a01 * a13 - a03 * a11;
            float b05 = a02 * a13 - a03 * a12;
            float b06 = a20 * a31 - a21 * a30;
            float b07 = a20 * a32 - a22 * a30;
            float b08 = a20 * a33 - a23 * a30;
            float b09 = a21 * a32 - a22 * a31;
            float b10 = a21 * a33 - a23 * a31;
            float b11 = a22 * a33 - a23 * a32;

            float det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;

            if (Mathf.Approximately(det, 0f))
            {
                throw new InvalidOperationException($"矩阵{m}不可逆");
            }

            float invDet = 1f / det;

            return new Matrix4x4(
                (a11 * b11 - a12 * b10 + a13 * b09) * invDet,
                (-a01 * b11 + a02 * b10 - a03 * b09) * invDet,
                (a31 * b05 - a32 * b04 + a33 * b03) * invDet,
                (-a21 * b05 + a22 * b04 - a23 * b03) * invDet,

                (-a10 * b11 + a12 * b08 - a13 * b07) * invDet,
                (a00 * b11 - a02 * b08 + a03 * b07) * invDet,
                (-a30 * b05 + a32 * b02 - a33 * b01) * invDet,
                (a20 * b05 - a22 * b02 + a23 * b01) * invDet,

                (a10 * b10 - a11 * b08 + a13 * b06) * invDet,
                (-a00 * b10 + a01 * b08 - a03 * b06) * invDet,
                (a30 * b04 - a31 * b02 + a33 * b00) * invDet,
                (-a20 * b04 + a21 * b02 - a23 * b00) * invDet,

                (-a10 * b09 + a11 * b07 - a12 * b06) * invDet,
                (a00 * b09 - a01 * b07 + a02 * b06) * invDet,
                (-a30 * b03 + a31 * b01 - a32 * b00) * invDet,
                (a20 * b03 - a21 * b01 + a22 * b00) * invDet
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Determinant3(float a00, float a01, float a02, float a10, float a11, float a12, float a20, float a21, float a22)
        {
            return a00 * (a11 * a22 - a12 * a21) - a01 * (a10 * a22 - a12 * a20) + a02 * (a10 * a21 - a11 * a20);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Matrix4x4 m && this == m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(m00); hash.Add(m01); hash.Add(m02); hash.Add(m03);
            hash.Add(m10); hash.Add(m11); hash.Add(m12); hash.Add(m13);
            hash.Add(m20); hash.Add(m21); hash.Add(m22); hash.Add(m23);
            hash.Add(m30); hash.Add(m31); hash.Add(m32); hash.Add(m33);
            return hash.ToHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Matrix4x4([{m00}, {m01}, {m02}, {m03}], [{m10}, {m11}, {m12}, {m13}], [{m20}, {m21}, {m22}, {m23}], [{m30}, {m31}, {m32}, {m33}])";
        }
    }
}
