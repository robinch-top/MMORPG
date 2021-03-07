using System;
using ETModel;
using Mirror;
namespace MMOGame
{
    public static class MapHelper
    {
        public static async ETVoid EnterMapAsync()
        {
            try
            {
                // 加载Unit资源
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"unit.unity3d");

                // 加载场景资源
                await Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("map.unity3d");


                // 切换到map场景
                using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
                {
                    await sceneChangeComponent.ChangeSceneAsync(SceneType.Map);
                }

                // G2C_EnterMap g2CEnterMap = await SessionComponent.Instance.Session.Call(new C2G_EnterMap()) as G2C_EnterMap;
                
                Unit unit = UnitFactory.Create(2314546,"Warrior");
                Manager.RPG.LoadCharacterToScene(unit.GameObject);

                Game.EventSystem.Run(EventIdType.EnterMapFinish);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }	
        }

        public static async ETVoid Back2LobbyAsync()
        {
            try
            {
                // 切换到map场景
                using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
                {
                    await sceneChangeComponent.ChangeSceneAsync(SceneType.Lobby);
                }

                Manager.RPG.ResetPlayer();
                Game.Scene.GetComponent<UnitComponent>().Remove(2314546);
                
                Game.EventSystem.Run(EventIdType.BackLobby);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }	
        }
    }
}