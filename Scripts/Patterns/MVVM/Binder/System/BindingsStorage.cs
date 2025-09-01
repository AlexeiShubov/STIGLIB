using System;
using System.Collections.Generic;
using System.Linq;

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
        
        public void WriteEvent(string eventName, Delegate handler)
        {
            if (!_bindings.ContainsKey(eventName))
            {
                _bindings.Add(eventName, handler);
            }

            if (DelegateContains(_bindings[eventName], handler)) return;

            _bindings[eventName] = Delegate.Combine(_bindings[eventName], handler);
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
