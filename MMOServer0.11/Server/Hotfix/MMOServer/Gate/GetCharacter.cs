using System;
using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    //客户端请求在当前账号下创建的角色
    [MessageHandler(AppType.Gate)]
    public class GetCharacter : AMRpcHandler<GetCharacter_C2G, CharacterMessage_G2C>
    {
        protected override async ETTask Run(Session session, GetCharacter_C2G request, CharacterMessage_G2C response, Action reply)
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
                List<Component> characters = await dbProxy.Query2<Character>($"{{UserId:{request.UserId},CharaId:{request.CharaId}}}");
                Character character = characters[0] as Character;
                
                response.Character = GateHelper.CharacterInfoByData(character);
                

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