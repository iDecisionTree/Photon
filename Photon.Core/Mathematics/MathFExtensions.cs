namespace Photon.Core.Mathematics;

public static class MathFExtensions
{
    extension(MathF)
    {
        public static float epsilon => 1e-5f;

        public static bool Approximately(float a, float b)
        {
            if (float.IsNaN(a) || float.IsNaN(b))
            {
                return false;
            }

            if (float.IsInfinity(a) || float.IsInfinity(b))
            {
                return a == b;
            }

            if (a == b)
            {
                return true;
            }

            float diff = MathF.Abs(a - b);
            float scale = MathF.Max(MathF.Abs(a), MathF.Abs(b));

            return diff <= MathF.Max(MathF.epsilon, MathF.epsilon * scale);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Clamp(value, min, max);
        }

        public static float Clamp01(float value)
        {
            return MathF.Clamp(value, 0f, 1f);
        }

        public static float DegToRad(float degrees)
        {
            return degrees * (MathF.PI / 180f);
        }

        public static float InverseLerp(float a, float b, float value)
        {
            return MathF.Clamp01(MathF.InverseLerpUnclamped(a, b, value));
        }

        public static float InverseLerpUnclamped(float a, float b, float value)
        {
            if (a == b)
            {
                return 0f;
            }

            float difference = value - a;
            float range = b - a;
            if ((float.IsInfinity(difference) || float.IsInfinity(range)) &&
                float.IsFinite(a) && float.IsFinite(b) && float.IsFinite(value))
            {
                return (float)(((double)value - a) / ((double)b - a));
            }

            return difference / range;
        }

        public static float Lerp(float a, float b, float t)
        {
            return MathF.LerpUnclamped(a, b, MathF.Clamp01(t));
        }

        public static float LerpUnclamped(float a, float b, float t)
        {
            if (t == 0f)
            {
                return a;
            }

            if (t == 1f)
            {
                return b;
            }

            float difference = b - a;
            if (float.IsInfinity(difference) && float.IsFinite(a) && float.IsFinite(b))
            {
                return (float)(a + ((double)b - a) * t);
            }

            return a + difference * t;
        }

        public static float MoveTowards(float current, float target, float maxDelta)
        {
            float difference = target - current;
            if (float.IsNaN(difference))
            {
                return current == target ? target : float.NaN;
            }

            if (difference == 0f || MathF.Abs(difference) <= maxDelta)
            {
                return target;
            }

            return current + MathF.CopySign(1f, difference) * maxDelta;
        }

        public static float RadToDeg(float radians)
        {
            return radians * (180f / MathF.PI);
        }

        public static float Remap(float value, float inputMin, float inputMax, float outputMin, float outputMax)
        {
            if (inputMin == inputMax)
            {
                throw new ArgumentException("Input range cannot be zero.", nameof(inputMax));
            }

            float t = MathF.InverseLerpUnclamped(inputMin, inputMax, value);
            return MathF.LerpUnclamped(outputMin, outputMax, t);
        }

        public static float SmoothStep(float a, float b, float t)
        {
            t = MathF.Clamp01(t);
            t = t * t * (3f - 2f * t);
            return MathF.LerpUnclamped(a, b, t);
        }

        public static float SmootherStep(float a, float b, float t)
        {
            t = MathF.Clamp01(t);
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            return MathF.LerpUnclamped(a, b, t);
        }

        public static float Step(float edge, float value)
        {
            if (float.IsNaN(edge) || float.IsNaN(value))
            {
                return float.NaN;
            }

            return value < edge ? 0f : 1f;
        }
    }
}
