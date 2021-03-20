using ETModel;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace ETHotfix
{
    [ObjectSystem]
    public class RoomComponetAwakeSystem : AwakeSystem<RoomComponent>
    {
        public override void Awake(RoomComponent self)
        {
            self.InitWorldMapRoom().Coroutine();
        }
    }

    public static class RoomComponentSystem
    {
        public static async ETVoid InitWorldMapRoom(this RoomComponent self)
        {
            // 获取所有世界地图map数据列表
            // 这样可以在另一个循环,异步的创建所有的世界地图房间,完成怪物，世界boss,Npc,世界任务,场景中产物的刷新。
            // foreach(MapInfo map in mapInfos){
            //     self.worldMapQueue.Enqueue(map.id);
            // }

            // 这里暂时只初始化 黎明镇的地图房间
            RoomConfig config = GateHelper.GetMapConfig(1001);
            Room daybreak = ComponentFactory.Create<Room,RoomConfig>(config);

            await daybreak.AddComponent<MailBoxComponent>().AddLocation();
            self.mapRooms.Add(daybreak.roomId, daybreak);

            // 本地图刷新
            // await daybreak.RefreshMap().Coroutine();
        }

        /// <summary>
        /// 返回地图房间
        /// </summary>
        public static Room GetMapRoom(this RoomComponent self, long roomId)
        {
            Room room;
            if (!self.mapRooms.TryGetValue(roomId, out room))
            {
                Log.Error("玩家不在已经开始游戏的房间中");
            }
            return room;
        }

        /// <summary>
        /// 返回副本房间
        /// </summary>
        public static Instance GetCopyRoom(this RoomComponent self, long roomId)
        {
            Instance room;
            if (!self.copyRooms.TryGetValue(roomId, out room))
            {
                Log.Error("玩家不在待机的房间中");
            }
            return room;
        }

        
        /// <summary>
        /// 加入房间
        /// </summary>
        public static void JoinRoom(this RoomComponent self, Room room, Gamer gamer)
        {
            if(gamer == null)
            {
                return;
            }

            room.Add(gamer);
            //房间广播
            Log.Info($"玩家{gamer.UserId}进入房间");

            //向Gate发送消息，玩家进入地图房间
            // ...
            // 向玩家客户端发送消息进入游戏地图
            // ...
        }


        /// <summary>
        /// 匹配队列广播
        /// </summary>
        public static void QueueBroadcast(this RoomComponent self,Queue<Gamer> matchingQueue, IActorMessage message)
        {
            foreach (Gamer gamer in matchingQueue)
            {
                //向客户端User发送Actor消息
                ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                ActorMessageSender actorProxy = actorProxyComponent.Get(gamer.CActorId);
 
                Log.Debug("转发给了客户端一条消息，客户端Session：" + gamer.CActorId.ToString());
                actorProxy.Send(message);
            }
        }
    }
}
