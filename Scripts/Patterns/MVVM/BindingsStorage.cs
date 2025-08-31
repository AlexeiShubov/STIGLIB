using System;
using System.Collections.Generic;
using System.Linq;

namespace STIGRADOR.MVVM
{
    public class BindingsStorage
    {
        private readonly Dictionary<string, Delegate> _bindings;

        public IReadOnlyDictionary<string, Delegate> Bindings => _bindings;

        public BindingsStorage()
        {
            _bindings = new Dictionary<string, Delegate>();
        }
        
        public void WriteEvent<T>(string eventName, T method) where T : Delegate
        {
            if (!_bindings.ContainsKey(eventName))
            {
                _bindings.Add(eventName, method);
            }

            if (DelegateContains(_bindings[eventName], method)) return;

            _bindings[eventName] = Delegate.Combine(_bindings[eventName], method);
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

        public Delegate GetEvents(string eventName)
        {
            return _bindings.TryGetValue(eventName, out var value) ? value : null;
        }

        public void Clear()
        {
            _bindings.Clear();
        }

        private bool DelegateContains(Delegate source, Delegate target)
        {
            if (source == target) return true;
            
            return source.GetInvocationList().Any(d => 
                d.Target == target.Target && 
                d.Method == target.Method);
        }
    }
}
