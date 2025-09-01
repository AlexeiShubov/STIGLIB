using System;

namespace STIGRADOR.Utils
{
    public static class TweenExtensions
    {
        public static ITweener Linear(this ITweener tweener, float duration, float from, float to, Action<float> action)
        {
            return ((Tweener) tweener).Add(EasingFunctions.Linear, duration, from, to, action);
        }

        public static ITweener EaseInOut(this ITweener tweener, float duration, float from, float to,
            Action<float> action)
        {
            return ((Tweener) tweener).Add(EasingFunctions.EaseInOut, duration, from, to, action);
        }

        public static ITweener EaseIn(this ITweener tweener, float duration, float from, float to, Action<float> action)
        {
            return ((Tweener) tweener).Add(EasingFunctions.EaseIn, duration, from, to, action);
        }

        public static ITweener EaseOut(this ITweener tweener, float duration, float from, float to,
            Action<float> action)
        {
            return ((Tweener) tweener).Add(EasingFunctions.EaseOut, duration, from, to, action);
        }

        public static ITweener Bounce(this ITweener tweener, float duration, float from, float to, Action<float> action)
        {
            return ((Tweener) tweener).Add(EasingFunctions.Bounce, duration, from, to, action);
        }

        public static ITweener Elastic(this ITweener tweener, float duration, float from, float to,
            Action<float> action)
        {
            return ((Tweener) tweener).Add(EasingFunctions.Elastic, duration, from, to, action);
        }
    }
}