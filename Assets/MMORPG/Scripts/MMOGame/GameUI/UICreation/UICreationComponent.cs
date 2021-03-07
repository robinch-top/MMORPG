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
        }

        public void ConfirmBtnOnClick()
        {
            string roleclass = Manager.RPG.playerClasses[classDropdown.value].name;

            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UICreation);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UISelection);
            Log.Info(roleclass);
        }

        public void CancelBtnOnClick()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UICreation);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UISelection);
        }

	}
}
