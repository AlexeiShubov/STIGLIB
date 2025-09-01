using UnityEngine;
using UnityEngine.EventSystems;

namespace STIGRADOR.MVVM
{
    public class UIClickBind : BinderMonoBehaviour
    {
        [SerializeField] protected string _enableField;
        [SerializeField] protected bool _defaultEnable = true;

        protected bool _interactable;
        protected EventTrigger _eventTrigger;
        
        protected virtual void Awake()
        {
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            
            _interactable = _defaultEnable;
            _enableField = _enableField == "" ? $"{gameObject.name}Enable" : _enableField;
            _eventTrigger = gameObject.AddComponent<EventTrigger>();

            entry.callback.AddListener(OnClick);
            _eventTrigger.triggers.Add(entry);
        }

        protected virtual void Start()
        {
            _Binder.Bind<bool>($"On{_enableField}Changed", OnItemEnable);

            OnItemEnable(_Model.GetBool(_enableField, _defaultEnable));
        }

        protected virtual void OnItemEnable(bool status)
        {
            _interactable = status;
        }

        protected virtual void OnClick(BaseEventData data)
        {
            OnClick();
        }

        protected virtual void OnClick()
        {
            if (!_interactable || !isActiveAndEnabled) return;

            _Invoker.Invoke("OnBtn", _enableField);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_eventTrigger == null) return;
            
            _eventTrigger.triggers.ForEach(t => t.callback.RemoveAllListeners());
            _eventTrigger.triggers.Clear();
        }
    }
}