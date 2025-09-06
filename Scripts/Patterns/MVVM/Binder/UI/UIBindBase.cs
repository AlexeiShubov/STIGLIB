using UnityEngine;

namespace STIGRADOR.MVVM
{
    public class UIBindBase : BinderMonoBehaviour
    {
        public readonly string _oNClickEventName = "OnClick";
        
        [SerializeField] protected string _eventName;
        [SerializeField] protected string _enableField;
        [SerializeField] protected bool _defaultEnable = true;

        private bool _interactable;

        private string _enableFieldEventName;
        
        protected virtual bool _Interactable => _interactable && isActiveAndEnabled; 
        
        public string EventName => _eventName;
        public string EnableField => _enableField;

        public override void Initialize(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
        {
            base.Initialize(scopeModel, scopeEventManager);

            _interactable = _defaultEnable;
            
            GenerateEventNames();
            Subscribe();
            OnItemEnable(_Model.GetBool(_enableField, _defaultEnable));
        }

        protected virtual void GenerateEventNames()
        {
            _eventName = _eventName == "" ? $"{gameObject.name}" : _eventName;
            _enableField = _enableField == "" ? $"{_eventName}Enable" : $"{_enableField}Enable";
            _enableFieldEventName = $"On{_enableField}Changed";
        }

        protected virtual void Subscribe()
        {
            _Binder.Bind<bool>(_enableFieldEventName, OnItemEnable);

            OnItemEnable(_Model.GetBool(_enableField, _defaultEnable));
        }

        protected virtual void OnClick()
        {
            if (!_Interactable) return;

            _Invoker.Invoke(_oNClickEventName, _eventName);
        }

        protected virtual void OnItemEnable(bool status)
        {
            _interactable = status;
        }
    }
}
