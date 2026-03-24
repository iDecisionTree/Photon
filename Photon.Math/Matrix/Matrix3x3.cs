using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Math.Matrix
{
    /// <summary>
    /// 行主序, 列向量, 左手系
    /// </summary>
    public readonly struct Matrix3x3 : IEquatable<Matrix3x3>
    {
        public readonly float m00, m01, m02;
        public readonly float m10, m11, m12;
        public readonly float m20, m21, m22;

        public Matrix3x3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        /// <summary>
        /// 接受列向量
        /// </summary>
        public Matrix3x3(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            m00 = v0.x;
            m10 = v0.y;
            m20 = v0.z;
            m01 = v1.x;
            m11 = v1.y;
            m21 = v1.z;
            m02 = v2.x;
            m12 = v2.y;
            m22 = v2.z;
        }

        public float determinant => Determinant(this);
        public Matrix3x3 inverted => Invert(this);
        public Matrix3x3 transposed => Transpose(this);

        public static readonly Matrix3x3 identity = new Matrix3x3(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);
        public static readonly Matrix3x3 zero = new Matrix3x3(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);

        public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02,
                a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12,
                a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22
            );
        }

        public static Matrix3x3 operator +(Matrix3x3 a, float b)
        {
            return new Matrix3x3(
                a.m00 + b, a.m01 + b, a.m02 + b,
                a.m10 + b, a.m11 + b, a.m12 + b,
                a.m20 + b, a.m21 + b, a.m22 + b
            );
        }

        public static Matrix3x3 operator +(float a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a + b.m00, a + b.m01, a + b.m02,
                a + b.m10, a + b.m11, a + b.m12,
                a + b.m20, a + b.m21, a + b.m22
            );
        }

        public static Matrix3x3 operator -(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a.m00 - b.m00, a.m01 - b.m01, a.m02 - b.m02,
                a.m10 - b.m10, a.m11 - b.m11, a.m12 - b.m12,
                a.m20 - b.m20, a.m21 - b.m21, a.m22 - b.m22
            );
        }

        public static Matrix3x3 operator -(Matrix3x3 a, float b)
        {
            return new Matrix3x3(
                a.m00 - b, a.m01 - b, a.m02 - b,
                a.m10 - b, a.m11 - b, a.m12 - b,
                a.m20 - b, a.m21 - b, a.m22 - b
            );
        }

        public static Matrix3x3 operator -(float a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a - b.m00, a - b.m01, a - b.m02,
                a - b.m10, a - b.m11, a - b.m12,
                a - b.m20, a - b.m21, a - b.m22
            );
        }

        public static Matrix3x3 operator -(Matrix3x3 a)
        {
            return new Matrix3x3(
                -a.m00, -a.m01, -a.m02,
                -a.m10, -a.m11, -a.m12,
                -a.m20, -a.m21, -a.m22
            );
        }

        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20,
                a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21,
                a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22,

                a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20,
                a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21,
                a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22,

                a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20,
                a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21,
                a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22
            );
        }

        public static Vector3 operator *(Matrix3x3 m, Vector3 v)
        {
            return new Vector3(
                m.m00 * v.x + m.m01 * v.y + m.m02 * v.z,
                m.m10 * v.x + m.m11 * v.y + m.m12 * v.z,
                m.m20 * v.x + m.m21 * v.y + m.m22 * v.z
            );
        }

        public static Matrix3x3 operator *(Matrix3x3 a, float b)
        {
            return new Matrix3x3(
                a.m00 * b, a.m01 * b, a.m02 * b,
                a.m10 * b, a.m11 * b, a.m12 * b,
                a.m20 * b, a.m21 * b, a.m22 * b
            );
        }

        public static Matrix3x3 operator *(float a, Matrix3x3 b)
        {
            return new Matrix3x3(
                a * b.m00, a * b.m01, a * b.m02,
                a * b.m10, a * b.m11, a * b.m12,
                a * b.m20, a * b.m21, a * b.m22
            );
        }

        public static Matrix3x3 operator /(Matrix3x3 a, float b)
        {
            if (Mathf.Approximately(b, 0f))
            {
                throw new DivideByZeroException($"{a.ToString()}不能除以0");
            }

            float inv = 1f / b;
            return new Matrix3x3(
                a.m00 * inv, a.m01 * inv, a.m02 * inv,
                a.m10 * inv, a.m11 * inv, a.m12 * inv,
                a.m20 * inv, a.m21 * inv, a.m22 * inv
            );
        }

        public static bool operator ==(Matrix3x3 a, Matrix3x3 b)
        {
            return Mathf.Approximately(a.m00, b.m00) && Mathf.Approximately(a.m01, b.m01) && Mathf.Approximately(a.m02, b.m02) &&
                   Mathf.Approximately(a.m10, b.m10) && Mathf.Approximately(a.m11, b.m11) && Mathf.Approximately(a.m12, b.m12) &&
                   Mathf.Approximately(a.m20, b.m20) && Mathf.Approximately(a.m21, b.m21) && Mathf.Approximately(a.m22, b.m22);
        }

        public static bool operator !=(Matrix3x3 a, Matrix3x3 b)
        {
            return !(a == b);
        }

        public bool Equals(Matrix3x3 other)
        {
            return this == other;
        }

        public static Matrix3x3 CreateScale(Vector3 scale)
        {
            return new Matrix3x3(
                scale.x, 0f, 0f,
                0f, scale.y, 0f,
                0f, 0f, scale.z
            );
        }

        /// <summary>
        /// 接受弧度
        /// </summary>
        public static Matrix3x3 CreateRotationX(float x)
        {
            float cos = Mathf.Cos(x);
            float sin = Mathf.Sin(x);

            return new Matrix3x3(
                1f, 0f, 0f,
                0f, cos, -sin,
                0f, sin, cos
            );
        }

        /// <summary>
        /// 接受弧度
        /// </summary>
        public static Matrix3x3 CreateRotationY(float y)
        {
            float cos = Mathf.Cos(y);
            float sin = Mathf.Sin(y);

            return new Matrix3x3(
                cos, 0f, sin,
                0f, 1f, 0f,
               -sin, 0f, cos
            );
        }

        /// <summary>
        /// 接受弧度
        /// </summary>
        public static Matrix3x3 CreateRotationZ(float z)
        {
            float cos = Mathf.Cos(z);
            float sin = Mathf.Sin(z);

            return new Matrix3x3(
                cos, -sin, 0f,
                sin, cos, 0f,
                0f, 0f, 1f
            );
        }

        /// <summary>
        /// Z-X-Y内旋, 接受弧度
        /// </summary>
        public static Matrix3x3 CreateFromEulerAngles(float z, float x, float y)
        {
            Matrix3x3 rz = CreateRotationZ(z);
            Matrix3x3 rx = CreateRotationX(x);
            Matrix3x3 ry = CreateRotationY(y);

            return rz * rx * ry;
        }

        public static float Determinant(Matrix3x3 m)
        {
            return m.m00 * (m.m11 * m.m22 - m.m12 * m.m21) - m.m01 * (m.m10 * m.m22 - m.m12 * m.m20) + m.m02 * (m.m10 * m.m21 - m.m11 * m.m20);
        }

        public static Matrix3x3 Invert(Matrix3x3 m)
        {
            float det = m.determinant;
            if (Mathf.Approximately(det, 0f))
            {
                throw new InvalidOperationException($"矩阵{m}不可逆");
            }

            float invDet = 1f / det;

            float c00 = (m.m11 * m.m22 - m.m12 * m.m21) * invDet;
            float c01 = -(m.m10 * m.m22 - m.m12 * m.m20) * invDet;
            float c02 = (m.m10 * m.m21 - m.m11 * m.m20) * invDet;

            float c10 = -(m.m01 * m.m22 - m.m02 * m.m21) * invDet;
            float c11 = (m.m00 * m.m22 - m.m02 * m.m20) * invDet;
            float c12 = -(m.m00 * m.m21 - m.m01 * m.m20) * invDet;

            float c20 = (m.m01 * m.m12 - m.m02 * m.m11) * invDet;
            float c21 = -(m.m00 * m.m12 - m.m02 * m.m10) * invDet;
            float c22 = (m.m00 * m.m11 - m.m01 * m.m10) * invDet;

            return new Matrix3x3(
                c00, c10, c20,
                c01, c11, c21,
                c02, c12, c22
            );
        }

        public static Matrix3x3 Transpose(Matrix3x3 m)
        {
            return new Matrix3x3(
                m.m00, m.m10, m.m20,
                m.m01, m.m11, m.m21,
                m.m02, m.m12, m.m22
            );
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Matrix3x3 m && this == m;
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(m00); hash.Add(m01); hash.Add(m02);
            hash.Add(m10); hash.Add(m11); hash.Add(m12);
            hash.Add(m20); hash.Add(m21); hash.Add(m22);
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            return $"Matrix3x3([{m00}, {m01}, {m02}], [{m10}, {m11}, {m12}], [{m20}, {m21}, {m22}])";
        }
    }
}