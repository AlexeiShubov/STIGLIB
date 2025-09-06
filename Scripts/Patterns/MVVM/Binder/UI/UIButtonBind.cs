using UnityEngine;
using UnityEngine.UI;

namespace STIGRADOR.MVVM
{
	[RequireComponent(typeof(Button))]
	public class UIButtonBind : UIBindBase
	{
		private Button _button;
		
		public override void Initialize(ScopeModel scopeModel, ScopeEventManager scopeEventManager)
		{
			_button = GetComponent<Button>();
			
			base.Initialize(scopeModel, scopeEventManager);
		}

		protected override void Subscribe()
		{
			base.Subscribe();
			
			_button.onClick.AddListener(OnClick);
		}
		
		protected override void OnItemEnable(bool status)
		{
			base.OnItemEnable(status);
			
			_button.interactable = status;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			
			_button.onClick.RemoveAllListeners();
		}
	}
}