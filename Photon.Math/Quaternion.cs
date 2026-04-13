using Photon.Math.Vector;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Photon.Math
{
    public readonly struct Quaternion : IEquatable<Quaternion>
    {
        public readonly float x;
        public readonly float y;
        public readonly float z;
        public readonly float w;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternion conjugated => Conjugate(this);
        public Quaternion inverted => Invert(this);
        public float length => Length(this);
        public float lengthSquared => LengthSquared(this);
        public Quaternion normalized => Normalize(this);

        public static readonly Quaternion identity = new Quaternion(0f, 0f, 0f, 1f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator +(Quaternion a, float b)
        {
            return new Quaternion(a.x + b, a.y + b, a.z + b, a.w + b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator +(float a, Quaternion b)
        {
            return new Quaternion(a + b.x, a + b.y, a + b.z, a + b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator -(Quaternion a, float b)
        {
            return new Quaternion(a.x - b, a.y - b, a.z - b, a.w - b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator -(float a, Quaternion b)
        {
            return new Quaternion(a - b.x, a - b.y, a - b.z, a - b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator -(Quaternion q)
        {
            return new Quaternion(-q.x, -q.y, -q.z, -q.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
                a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
                a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 operator *(Quaternion q, Vector3 v)
        {
            Vector3 u = new Vector3(q.x, q.y, q.z);
            Vector3 t = 2f * Vector3.Cross(u, v);

            return v + q.w * t + Vector3.Cross(u, t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator *(Quaternion a, float b)
        {
            return new Quaternion(a.x * b, a.y * b, a.z * b, a.w * b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator *(float a, Quaternion b)
        {
            return new Quaternion(a * b.x, a * b.y, a * b.z, a * b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion operator /(Quaternion a, float b)
        {
            if (Mathf.Approximately(b, 0f))
            {
                throw new DivideByZeroException($"{a.ToString()}不能除以0");
            }

            float inv = 1f / b;
            return new Quaternion(a.x * inv, a.y * inv, a.z * inv, a.w * inv);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Quaternion a, Quaternion b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z) && Mathf.Approximately(a.w, b.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return !(a == b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quaternion other)
        {
            return this == other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion(-q.x, -q.y, -q.z, q.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        /// <summary>
        /// Z-X-Y内旋, 接受弧度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion FromEulerAngles(Vector3 eulerAngles)
        {
            float hz = eulerAngles.z * 0.5f;
            float hx = eulerAngles.x * 0.5f;
            float hy = eulerAngles.y * 0.5f;

            float cz = Mathf.Cos(hz);
            float sz = Mathf.Sin(hz);
            float cx = Mathf.Cos(hx);
            float sx = Mathf.Sin(hx);
            float cy = Mathf.Cos(hy);
            float sy = Mathf.Sin(hy);

            Quaternion qz = new Quaternion(0f, 0f, sz, cz);
            Quaternion qx = new Quaternion(sx, 0f, 0f, cx);
            Quaternion qy = new Quaternion(0f, sy, 0f, cy);

            return qy * qx * qz;
        }

        /// <summary>
        /// Z-X-Y内旋, 接受弧度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion FromEulerAngles(float x, float y, float z)
        {
            float hz = z * 0.5f;
            float hx = x * 0.5f;
            float hy = y * 0.5f;

            float cz = Mathf.Cos(hz);
            float sz = Mathf.Sin(hz);
            float cx = Mathf.Cos(hx);
            float sx = Mathf.Sin(hx);
            float cy = Mathf.Cos(hy);
            float sy = Mathf.Sin(hy);

            Quaternion qz = new Quaternion(0f, 0f, sz, cz);
            Quaternion qx = new Quaternion(sx, 0f, 0f, cx);
            Quaternion qy = new Quaternion(0f, sy, 0f, cy);

            return qy * qx * qz;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Invert(Quaternion q)
        {
            float lengthSquared = q.lengthSquared;
            if (Mathf.Approximately(lengthSquared, 0f))
            {
                throw new InvalidOperationException($"四元数{q}不可逆");
            }
            return q.conjugated / q.lengthSquared;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Length(Quaternion q)
        {
            return Mathf.Sqrt(q.lengthSquared);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LengthSquared(Quaternion q)
        {
            return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
        {
            return (a + (b - a) * Mathf.Clamp01(t)).normalized;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            return (a + (b - a) * t).normalized;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Normalize(Quaternion q)
        {
            float length = q.length;
            if (Mathf.Approximately(length, 0f))
            {
                return identity;
            }
            return q / length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 RotateVector(Quaternion q, Vector3 v)
        {
            return q * v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
        {
            t = Mathf.Clamp01(t);
            float dot = Dot(a, b);

            if (dot < 0f)
            {
                a = -a;
                dot = -dot;
            }

            if (dot > 0.9995f)
            {
                return Lerp(a, b, t);
            }

            float theta0 = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f));
            float theta = theta0 * t;
            float sinTheta = Mathf.Sin(theta);
            float sinTheta0 = Mathf.Sin(theta0);

            float s0 = Mathf.Cos(theta) - dot * sinTheta / sinTheta0;
            float s1 = sinTheta / sinTheta0;

            return new Quaternion(
                s0 * a.x + s1 * b.x,
                s0 * a.y + s1 * b.y,
                s0 * a.z + s1 * b.z,
                s0 * a.w + s1 * b.w
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Quaternion q && this == q;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"Quaternion({x}, {y}, {z}, {w})";
        }
    }
}
