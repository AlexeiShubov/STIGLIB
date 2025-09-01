using System;
using System.Collections.Generic;
using UnityEngine;

namespace STIGRADOR.MVVM
{
    public abstract class BaseEventManager : IInvoker
    {
        protected readonly Dictionary<string, HashSet<Delegate>> _eventHandlers = new Dictionary<string, HashSet<Delegate>>();

        public virtual void Bind(string eventName, Delegate handler)
        {
            InternalBind(eventName, handler);
        }
        
        public virtual void UnBind(string eventName, Delegate handler)
        {
            if (!_eventHandlers.TryGetValue(eventName, out var handlers)) return;
            
            handlers.Remove(handler);

            if (handlers.Count == 0)
            {
                _eventHandlers.Remove(eventName);
            }
        }

        #region Invoke Methods

        public virtual void Invoke(string eventName)
        {
            InvokeInternal<Action>(eventName, action => action());
        }

        public virtual void Invoke<A>(string eventName, A arg)
        {
            InvokeInternal<Action<A>>(eventName, action => action(arg));
        }

        public virtual void Invoke<A, B>(string eventName, A arg1, B arg2)
        {
            InvokeInternal<Action<A, B>>(eventName, action => action(arg1, arg2));
        }

        public virtual void Invoke<A, B, C>(string eventName, A arg1, B arg2, C arg3)
        {
            InvokeInternal<Action<A, B, C>>(eventName, action => action(arg1, arg2, arg3));
        }

        protected virtual void InvokeInternal<T>(string eventName, Action<T> invokeAction) where T : Delegate
        {
            if (!_eventHandlers.TryGetValue(eventName, out var handlers)) return;
            
            foreach (var handler in handlers)
            {
                if (handler is T action)
                {
                    invokeAction(action);
                }
                else
                {
                    Debug.LogWarning($"Attempt to call event <color=yellow>'{eventName}'</color> with invalid parameter type <color=magenta>{handler?.GetType()}</color>!");
                }
            }
        }

        #endregion

        protected virtual void InternalBind(string eventName, Delegate handler)
        {
            if (!_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers.Add(eventName, new HashSet<Delegate> { handler });
                
                return;
            }
            
            if (_eventHandlers[eventName].Contains(handler)) return;
            
            _eventHandlers[eventName].Add(handler);
        }
    }
}