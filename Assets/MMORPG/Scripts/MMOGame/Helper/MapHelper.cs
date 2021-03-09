using System;
using ETModel;
using Mirror;
using UnityEngine;
using Google.Protobuf;
namespace MMOGame
{
    public static class MapHelper
    {
        public static async ETVoid EnterMapAsync()
        {
            try
            {
                // 加载场景资源
                await Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("map.unity3d");
                
                // 切换到map场景
                using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
                {
                    await sceneChangeComponent.ChangeSceneAsync(SceneType.Map);
                }

                // 向服务器请求进入地图场景
                // 进入场景后，再去请求服务器，会从map返回CreateUnits到前端
                // 再创建角色模型，这样不会让加载场景与创建角色模型卡在一起进行
                EnterMap_G2C g2cEntmap = await Net.Gate.Call(new EnterMap_C2G(){CharaId = Manager.RPG.CharaId}) as EnterMap_G2C;

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
                // 切换到选角大厅场景
                using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
                {
                    await sceneChangeComponent.ChangeSceneAsync(SceneType.Lobby);
                }

                // 重设当前角色，移除Unit
                Manager.RPG.ResetPlayer();
                Game.Scene.GetComponent<UnitComponent>().Remove(UnitComponent.Instance.MyUnit.Id);
                
                Game.EventSystem.Run(EventIdType.BackLobby);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }	
        }
    }
}