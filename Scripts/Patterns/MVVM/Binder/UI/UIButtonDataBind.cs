using UnityEngine;
using UnityEngine.UI;

namespace STIGRADOR.MVVM
{
	[RequireComponent(typeof(Button))]
	public class UIButtonDataBind : BinderMonoBehaviour
	{
		[SerializeField] protected string _enableField;
		[SerializeField] protected bool _defaultEnable = true;
		
		protected Button _button;

		protected virtual void Awake()
		{
			_button = GetComponent<Button>();

			_enableField = _enableField == "" ? $"Btn{gameObject.name}Enable" : _enableField;
			_button.onClick.AddListener(OnClick);
		}

		protected virtual void Start()
		{
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

			_Invoker.Invoke("OnBtn", _enableField);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			_button.onClick.RemoveAllListeners();
		}
	}
}