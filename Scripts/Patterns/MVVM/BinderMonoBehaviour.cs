using System;
using UnityEngine;

namespace STIGRADOR.MVVM
{
    public abstract class BinderMonoBehaviour : MonoBehaviour
    {
        protected readonly BindingsStorage _bindingsStorage = new BindingsStorage();
        
        protected ScopeModel ScopeModel;
        protected EventManager _eventManager;
        
        public void Construct(ScopeModel scopeModel, EventManager eventManager)
        {
            ScopeModel = scopeModel;
            _eventManager = eventManager;
        }

        protected void AutoBind(string eventName, Action method)
        {
            _eventManager.AutoBind(eventName, method);
            _bindingsStorage.WriteEvent(eventName, method);
        } 
        
        protected void AutoBind<A>(string eventName, Action<A> method)
        {
            _eventManager.AutoBind(eventName, method);
            _bindingsStorage.WriteEvent(eventName, method);
        } 
        
        
        protected void AutoBind<A, B>(string eventName, Action<A, B> method)        
        {
            _eventManager.AutoBind(eventName, method);
            _bindingsStorage.WriteEvent(eventName, method);
        } 
        
        protected void AutoBind<A, B, C>(string eventName, Action<A, B, C> method)
        {
            _eventManager.AutoBind(eventName, method);
            _bindingsStorage.WriteEvent(eventName, method);
        } 
        
        protected virtual void AutoUnsubscribe()
        {
            foreach (var pair in _bindingsStorage.Bindings)
            {
                var handlers = pair.Value?.GetInvocationList() ?? Array.Empty<Delegate>();
                
                foreach (var handler in handlers)
                {
                    _eventManager.UnBind(pair.Key, handler);
                }
            }
            
            _bindingsStorage.Clear();
        }

        protected virtual void OnDestroy()
        {
            AutoUnsubscribe();
        }
    }
}
