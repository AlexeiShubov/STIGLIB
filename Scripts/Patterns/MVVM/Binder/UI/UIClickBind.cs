using UnityEngine;
using UnityEngine.EventSystems;

namespace STIGRADOR.MVVM
{
    [RequireComponent(typeof(EventTrigger))]
    public class UIClickBind : UIBindBase
    {
        private EventTrigger _eventTrigger;
        private EventTrigger.Entry _entry;

        public override void Initialize(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
        {
            _eventTrigger = GetComponent<EventTrigger>();

            base.Initialize(scopeModel, scopeEventManager);
        }

        protected override void Subscribe()
        {
            base.Subscribe();

            _entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };

            _entry.callback.AddListener(OnClickEventTrigger);
            _eventTrigger.triggers.Add(_entry);
        }

        protected virtual void OnClickEventTrigger(BaseEventData data)
        {
            OnClick();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_entry != null)
            {
                _entry.callback.RemoveListener(OnClickEventTrigger);
                _eventTrigger.triggers.Remove(_entry);
            }
        }
    }
}