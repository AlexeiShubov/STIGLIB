using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace STIGRADOR.MVVM
{
    public class BindingsStorage : IDisposable
    {
        private readonly Dictionary<string, Delegate> _bindings;

        public IReadOnlyDictionary<string, Delegate> Bindings => _bindings;

        public BindingsStorage()
        {
            _bindings = new Dictionary<string, Delegate>();
        }

        public bool WriteEvent(string eventName, Delegate handler)
        {
            if (!_bindings.TryGetValue(eventName, out var existing))
            {
                _bindings.Add(eventName, handler);

                return true;
            }

            if (existing != null && existing.GetType() != handler.GetType())
            {
                Debug.LogError($"[Binder] Cannot bind <color=yellow>'{eventName}'</color>: already bound with delegate type <color=magenta>{existing.GetType().Name}</color>, attempted to bind <color=magenta>{handler.GetType().Name}</color>. New binding ignored.");

                return false;
            }

            if (DelegateContains(existing, handler)) return true;

            _bindings[eventName] = Delegate.Combine(existing, handler);

            return true;
        }

        public void RemoveEvent(string eventName, Delegate handler)
        {
            if (!_bindings.TryGetValue(eventName, out var value)) return;
            
            var newHandler = Delegate.Remove(value, handler);
                
            if (newHandler is null)
            {
                _bindings.Remove(eventName);
                    
                return;
            }

            _bindings[eventName] = newHandler;
        }

        private bool DelegateContains(Delegate source, Delegate handler)
        {
            if (source == handler) return true;
            
            return source
                .GetInvocationList()
                .Any(d => 
                d.Target == handler.Target && 
                d.Method == handler.Method);
        }

        public void Dispose()
        {
            _bindings.Clear();
        }
    }
}
