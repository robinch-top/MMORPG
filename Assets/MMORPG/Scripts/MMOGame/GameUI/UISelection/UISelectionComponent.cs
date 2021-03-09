using System;
using System.Collections.Generic;
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

            // 添加事件
            rc.Get<GameObject>("CreateButton").GetComponent<Button>().onClick.Add(() => CreateBtnOnClick());
            rc.Get<GameObject>("EnterButton").GetComponent<Button>().onClick.Add(() => EnterBtnOnClick());
            rc.Get<GameObject>("DeleteButton").GetComponent<Button>().onClick.Add(() => DeleteBtnOnClick());
            rc.Get<GameObject>("LogoutButton").GetComponent<Button>().onClick.Add(() => LogoutBtnOnClick());

            // 调用加载角色列表请求
            if(Manager.RPG.lastUI != UIState.UICreation)
                GateHelper.GetCharacters().Coroutine();

            Manager.RPG.lastUI = UIState.UISelection;
        }

        public void CreateBtnOnClick()
        {
            Game.EventSystem.Run(EventIdType.CharacterCreation);
        }

        public void EnterBtnOnClick()
        {
            if(Manager.RPG.selection<0){
                // 弹出提示，还没有选择角色
                // ...
                return;
            }

            Game.EventSystem.Run(EventIdType.GameEnterMap);

            // 调用进入世界地图场景请求
            MapHelper.EnterMapAsync().Coroutine();
        }

        
        public void DeleteBtnOnClick()
        {
            // 删除选中角色
            // ...
            //GateHelper.Delete(charaId).Coroutine();
        }

        public void LogoutBtnOnClick()
        {
            // 调用退出登录请求
            GateHelper.Logout().Coroutine();
            Game.EventSystem.Run(EventIdType.GameLogout);
        }
	}
}
