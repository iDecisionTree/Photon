using System.Runtime.CompilerServices;

namespace Photon.Math
{
    public static class Mathf
    {
        public const float PI = MathF.PI;
        public const float E = MathF.E;
        public const float Deg2Rad = MathF.PI / 180f;
        public const float Rad2Deg = 180f / MathF.PI;
        public const float Epsilon = 1e-6f;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(float x) => MathF.Abs(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sign(float x) => MathF.Sign(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float x, float y) => MathF.Min(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float x, float y) => MathF.Max(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(float x) => MathF.Sqrt(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(float x, float y) => MathF.Pow(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Exp(float x) => MathF.Exp(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(float x) => MathF.Log(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(float x, float y) => MathF.Log(x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log10(float x) => MathF.Log10(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log2(float x) => MathF.Log2(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float x) => MathF.Sin(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float x) => MathF.Cos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(float x) => MathF.Tan(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asin(float x) => MathF.Asin(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acos(float x) => MathF.Acos(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan(float x) => MathF.Atan(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan2(float y, float x) => MathF.Atan2(y, x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(float x) => MathF.Floor(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceiling(float x) => MathF.Ceiling(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float x) => MathF.Round(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToInt(float x) => (int)MathF.Floor(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilingToInt(float x) => (int)MathF.Ceiling(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RoundToInt(float x) => (int)MathF.Round(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float x, float min, float max)
        {
            if (x < min)
            {
                return min;
            }
            if (x > max)
            {
                return max;
            }

            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp01(float x)
        {
            if (x < 0f)
            {
                return 0f;
            }
            if (x > 1f)
            {
                return 1f;
            }

            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp01(t);
            t = t * t * (3f - 2f * t);

            return a + (b - a) * t;
        }
    }
}
