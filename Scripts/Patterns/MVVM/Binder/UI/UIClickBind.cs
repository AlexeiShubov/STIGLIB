using UnityEngine;
using UnityEngine.EventSystems;

namespace STIGRADOR.MVVM
{
    [RequireComponent(typeof(EventTrigger))]
    public class UIClickBind : UIBindBase
    {
        private EventTrigger _eventTrigger;
        
        public override void Initialize(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
        {
            _eventTrigger = GetComponent<EventTrigger>();
            
            base.Initialize(scopeModel, scopeEventManager);
        }
        
        protected override void Subscribe()
        {
            base.Subscribe();
            
            var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            
            entry.callback.AddListener(OnClickEventTrigger);
            _eventTrigger.triggers.Add(entry);
        }

        protected virtual void OnClickEventTrigger(BaseEventData data)
        {
            OnClick();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _eventTrigger.triggers.ForEach(t => t.callback.RemoveAllListeners());
            _eventTrigger.triggers.Clear();
        }
    }
}