using System;
using System.Net;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class EnterMapHandler : AMRpcHandler<EnterMap_C2G, EnterMap_G2C>
    {
        protected override async ETTask Run(Session session, EnterMap_C2G request,EnterMap_G2C response,Action reply)
        {
            try
            {
				User user = session.GetComponent<SessionUserComponent>().User;
				
				// 在map服务器上创建战斗Unit
				IPEndPoint mapAddress = StartConfigComponent.Instance.MapConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
				Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapAddress);

				mapSession.Send(new CreateUnit_G2M() { 
					UserId = user.UserId, 
					CharaId = request.CharaId,
					GActorId = user.InstanceId,
					CActorId = user.GateSessionId
				});

				// 回复客户端
                response.Frame = Game.Scene.GetComponent<UnitStateMgrComponent>().currFrame;
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