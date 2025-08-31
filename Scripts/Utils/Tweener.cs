using System;
using System.Collections.Generic;

namespace STIGRADOR
{
    public delegate void TweenAction(float value);
    public delegate float EasingFunction(float time, float from, float to, float duration);

    public class Tweener
    {
        private readonly Queue<TweenItem> _tweenQueue = new Queue<TweenItem>();
        
        private TweenItem _currentTween;
        private readonly bool _loop;

        public float ElapsedTime { get; private set; }
        public bool IsPlaying => _tweenQueue.Count > 0 || _currentTween != null;

        public Tweener(bool loop = false)
        {
            _loop = loop;
        }

        public static Tweener Create() => new Tweener();
        public static Tweener CreateLoop() => new Tweener(true);

        public Tweener Add(EasingFunction easing, float duration, float from, float to, TweenAction action)
        {
            _tweenQueue.Enqueue(new TweenItem
            {
                Easing = easing,
                Duration = duration,
                From = from,
                To = to,
                Action = action
            });
            
            return this;
        }

        public Tweener Wait(float duration)
        {
            return Add((t, f, to, d) => to, duration, 0, 0, _ => { });
        }

        public Tweener Action(Action action)
        {
            return Add((t, f, to, d) => to, 0, 0, 0, _ => action());
        }

        public void Stop()
        {
            _tweenQueue.Clear();
            _currentTween = null;
            ElapsedTime = 0;
        }

        public void DoUpdate(float deltaTime)
        {
            if (!IsPlaying) return;

            ElapsedTime += deltaTime;

            if (_currentTween == null)
            {
                NextTween();
                
                return;
            }

            var progress = Math.Min(ElapsedTime / _currentTween.Duration, 1f);
            var value = _currentTween.Easing(progress, _currentTween.From, _currentTween.To, _currentTween.Duration);
            
            _currentTween.Action(value);

            if (ElapsedTime >= _currentTween.Duration)
            {
                NextTween();
            }
        }

        private void NextTween()
        {
            if (_loop && _currentTween != null)
            {
                _tweenQueue.Enqueue(_currentTween);
            }

            if (_tweenQueue.Count > 0)
            {
                _currentTween = _tweenQueue.Dequeue();
                ElapsedTime = 0;
            }
            else
            {
                _currentTween = null;
            }
        }

        private class TweenItem
        {
            public EasingFunction Easing { get; set; }
            public float Duration { get; set; }
            public float From { get; set; }
            public float To { get; set; }
            public TweenAction Action { get; set; }
        }
    }

    public static class TweenExtensions
    {
        public static Tweener Linear(this Tweener tweener, float duration, float from, float to, TweenAction action)
        {
            return tweener.Add(EasingFunctions.Linear, duration, from, to, action);
        }

        public static Tweener EaseInOut(this Tweener tweener, float duration, float from, float to, TweenAction action)
        {
            return tweener.Add(EasingFunctions.EaseInOut, duration, from, to, action);
        }

        public static Tweener EaseIn(this Tweener tweener, float duration, float from, float to, TweenAction action)
        {
            return tweener.Add(EasingFunctions.EaseIn, duration, from, to, action);
        }

        public static Tweener EaseOut(this Tweener tweener, float duration, float from, float to, TweenAction action)
        {
            return tweener.Add(EasingFunctions.EaseOut, duration, from, to, action);
        }

        public static Tweener Bounce(this Tweener tweener, float duration, float from, float to, TweenAction action)
        {
            return tweener.Add(EasingFunctions.Bounce, duration, from, to, action);
        }

        public static Tweener Elastic(this Tweener tweener, float duration, float from, float to, TweenAction action)
        {
            return tweener.Add(EasingFunctions.Elastic, duration, from, to, action);
        }
    }

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