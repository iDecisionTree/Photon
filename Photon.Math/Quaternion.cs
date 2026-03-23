using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Photon.Math
{
    public readonly struct Quaternion : IEquatable<Quaternion>
    {
        public readonly float x;
        public readonly float y;
        public readonly float z;
        public readonly float w;

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

        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public static Quaternion operator +(Quaternion a, float b)
        {
            return new Quaternion(a.x + b, a.y + b, a.z + b, a.w + b);
        }

        public static Quaternion operator +(float a, Quaternion b)
        {
            return new Quaternion(a + b.x, a + b.y, a + b.z, a + b.w);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public static Quaternion operator -(Quaternion a, float b)
        {
            return new Quaternion(a.x - b, a.y - b, a.z - b, a.w - b);
        }

        public static Quaternion operator -(float a, Quaternion b)
        {
            return new Quaternion(a - b.x, a - b.y, a - b.z, a - b.w);
        }

        public static Quaternion operator -(Quaternion q)
        {
            return new Quaternion(-q.x, -q.y, -q.z, -q.w);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            return new Quaternion(
                a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
                a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x,
                a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
                a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z
            );
        }

        public static Vector3 operator *(Quaternion a, Vector3 b)
        {
            float qx = a.x, qy = a.y, qz = a.z, qw = a.w;
            float vx = b.x, vy = b.y, vz = b.z;

            float crossX = qy * vz - qz * vy;
            float crossY = qz * vx - qx * vz;
            float crossZ = qx * vy - qy * vx;

            float dot = qx * vx + qy * vy + qz * vz;

            float x = vx * (qw * qw - qx * qx - qy * qy + qz * qz) + 2f * (qy * crossY + qz * crossZ + dot * qx);
            float y = vy * (qw * qw - qx * qx + qy * qy - qz * qz) + 2f * (qx * crossX + qz * crossZ + dot * qy);
            float z = vz * (qw * qw + qx * qx - qy * qy - qz * qz) + 2f * (qx * crossX + qy * crossY + dot * qz);

            return new Vector3(x, y, z);
        }

        public static Vector3 operator *(Vector3 a, Quaternion b)
        {
            float vx = a.x, vy = a.y, vz = a.z;
            float qx = b.x, qy = b.y, qz = b.z, qw = b.w;

            float crossX = qy * vz - qz * vy;
            float crossY = qz * vx - qx * vz;
            float crossZ = qx * vy - qy * vx;

            float dot = qx * vx + qy * vy + qz * vz;

            float x = vx * (qw * qw - qx * qx - qy * qy + qz * qz) + 2f * (qy * crossY + qz * crossZ + dot * qx);
            float y = vy * (qw * qw - qx * qx + qy * qy - qz * qz) + 2f * (qx * crossX + qz * crossZ + dot * qy);
            float z = vz * (qw * qw + qx * qx - qy * qy - qz * qz) + 2f * (qx * crossX + qy * crossY + dot * qz);

            return new Vector3(x, y, z);
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
            if (Mathf.Approximately(b, 0f))
            {
                throw new DivideByZeroException($"{a.ToString()}不能除以0");
            }

            float inv = 1f / b; 
            return new Quaternion(a.x * inv, a.y * inv, a.z * inv, a.w * inv);
        }

        public static bool operator ==(Quaternion a, Quaternion b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z) && Mathf.Approximately(a.w, b.w);
        }

        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return !(a == b);
        }

        public bool Equals(Quaternion other)
        {
            return this == other;
        }

        public static Quaternion Conjugate(Quaternion q)
        {
            return new Quaternion(-q.x, -q.y, -q.z, q.w);
        }

        public static float Distance(Quaternion a, Quaternion b)
        {
            return (a - b).length;
        }

        public static float DistanceSquared(Quaternion a, Quaternion b)
        {
            return (a - b).lengthSquared;
        }

        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static Quaternion FromEulerAngles(float x, float y, float z)
        {
            float cx = Mathf.Cos(x * 0.5f);
            float cy = Mathf.Cos(y * 0.5f);
            float cz = Mathf.Cos(z * 0.5f);
            float sx = Mathf.Sin(x * 0.5f);
            float sy = Mathf.Sin(y * 0.5f);
            float sz = Mathf.Sin(z * 0.5f);

            return new Quaternion(
                sx * cy * cz - cx * sy * sz,
                cx * sy * cz + sx * cy * sz,
                cx * cy * sz - sx * sy * cz,
                cx * cy * cz + sx * sy * sz
            );
        }

        /// <summary>
        /// Z-X-Y内旋, 接受弧度
        /// </summary>
        public static Quaternion FromEuler(float yaw, float pitch, float roll)
        {
            float cy = Mathf.Cos(yaw * 0.5f);
            float sy = Mathf.Sin(yaw * 0.5f);
            float cx = Mathf.Cos(pitch * 0.5f);
            float sx = Mathf.Sin(pitch * 0.5f);
            float cz = Mathf.Cos(roll * 0.5f);
            float sz = Mathf.Sin(roll * 0.5f);

            return new Quaternion(
                sx * cy * cz + cx * sy * sz,
                cx * sy * cz - sx * cy * sz,
                cx * cy * sz - sx * sy * cz,
                cx * cy * cz + sx * sy * sz
            );
        }

        public static Quaternion Invert(Quaternion q)
        {
            float lengthSquared = q.lengthSquared;
            if (Mathf.Approximately(lengthSquared, 0f))
            {
                return identity;
            }
            return q.conjugated / q.lengthSquared;
        }

        public static float Length(Quaternion q)
        {
            return Mathf.Sqrt(q.lengthSquared);
        }

        public static float LengthSquared(Quaternion q)
        {
            return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
        }

        public static Quaternion Lerp(Quaternion a, Quaternion b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        public static Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            return a + (b - a) * t;
        }

        public static Quaternion Normalize(Quaternion q)
        {
            float length = q.length;
            if (Mathf.Approximately(length, 0f))
            {
                return identity;
            }
            return q / length;
        }

        public static Vector3 RotateVector(Quaternion q, Vector3 v)
        {
            return q * v;
        }

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

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Quaternion q && this == q;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z, w);
        }

        public override string ToString()
        {
            return $"Quaternion({x}, {y}, {z}, {w})";
        }
    }
}
