using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
namespace MMOGame
{
	[ObjectSystem]
	public class UISelectionComponentSystem : AwakeSystem<UISelectionComponent>
	{
		public override void Awake(UISelectionComponent self)
		{
			self.Awake();
		}
	}
	
	public class UISelectionComponent: ETModel.Component
	{
        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            //添加事件
            rc.Get<GameObject>("CreateButton").GetComponent<Button>().onClick.Add(() => CreateBtnOnClick());
            rc.Get<GameObject>("EnterButton").GetComponent<Button>().onClick.Add(() => EnterBtnOnClick());
            rc.Get<GameObject>("DeleteButton").GetComponent<Button>().onClick.Add(() => DeleteBtnOnClick());
            rc.Get<GameObject>("LogoutButton").GetComponent<Button>().onClick.Add(() => LogoutBtnOnClick());
        }

        public void CreateBtnOnClick()
        {
            Game.EventSystem.Run(EventIdType.CharacterCreation);
        }

        public void EnterBtnOnClick()
        {
            Game.EventSystem.Run(EventIdType.GameEnterMap);

            MapHelper.EnterMapAsync().Coroutine();
        }

        
        public void DeleteBtnOnClick()
        {
            //LandHelper.Register(this.account.text, this.password.text).Coroutine();
        }

        public void LogoutBtnOnClick()
        {
            Game.EventSystem.Run(EventIdType.GameLogout);
        }
	}
}
