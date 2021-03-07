﻿using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace MMOGame
{
	[ObjectSystem]
	public class UiLoginComponentSystem : AwakeSystem<UILoginComponent>
	{
		public override void Awake(UILoginComponent self)
		{
			self.Awake();
		}
	}

	public class UILoginComponent : ETModel.Component
    {
		//提示文本
		public Text prompt;

		public InputField account;
		public InputField password;

		//是否正在登录中（避免登录请求还没响应时连续点击登录）
		public bool isLogining;
		//是否正在注册中（避免登录请求还没响应时连续点击注册）
		public bool isRegistering;
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            // 设置主摄像机的位置
            Manager.MMO.CameraTo("Login");

            // 初始化数据
            account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            prompt = rc.Get<GameObject>("Prompt").GetComponent<Text>();
            this.isLogining = false;
            this.isRegistering = false;

            // 添加事件
            rc.Get<GameObject>("LoginButton").GetComponent<Button>().onClick.Add(() => LoginBtnOnClick());
            rc.Get<GameObject>("RegisterButton").GetComponent<Button>().onClick.Add(() => RegisterBtnOnClick());
        }

        public void LoginBtnOnClick()
        {
            if (this.isLogining || this.IsDisposed)
            {
                return;
            }
            this.isLogining = true;
            Game.EventSystem.Run(EventIdType.GameSelection);
        }
        public void RegisterBtnOnClick()
        {
            if (this.isRegistering || this.IsDisposed)
            {
                return;
            }
            this.isRegistering = true;
            Game.EventSystem.Run(EventIdType.GameRegister);
        }
    }
}
