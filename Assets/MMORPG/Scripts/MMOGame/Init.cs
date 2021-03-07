using System;
using System.Threading;
using UnityEngine;
using ETModel;
using Mirror;
namespace MMOGame{
	public class Init : MonoBehaviour
	{
		public void Awake(){

			// 返回选角大厅场景时，删除重复的Global物体
			if(Manager.RPG && Manager.RPG.state == NetworkState.World)
				Destroy(this.gameObject);
		}

		public void Start()
        {
			// 取得管理组件
			Manager.RPG = RPGManager.singleton;
			
			this.StartAsync().Coroutine();
        }
		
		private async ETVoid StartAsync()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

				DontDestroyOnLoad(gameObject);
				ClientConfigHelper.SetConfigHelper();
				Game.EventSystem.Add(DLLType.Core, typeof(Core).Assembly);
				Game.EventSystem.Add(DLLType.Model, typeof(Model).Assembly);
				Game.EventSystem.Add(DLLType.MMO, typeof(Init).Assembly);
				//Log.Info(typeof(Core).Assembly.ToString());

				Game.Scene.AddComponent<TimerComponent>();
				Game.Scene.AddComponent<GlobalConfigComponent>();
				Game.Scene.AddComponent<NetOuterComponent>();
				Game.Scene.AddComponent<ResourcesComponent>();
				Game.Scene.AddComponent<PlayerComponent>();
				Game.Scene.AddComponent<UnitComponent>();
				Game.Scene.AddComponent<UIComponent>();

				// 下载ab包
				await BundleHelper.DownloadBundle();

				// 加载配置
				Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				Game.Scene.AddComponent<ConfigComponent>();
				Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
				
				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatcherComponent>();

				UnitConfig unitConfig = (UnitConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(UnitConfig), 1001);
				Log.Debug($"config {JsonHelper.ToJson(unitConfig)}");

				// 网络同步方式组件
				Game.Scene.AddComponent<NetSyncComponent,SyncType>(SyncType.State);

				Game.EventSystem.Run(EventIdType.InitStart);

				//Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			OneThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}