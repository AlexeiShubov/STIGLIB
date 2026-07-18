using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace STIGRADOR.MVVM
{
    public class UIBindBase : BinderMonoBehaviour
    {
        private const string _onClickEventName = "OnClick";

        [SerializeField] protected string _eventName;
        [SerializeField] protected string _enableField;
        [SerializeField] protected bool _defaultEnable = true;

        private bool _interactable;

        private string _enableFieldEventName;
        private string _widgetClickEventName;

        protected virtual bool _Interactable => _interactable && isActiveAndEnabled;

        public string EventName => _eventName;
        public string EnableField => _enableField;
        public string OnClickEventName => _onClickEventName;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private static readonly ConditionalWeakTable<ScopeEventManager, Dictionary<string, GameObject>> _eventNameOwners = new ConditionalWeakTable<ScopeEventManager, Dictionary<string, GameObject>>();
#endif

        public override void Initialize(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
        {
            base.Initialize(scopeModel, scopeEventManager);

            _interactable = _defaultEnable;

            GenerateEventNames();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            CheckEventNameCollision(scopeEventManager);
#endif
            Subscribe();
        }

        protected virtual void GenerateEventNames()
        {
            _eventName = _eventName == "" ? $"{gameObject.name}" : _eventName;
            _enableField = _enableField == "" ? $"{_eventName}Enable" : $"{_enableField}Enable";
            _enableFieldEventName = $"On{_enableField}Changed";
            _widgetClickEventName = $"On{_eventName}Click";
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private void CheckEventNameCollision(ScopeEventManager scopeEventManager)
        {
            var owners = _eventNameOwners.GetOrCreateValue(scopeEventManager);

            if (owners.TryGetValue(_enableFieldEventName, out var existingOwner) && existingOwner != null && existingOwner != gameObject)
            {
                Debug.LogError($"[UIBindBase] Event name collision on <color=yellow>'{_enableFieldEventName}'</color>: both <color=magenta>'{existingOwner.name}'</color> and <color=magenta>'{gameObject.name}'</color> resolve to the same enable-field event in this scope and will cross-talk. Give them distinct _eventName/_enableField values.", gameObject);

                return;
            }

            owners[_enableFieldEventName] = gameObject;
        }
#endif

        protected virtual void Subscribe()
        {
            _Binder.Bind<bool>(_enableFieldEventName, OnItemEnable);

            OnItemEnable(_Model.GetBool(_enableField, _defaultEnable));
        }

        protected virtual void OnClick()
        {
            if (!_Interactable) return;

            _Invoker.Invoke(_onClickEventName, _eventName);
            _Invoker.Invoke(_widgetClickEventName);
        }

        protected virtual void OnItemEnable(bool status)
        {
            _interactable = status;
        }
    }
}
