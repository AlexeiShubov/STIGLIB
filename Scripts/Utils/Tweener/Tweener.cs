using System;
using System.Collections.Generic;

namespace STIGRADOR.Utils
{
    public delegate float EasingFunction(float time, float from, float to, float duration);
    
    public class Tweener : ITweener, IUpdatable
    {
        private readonly bool _loop;
        private readonly Queue<TweenItem> _tweenQueue = new Queue<TweenItem>();
        
        private float _elapsedTime;
        private TweenItem _currentTween;

        public bool IsPlaying => _tweenQueue.Count > 0 || _currentTween != null;

        public static ITweener CreateTweener(bool loop = false) => new Tweener(loop);

        private Tweener(bool loop)
        {
            _loop = loop;
        }
        
        public void DoUpdate(float deltaTime)
        {
            if (!IsPlaying) return;

            _elapsedTime += deltaTime;

            if (_currentTween == null)
            {
                NextTween();
                
                return;
            }

            var progress = Math.Min(_elapsedTime / _currentTween.Duration, 1f);
            var value = _currentTween.Easing(progress, _currentTween.From, _currentTween.To, _currentTween.Duration);
            
            _currentTween.Action(value);

            if (_elapsedTime >= _currentTween.Duration)
            {
                NextTween();
            }
        }

        internal Tweener Add(EasingFunction easing, float duration, float from, float to, Action<float> action)
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
        
        public ITweener Wait(float duration)
        {
            return Add((t, f, to, d) => to, duration, 0, 0, _ => { });
        }

        public ITweener Action(Action action)
        {
            return Add((t, f, to, d) => to, 0, 0, 0, _ => action());
        }

        public void Stop(bool finalizeCurrentTween = false)
        {
            if (finalizeCurrentTween)
            {
                _currentTween.Action?.Invoke(1f);
            }
            
            _tweenQueue.Clear();
            _currentTween = null;
            _elapsedTime = 0;
        }

        private void NextTween()
        {
            if (_loop && _currentTween != null)
            {
                _tweenQueue.Enqueue(_currentTween);
            }

            _elapsedTime = 0;
            _currentTween = _tweenQueue.Count > 0 ? _tweenQueue.Dequeue() : null;
        }

        private class TweenItem
        {
            public EasingFunction Easing { get; set; }
            public float Duration { get; set; }
            public float From { get; set; }
            public float To { get; set; }
            public Action<float> Action { get; set; }
        }
    }
}