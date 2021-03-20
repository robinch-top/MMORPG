using ETModel;
using System;

namespace ETHotfix
{
    [MessageHandler(AppType.Gate)]
    public class GetLoginKey : AMRpcHandler<GetLoginKey_R2G, GetLoginKey_G2R>
    {
        protected override async ETTask Run(Session session, GetLoginKey_R2G request, GetLoginKey_G2R response,Action reply)
        {
            try
            {
                long key = RandomHelper.RandInt64();
                Game.Scene.GetComponent<SessionKeyComponent>().Add(key, request.UserId);
                response.GateLoginKey = key;
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