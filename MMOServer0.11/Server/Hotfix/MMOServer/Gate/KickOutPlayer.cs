using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class KickOutPlayer : AMRpcHandler<KickOutPlayer_R2G, KickOutPlayer_G2R>
    {
        protected override async ETTask Run(Session session, KickOutPlayer_R2G request, KickOutPlayer_G2R response,Action reply)
        {
            try
            {
                //获取此UserID的网关session
                User user = Game.Scene.GetComponent<UserComponent>().Get(request.UserId);
                Session lastSession = Game.Scene.GetComponent<NetOuterComponent>().Get(user.GateSessionId);
                
                //移除session与user的绑定
                lastSession.RemoveComponent<SessionUserComponent>();

                reply();

                await ETTask.CompletedTask;
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }

        }
    }
}
