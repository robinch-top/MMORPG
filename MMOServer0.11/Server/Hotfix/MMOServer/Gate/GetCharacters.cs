using System;
using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    //客户端请求在当前账号下创建的角色
    [MessageHandler(AppType.Gate)]
    public class GetCharacters : AMRpcHandler<GetCharacters_C2G, GetCharacters_G2C>
    {
        protected override async ETTask Run(Session session, GetCharacters_C2G request, GetCharacters_G2C response, Action reply)
        {
            try
            {
                // 验证Session
                if (!GateHelper.SignSession(session))
                {
                    response.Error = MMOErrorCode.ERR_UserNotOnline;
                    reply();
                    return;
                }

                // 获取用户对象
                User user = session.GetComponent<SessionUserComponent>().User;

                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
                List<Component> characters = await dbProxy.Query2<Character>($"{{UserId:{user.UserId}}}");
                foreach(Character character in characters)
                {
                    response.Characters.Add(GateHelper.CharacterInfoByData(character));
                }

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