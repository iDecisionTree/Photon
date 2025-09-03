using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Photon.Math
{
    /// <summary>
    /// 使用列主序
    /// </summary>
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        private float _m11, _m21, _m31, _m41;
        private float _m12, _m22, _m32, _m42;
        private float _m13, _m23, _m33, _m43;
        private float _m14, _m24, _m34, _m44;

        public float m11
        {
            get => _m11;
            set => _m11 = value;
        }

        public float m21
        {
            get => _m21;
            set => _m21 = value;
        }

        public float m31
        {
            get => _m31;
            set => _m31 = value;
        }

        public float m41
        {
            get => _m41;
            set => _m41 = value;
        }

        public float m12
        {
            get => _m12;
            set => _m12 = value;
        }

        public float m22
        {
            get => _m22;
            set => _m22 = value;
        }

        public float m32
        {
            get => _m32;
            set => _m32 = value;
        }

        public float m42
        {
            get => _m42;
            set => _m42 = value;
        }

        public float m13
        {
            get => _m13;
            set => _m13 = value;
        }

        public float m23
        {
            get => _m23;
            set => _m23 = value;
        }

        public float m33
        {
            get => _m33;
            set => _m33 = value;
        }

        public float m43
        {
            get => _m43;
            set => _m43 = value;
        }

        public float m14
        {
            get => _m14;
            set => _m14 = value;
        }

        public float m24
        {
            get => _m24;
            set => _m24 = value;
        }

        public float m34
        {
            get => _m34;
            set => _m34 = value;
        }

        public float m44
        {
            get => _m44;
            set => _m44 = value;
        }

        public Matrix4x4 transposed => Transpose(this);
        public Matrix4x4 inverted => Invert(this);

        public static readonly Matrix4x4 identity = new Matrix4x4(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f);

        public static readonly Matrix4x4 zero = new Matrix4x4(
            0f, 0f, 0f, 0f,
            0f, 0f, 0f, 0f,
            0f, 0f, 0f, 0f,
            0f, 0f, 0f, 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4()
        {
            _m11 = 0f; _m21 = 0f; _m31 = 0f; _m41 = 0f;
            _m12 = 0f; _m22 = 0f; _m32 = 0f; _m42 = 0f;
            _m13 = 0f; _m23 = 0f; _m33 = 0f; _m43 = 0f;
            _m14 = 0f; _m24 = 0f; _m34 = 0f; _m44 = 0f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Matrix4x4(float m11, float m21, float m31, float m41, float m12, float m22, float m32, float m42, float m13, float m23, float m33, float m43, float m14, float m24, float m34, float m44)
        {
            _m11 = m11; _m21 = m21; _m31 = m31; _m41 = m41;
            _m12 = m12; _m22 = m22; _m32 = m32; _m42 = m42;
            _m13 = m13; _m23 = m23; _m33 = m33; _m43 = m43;
            _m14 = m14; _m24 = m24; _m34 = m34; _m44 = m44;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator +(Matrix4x4 x, Matrix4x4 y)
        {
            return new Matrix4x4(
                x.m11 + y.m11, x.m21 + y.m21, x.m31 + y.m31, x.m41 + y.m41,
                x.m12 + y.m12, x.m22 + y.m22, x.m32 + y.m32, x.m42 + y.m42,
                x.m13 + y.m13, x.m23 + y.m23, x.m33 + y.m33, x.m43 + y.m43,
                x.m14 + y.m14, x.m24 + y.m24, x.m34 + y.m34, x.m44 + y.m44);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator -(Matrix4x4 x, Matrix4x4 y)
        {
            return new Matrix4x4(
                x.m11 - y.m11, x.m21 - y.m21, x.m31 - y.m31, x.m41 - y.m41,
                x.m12 - y.m12, x.m22 - y.m22, x.m32 - y.m32, x.m42 - y.m42,
                x.m13 - y.m13, x.m23 - y.m23, x.m33 - y.m33, x.m43 - y.m43,
                x.m14 - y.m14, x.m24 - y.m24, x.m34 - y.m34, x.m44 - y.m44);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(Matrix4x4 x, Matrix4x4 y)
        {
            float z11 = x.m11 * y.m11 + x.m12 * y.m21 + x.m13 * y.m31 + x.m14 * y.m41;
            float z21 = x.m21 * y.m11 + x.m22 * y.m21 + x.m23 * y.m31 + x.m24 * y.m41;
            float z31 = x.m31 * y.m11 + x.m32 * y.m21 + x.m33 * y.m31 + x.m34 * y.m41;
            float z41 = x.m41 * y.m11 + x.m42 * y.m21 + x.m43 * y.m31 + x.m44 * y.m41;

            float z12 = x.m11 * y.m12 + x.m12 * y.m22 + x.m13 * y.m32 + x.m14 * y.m42;
            float z22 = x.m21 * y.m12 + x.m22 * y.m22 + x.m23 * y.m32 + x.m24 * y.m42;
            float z32 = x.m31 * y.m12 + x.m32 * y.m22 + x.m33 * y.m32 + x.m34 * y.m42;
            float z42 = x.m41 * y.m12 + x.m42 * y.m22 + x.m43 * y.m32 + x.m44 * y.m42;

            float z13 = x.m11 * y.m13 + x.m12 * y.m23 + x.m13 * y.m33 + x.m14 * y.m43;
            float z23 = x.m21 * y.m13 + x.m22 * y.m23 + x.m23 * y.m33 + x.m24 * y.m43;
            float z33 = x.m31 * y.m13 + x.m32 * y.m23 + x.m33 * y.m33 + x.m34 * y.m43;
            float z43 = x.m41 * y.m13 + x.m42 * y.m23 + x.m43 * y.m33 + x.m44 * y.m43;

            float z14 = x.m11 * y.m14 + x.m12 * y.m24 + x.m13 * y.m34 + x.m14 * y.m44;
            float z24 = x.m21 * y.m14 + x.m22 * y.m24 + x.m23 * y.m34 + x.m24 * y.m44;
            float z34 = x.m31 * y.m14 + x.m32 * y.m24 + x.m33 * y.m34 + x.m34 * y.m44;
            float z44 = x.m41 * y.m14 + x.m42 * y.m24 + x.m43 * y.m34 + x.m44 * y.m44;

            return new Matrix4x4(
                z11, z21, z31, z41,
                z12, z22, z32, z42,
                z13, z23, z33, z43,
                z14, z24, z34, z44);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(float x, Matrix4x4 y)
        {
            return new Matrix4x4(
                x * y.m11, x * y.m21, x * y.m31, x * y.m41,
                x * y.m12, x * y.m22, x * y.m32, x * y.m42,
                x * y.m13, x * y.m23, x * y.m33, x * y.m43,
                x * y.m14, x * y.m24, x * y.m34, x * y.m44);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 operator *(Matrix4x4 x, float y)
        {
            return new Matrix4x4(
                x.m11 * y, x.m21 * y, x.m31 * y, x.m41 * y,
                x.m12 * y, x.m22 * y, x.m32 * y, x.m42 * y,
                x.m13 * y, x.m23 * y, x.m33 * y, x.m43 * y,
                x.m14 * y, x.m24 * y, x.m34 * y, x.m44 * y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Transpose(Matrix4x4 x)
        {
            return new Matrix4x4(
                x.m11, x.m12, x.m13, x.m14,
                x.m21, x.m22, x.m23, x.m24,
                x.m31, x.m32, x.m33, x.m34,
                x.m41, x.m42, x.m43, x.m44);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryInvert(Matrix4x4 x, out Matrix4x4 result)
        {
            float a11 = x.m11, a12 = x.m12, a13 = x.m13, a14 = x.m14;
            float a21 = x.m21, a22 = x.m22, a23 = x.m23, a24 = x.m24;
            float a31 = x.m31, a32 = x.m32, a33 = x.m33, a34 = x.m34;
            float a41 = x.m41, a42 = x.m42, a43 = x.m43, a44 = x.m44;

            float b00 = a11 * a22 - a12 * a21;
            float b01 = a11 * a23 - a13 * a21;
            float b02 = a11 * a24 - a14 * a21;
            float b03 = a12 * a23 - a13 * a22;
            float b04 = a12 * a24 - a14 * a22;
            float b05 = a13 * a24 - a14 * a23;
            float b06 = a31 * a42 - a32 * a41;
            float b07 = a31 * a43 - a33 * a41;
            float b08 = a31 * a44 - a34 * a41;
            float b09 = a32 * a43 - a33 * a42;
            float b10 = a32 * a44 - a34 * a42;
            float b11 = a33 * a44 - a34 * a43;

            float det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;

            // 矩阵不可逆
            if (MathF.Abs(det) < Mathf.Epsilon)
            {
                result = zero;
                return false;
            }

            float invDet = 1f / det;

            float r11 = (a22 * b11 - a23 * b10 + a24 * b09) * invDet;
            float r12 = (-a12 * b11 + a13 * b10 - a14 * b09) * invDet;
            float r13 = (a42 * b05 - a43 * b04 + a44 * b03) * invDet;
            float r14 = (-a32 * b05 + a33 * b04 - a34 * b03) * invDet;

            float r21 = (-a21 * b11 + a23 * b08 - a24 * b07) * invDet;
            float r22 = (a11 * b11 - a13 * b08 + a14 * b07) * invDet;
            float r23 = (-a41 * b05 + a43 * b02 - a44 * b01) * invDet;
            float r24 = (a31 * b05 - a33 * b02 + a34 * b01) * invDet;

            float r31 = (a21 * b10 - a22 * b08 + a24 * b06) * invDet;
            float r32 = (-a11 * b10 + a12 * b08 - a14 * b06) * invDet;
            float r33 = (a41 * b04 - a42 * b02 + a44 * b00) * invDet;
            float r34 = (-a31 * b04 + a32 * b02 - a34 * b00) * invDet;

            float r41 = (-a21 * b09 + a22 * b07 - a23 * b06) * invDet;
            float r42 = (a11 * b09 - a12 * b07 + a13 * b06) * invDet;
            float r43 = (-a41 * b03 + a42 * b01 - a43 * b00) * invDet;
            float r44 = (a31 * b03 - a32 * b01 + a33 * b00) * invDet;

            result = new Matrix4x4(
                r11, r21, r31, r41,
                r12, r22, r32, r42,
                r13, r23, r33, r43,
                r14, r24, r34, r44);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 Invert(Matrix4x4 x)
        {
            float a11 = x.m11, a12 = x.m12, a13 = x.m13, a14 = x.m14;
            float a21 = x.m21, a22 = x.m22, a23 = x.m23, a24 = x.m24;
            float a31 = x.m31, a32 = x.m32, a33 = x.m33, a34 = x.m34;
            float a41 = x.m41, a42 = x.m42, a43 = x.m43, a44 = x.m44;

            float b00 = a11 * a22 - a12 * a21;
            float b01 = a11 * a23 - a13 * a21;
            float b02 = a11 * a24 - a14 * a21;
            float b03 = a12 * a23 - a13 * a22;
            float b04 = a12 * a24 - a14 * a22;
            float b05 = a13 * a24 - a14 * a23;
            float b06 = a31 * a42 - a32 * a41;
            float b07 = a31 * a43 - a33 * a41;
            float b08 = a31 * a44 - a34 * a41;
            float b09 = a32 * a43 - a33 * a42;
            float b10 = a32 * a44 - a34 * a42;
            float b11 = a33 * a44 - a34 * a43;

            float det = b00 * b11 - b01 * b10 + b02 * b09 + b03 * b08 - b04 * b07 + b05 * b06;

            // 矩阵不可逆
            if (MathF.Abs(det) < Mathf.Epsilon)
            {
                return zero;
            }

            float invDet = 1f / det;

            float r11 = (a22 * b11 - a23 * b10 + a24 * b09) * invDet;
            float r12 = (-a12 * b11 + a13 * b10 - a14 * b09) * invDet;
            float r13 = (a42 * b05 - a43 * b04 + a44 * b03) * invDet;
            float r14 = (-a32 * b05 + a33 * b04 - a34 * b03) * invDet;

            float r21 = (-a21 * b11 + a23 * b08 - a24 * b07) * invDet;
            float r22 = (a11 * b11 - a13 * b08 + a14 * b07) * invDet;
            float r23 = (-a41 * b05 + a43 * b02 - a44 * b01) * invDet;
            float r24 = (a31 * b05 - a33 * b02 + a34 * b01) * invDet;

            float r31 = (a21 * b10 - a22 * b08 + a24 * b06) * invDet;
            float r32 = (-a11 * b10 + a12 * b08 - a14 * b06) * invDet;
            float r33 = (a41 * b04 - a42 * b02 + a44 * b00) * invDet;
            float r34 = (-a31 * b04 + a32 * b02 - a34 * b00) * invDet;

            float r41 = (-a21 * b09 + a22 * b07 - a23 * b06) * invDet;
            float r42 = (a11 * b09 - a12 * b07 + a13 * b06) * invDet;
            float r43 = (-a41 * b03 + a42 * b01 - a43 * b00) * invDet;
            float r44 = (a31 * b03 - a32 * b01 + a33 * b00) * invDet;

            return new Matrix4x4(
                r11, r21, r31, r41,
                r12, r22, r32, r42,
                r13, r23, r33, r43,
                r14, r24, r34, r44);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Matrix4x4 other)
        {
            return _m11 == other._m11 && _m21 == other._m21 && _m31 == other._m31 && _m41 == other._m41 &&
                   _m12 == other._m12 && _m22 == other._m22 && _m32 == other._m32 && _m42 == other._m42 &&
                   _m13 == other._m13 && _m23 == other._m23 && _m33 == other._m33 && _m43 == other._m43 &&
                   _m14 == other._m14 && _m24 == other._m24 && _m34 == other._m34 && _m44 == other._m44;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Matrix4x4 other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(_m11);
            hash.Add(_m21);
            hash.Add(_m31);
            hash.Add(_m41);
            hash.Add(_m12);
            hash.Add(_m22);
            hash.Add(_m32);
            hash.Add(_m42);
            hash.Add(_m13);
            hash.Add(_m23);
            hash.Add(_m33);
            hash.Add(_m43);
            hash.Add(_m14);
            hash.Add(_m24);
            hash.Add(_m34);
            hash.Add(_m44);

            return hash.ToHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Matrix4x4[\n\t{_m11}, {_m12}, {_m13}, {_m14}\n\t{_m21}, {_m22}, {_m23}, {_m24}\n\t{_m31}, {_m32}, {_m33}, {_m34}\n\t{_m41}, {_m42}, {_m43}, {_m44}\n]";
        }
    }
}
