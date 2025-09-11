namespace Photon.Core
{
    public static class Mathf
    {
        public const float PI = MathF.PI;
        public const float E = MathF.E;
        public const float Deg2Rad = MathF.PI / 180f;
        public const float Rad2Deg = 180f / MathF.PI;
        public const float Epsilon = 1e-6f;

        public static float Abs(float x) => MathF.Abs(x);

        public static int Sign(float x) => MathF.Sign(x);

        public static float CopySign(float x, float y) => MathF.CopySign(x, y);

        public static float Min(float x, float y) => MathF.Min(x, y);

        public static float Max(float x, float y) => MathF.Max(x, y);

        public static float Sqrt(float x) => MathF.Sqrt(x);

        public static float Pow(float x, float y) => MathF.Pow(x, y);

        public static float Exp(float x) => MathF.Exp(x);

        public static float Log(float x) => MathF.Log(x);

        public static float Log(float x, float y) => MathF.Log(x, y);

        public static float Log10(float x) => MathF.Log10(x);

        public static float Log2(float x) => MathF.Log2(x);

        public static float Sin(float x) => MathF.Sin(x);

        public static float Cos(float x) => MathF.Cos(x);

        public static float Tan(float x) => MathF.Tan(x);

        public static float Asin(float x) => MathF.Asin(x);

        public static float Acos(float x) => MathF.Acos(x);

        public static float Atan(float x) => MathF.Atan(x);

        public static float Atan2(float y, float x) => MathF.Atan2(y, x);

        public static float Floor(float x) => MathF.Floor(x);

        public static float Ceiling(float x) => MathF.Ceiling(x);

        public static float Round(float x) => MathF.Round(x);

        public static int FloorToInt(float x) => (int)MathF.Floor(x);

        public static int CeilingToInt(float x) => (int)MathF.Ceiling(x);

        public static int RoundToInt(float x) => (int)MathF.Round(x);

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

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float LerpUnclamped(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float SmoothStep(float a, float b, float t)
        {
            t = Clamp01(t);
            t = t * t * (3f - 2f * t);

            return a + (b - a) * t;
        }
    }
}
