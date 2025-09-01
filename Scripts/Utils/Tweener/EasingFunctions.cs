using System;

namespace STIGRADOR.Utils
{
    public static class EasingFunctions
    {
        public static float Linear(float time, float from, float to, float duration)
        {
            return from + (to - from) * time;
        }

        public static float EaseIn(float time, float from, float to, float duration)
        {
            return from + (to - from) * time * time;
        }

        public static float EaseOut(float time, float from, float to, float duration)
        {
            return from + (to - from) * (1 - (1 - time) * (1 - time));
        }

        public static float EaseInOut(float time, float from, float to, float duration)
        {
            return time < 0.5f
                ? from + (to - from) * 2 * time * time
                : from + (to - from) * (1 - 2 * (1 - time) * (1 - time));
        }

        public static float Bounce(float time, float from, float to, float duration)
        {
            const float inv275 = 1 / 2.75f;
            const float threshold1 = 1 * inv275;
            const float threshold2 = 2 * inv275;
            const float threshold3 = 2.5f * inv275;

            const float adjust1 = 1.5f * inv275;
            const float adjust2 = 2.25f * inv275;
            const float adjust3 = 2.625f * inv275;

            switch (time)
            {
                case var t when t < threshold1:
                    return to * 7.5625f * time * time + from;

                case var t when t < threshold2:
                    time -= adjust1;
                    return to * (7.5625f * time * time + 0.75f) + from;

                case var t when t < threshold3:
                    time -= adjust2;
                    return to * (7.5625f * time * time + 0.9375f) + from;

                default:
                    time -= adjust3;
                    return to * (7.5625f * time * time + 0.984375f) + from;
            }
        }

        public static float Elastic(float time, float from, float to, float duration)
        {
            return to * MathF.Sin(13 * MathF.PI / 2 * time) * MathF.Pow(2, 10 * (time - 1)) + from;
        }
    }
}