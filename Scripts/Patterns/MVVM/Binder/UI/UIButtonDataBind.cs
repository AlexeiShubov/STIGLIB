using UnityEngine;
using UnityEngine.UI;

namespace STIGRADOR.MVVM
{
	[RequireComponent(typeof(Button))]
	public class UIButtonDataBind : BinderMonoBehaviour
	{
		[SerializeField] protected string _eventNameButton;
		[SerializeField] protected string _enableField;
		[SerializeField] protected bool _defaultEnable = true;
		
		protected Button _button;

		public override void Construct(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
		{
			base.Construct(scopeModel, scopeEventManager);
			
			_button = GetComponent<Button>();

			_eventNameButton = _eventNameButton == "" ? $"{gameObject.name}" : _eventNameButton;
			_enableField = _enableField == "" ? $"Btn{_eventNameButton}Enable" : $"Btn{_enableField}Enable";

			Subscribe();
		}

		protected void Subscribe()
		{
			_button.onClick.AddListener(OnClick);
			_Binder.Bind<bool>($"On{_enableField}Changed", OnItemEnable);

			OnItemEnable(_Model.GetBool(_enableField, _defaultEnable));
		}

		protected virtual void OnItemEnable(bool status)
		{
			_button.interactable = status;
		}

		protected virtual void OnClick()
		{
			if (!_button.interactable || !isActiveAndEnabled) return;

			_Invoker.Invoke("OnBtn", _eventNameButton);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			_button.onClick.RemoveAllListeners();
		}
	}
}