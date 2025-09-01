using System;

namespace STIGRADOR.MVVM
{
    public class Binder : IDisposable
    {
        private readonly BindingsStorage _bindingsStorage = new BindingsStorage();
        private readonly BaseEventManager _eventManager;
        
        public Binder(BaseEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public void Bind(string eventName, Action handler)
        {
            _eventManager.Bind(eventName, handler);
            _bindingsStorage.WriteEvent(eventName, handler);
        } 
        
        public void Bind<A>(string eventName, Action<A> handler)
        {
            _eventManager.Bind(eventName, handler);
            _bindingsStorage.WriteEvent(eventName, handler);
        } 
        
        public void Bind<A, B>(string eventName, Action<A, B> handler)        
        {
            _eventManager.Bind(eventName, handler);
            _bindingsStorage.WriteEvent(eventName, handler);
        } 
        
        public void Bind<A, B, C>(string eventName, Action<A, B, C> handler)
        {
            _eventManager.Bind(eventName, handler);
            _bindingsStorage.WriteEvent(eventName, handler);
        }

        public void UnBind(string eventName, Action handler)
        {
            _eventManager.UnBind(eventName, handler);
            _bindingsStorage.RemoveEvent(eventName, handler);
        }
        
        public void UnBind<A>(string eventName, Action<A> handler)
        {
            _eventManager.UnBind(eventName, handler);
            _bindingsStorage.RemoveEvent(eventName, handler);
        }
        
        public void UnBind<A, B>(string eventName, Action<A, B> handler)
        {
            _eventManager.UnBind(eventName, handler);
            _bindingsStorage.RemoveEvent(eventName, handler);
        }
        
        public void UnBind<A, B, C>(string eventName, Action<A, B, C> handler)
        {
            _eventManager.UnBind(eventName, handler);
            _bindingsStorage.RemoveEvent(eventName, handler);
        }

        private void UnBindAll()
        {
            foreach (var pair in _bindingsStorage.Bindings)
            {
                var handlers = pair.Value?.GetInvocationList() ?? Array.Empty<Delegate>();
                
                foreach (var handler in handlers)
                {
                    _eventManager.UnBind(pair.Key, handler);
                }
            }
            
            _bindingsStorage.Dispose();
        }

        public void Dispose()
        {
            UnBindAll();
        }
    }
}