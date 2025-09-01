using System;

namespace STIGRADOR.Utils
{
    public interface ITweener
    {
        ITweener Wait(float duration);
        ITweener Action(Action action);
        void Stop(bool finalizeCurrentTween = false);
        bool IsPlaying { get; }
    }
}