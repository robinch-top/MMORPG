using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
namespace MMOGame
{
	[ObjectSystem]
	public class UIRegisterComponentSystem : AwakeSystem<UIRegisterComponent>
	{
		public override void Awake(UIRegisterComponent self)
		{
			self.Awake();
		}
	}

	public class UIRegisterComponent : ETModel.Component
	{
		//提示文本
		public Text prompt;

		public InputField account;
		public InputField password;
		public InputField rePassword;
		public void Awake()
		{
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

			//初始化数据
			account = rc.Get<GameObject>("Account").GetComponent<InputField>();
			password = rc.Get<GameObject>("Password").GetComponent<InputField>();
			rePassword = rc.Get<GameObject>("RePassword").GetComponent<InputField>();
			prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();


			//添加事件
			rc.Get<GameObject>("CancelButton").GetComponent<Button>().onClick.Add(() => CancelBtnOnClick());
			rc.Get<GameObject>("SubmitButton").GetComponent<Button>().onClick.Add(() => SubmitBtnOnClick());
		}
		public void SubmitBtnOnClick()
		{
			if (password != rePassword)
				prompt.text = "两次输入的密码不一致！";
			//LandHelper.Register(this.account.text, this.password.text).Coroutine();
		}

		public void CancelBtnOnClick()
		{
			Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIRegister);
			Game.Scene.GetComponent<UIComponent>().Create(UIType.UILogin);
		}
	}
}