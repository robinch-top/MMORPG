using UnityEngine;
using ETModel;
namespace MMOGame
{
    public static class UnitResources
    {
        public static GameObject Get(string type)
        {
	        ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
	          
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset("unit.unity3d", "Unit");
	        GameObject prefab = bundleGameObject.Get<GameObject>($"{type}");

            return prefab;
        }

        public static async ETVoid LoadUnitAsync()
        {
            // 加载Unit资源
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            await resourcesComponent.LoadBundleAsync($"unit.unity3d");

            Game.EventSystem.Run(EventIdType.UnitLoadFinished);

            // resourcesComponent.UnloadBundle("unit.unity3d");
        }

    }
}