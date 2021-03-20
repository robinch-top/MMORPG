using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class PlayerOnline : AMHandler<PlayerOnline_G2R>
    {
        protected override async ETTask Run(Session session, PlayerOnline_G2R message)
        {
            OnlineComponent onlineComponent = Game.Scene.GetComponent<OnlineComponent>();

            //检查玩家是否在线 如不在线则添加
            if (onlineComponent.GetGateAppId(message.UserId) == 0)
            {
                onlineComponent.Add(message.UserId, message.GateAppId);
            }
            else
            {
                Log.Error("玩家已在线 Realm服务器收到重复上线请求的异常");
            }

            await ETTask.CompletedTask;
        }
    }
}
