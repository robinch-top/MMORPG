using ETModel;
using System.Linq;
using System.Collections.Generic;
namespace ETHotfix
{
    public static class RoomSystem
    {
        /// <summary>
        /// 斗地主游戏开始
        /// </summary>
        public static void InitRoom(this Room self)
        {
            // 添加一些组件
            // ...
        }

        /// <summary>
        /// 添加gamer
        /// </summary>
        /// <param name="gamer"></param>
        public static void Add(this Room self, Gamer gamer)
        {
            //玩家需要获取一个座位坐下
            if (!self.gamers.TryGetValue(gamer.UserId, out Gamer ga))
            {
                self.gamers.Add(gamer.UserId,gamer);
            }
            else
            {
                Log.Error("玩家已存在");
            }
        }
        
        /// <summary>
        /// 获取玩家座位索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Gamer GetGamerByUserId(this Room self, long id)
        {
            if (self.gamers.TryGetValue(id, out Gamer gamer))
            {
                return gamer;
            }

            return null;
        }

        public static Gamer[] GetGamers(this Room self)
        {
            return self.gamers.Values.ToArray();
        }

        /// <summary>
        /// 移除玩家并返回 玩家离开房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void Remove(this Room self, long id)
        {
            Log.Info($"将从id{id}玩家移除房间");
            if (self.gamers.TryGetValue(id, out Gamer gamer))
            {
                self.gamers.Remove(gamer.UserId);
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="message"></param>
        public static void Broadcast(this Room self, IActorMessage message)
        {
            foreach (KeyValuePair<long,Gamer> kp in self.gamers)
            {
                Gamer gamer = kp.Value;
                //如果玩家不存在或者不在线
                if (gamer == null || gamer.isOffline)
                {
                    continue;
                }
                //向客户端User发送Actor消息
                ActorMessageSenderComponent actorProxyComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
                ActorMessageSender actorProxy = actorProxyComponent.Get(gamer.CActorId);
                actorProxy.Send(message);
            }
        }

        public static void ClearRoom(this Room self){
            // RoomComponent roomComponet = Game.Scene.GetComponent<RoomComponent>();
            for(int i=0;i<self.gamers.Count;i++)
            {
                Gamer gamer = self.gamers[i];
                // roomComponet.rooms.Remove(rooms.roomId);
                gamer.Dispose();
                self.Dispose();
            }
        }
    }
}
