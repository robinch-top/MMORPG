using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Gate)]
    public class M2G_EnterMapSucessHandler : AMActorHandler<User, Actor_EnterMapSucess_M2G>
    {
        protected override async ETTask Run(User user, Actor_EnterMapSucess_M2G message)
        {
            // gate更新ActorID,UnitId
            user.ActorId = message.GamerId;
            user.UnitId = message.UnitId;
            Log.Info($"玩家{user.UserId}匹配成功 更新客户端Actor转发向Gamer");

            await ETTask.CompletedTask;
        }
    }
}
