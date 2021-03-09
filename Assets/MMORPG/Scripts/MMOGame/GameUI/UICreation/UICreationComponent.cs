using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
namespace MMOGame
{
	[ObjectSystem]
	public class UICreationComponentSystem : AwakeSystem<UICreationComponent>
	{
		public override void Awake(UICreationComponent self)
		{
			self.Awake();
		}
	}
	
	public class UICreationComponent: ETModel.Component
	{
        public InputField nickname;
        public Dropdown classDropdown;
        
        public void Awake()
        {
            
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            nickname = rc.Get<GameObject>("Nickname").GetComponent<InputField>();
            classDropdown = rc.Get<GameObject>("ClassDropdown").GetComponent<Dropdown>();
             
            //生成classDropdown选项
            classDropdown.options = Manager.RPG.playerClasses.Select(
                        p => new Dropdown.OptionData(p.CNName)
                    ).ToList();

            //添加事件
            rc.Get<GameObject>("ConfirmButton").GetComponent<Button>().onClick.Add(() => ConfirmBtnOnClick());
            rc.Get<GameObject>("CancelButton").GetComponent<Button>().onClick.Add(() => CancelBtnOnClick());

            Manager.RPG.lastUI = UIState.UICreation;
        }

        public void ConfirmBtnOnClick()
        {
            string roleclass = Manager.RPG.playerClasses[classDropdown.value].name;

            if(nickname.text == ""){
                // 弹出提示，角色昵称不能为空
                // ...
                return;
            }

            // 调用创建角色请求
            GateHelper.CreateCharacter(nickname.text,roleclass,Manager.RPG.AvailableLocation()).Coroutine();
        }

        public void CancelBtnOnClick()
        {
            // 这种取消没有任何其它逻辑，就不用调用流程事件了
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UICreation);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UISelection);
        }

	}
}
