using UnityEngine;
using ETModel;
namespace MMOGame
{
    public static class UnitFactory
    {
        public static Unit Create(long id, string type = "Skeleton")
        {
            GameObject prefab = UnitResources.Get($"{type}");
	        GameObject go = UnityEngine.Object.Instantiate(prefab);
	        Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(id, go);

            SyncType syncType = Game.Scene.GetComponent<NetSyncComponent>().type;
            if(syncType == SyncType.Frame){
	            unit.AddComponent<FrameMoveComponent>();
            }else if(syncType == SyncType.State){
                unit.AddComponent<MoveComponent>();
                unit.AddComponent<TurnComponent>();
                unit.AddComponent<UnitPathComponent>();
            }
            
			UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            unitComponent.Add(unit);
            return unit;
        }


        public static void Remove(Unit unit){
            SyncType syncType = Game.Scene.GetComponent<NetSyncComponent>().type;
            if(syncType == SyncType.Frame){
	            unit.RemoveComponent<FrameMoveComponent>();
            }else if(syncType == SyncType.State){
                unit.RemoveComponent<MoveComponent>();
                unit.RemoveComponent<TurnComponent>();
                unit.RemoveComponent<UnitPathComponent>();
            }

            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            unitComponent.Remove(unit.Id);
            unit.Dispose();
        }
    }
}