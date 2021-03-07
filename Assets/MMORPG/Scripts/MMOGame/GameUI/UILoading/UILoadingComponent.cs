using UnityEngine;
using UnityEngine.UI;
using ETModel;
namespace MMOGame
{
	[ObjectSystem]
	public class UiLoadingComponentAwakeSystem : AwakeSystem<UILoadingComponent>
	{
		public override void Awake(UILoadingComponent self)
		{
			self.text = self.GetParent<UI>().GameObject.Get<GameObject>("Text").GetComponent<Text>();
		}
	}

	[ObjectSystem]
	public class UiLoadingComponentStartSystem : StartSystem<UILoadingComponent>
	{
		public override void Start(UILoadingComponent self)
		{
			StartAsync(self).Coroutine();
		}
		
		public async ETVoid StartAsync(UILoadingComponent self)
		{
			TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
			long instanceId = self.InstanceId;
			while (true)
			{
				await timerComponent.WaitAsync(500);

				if (self.InstanceId != instanceId)
				{
					return;
				}

				SceneChangeComponent sceneChange = Game.Scene.GetComponent<SceneChangeComponent>();
				if (sceneChange == null)
				{
					continue;
				}
				self.text.text = $"{sceneChange.Process}%";
			}
		}
	}

	public class UILoadingComponent : ETModel.Component
	{
		public Text text;
	}
}
