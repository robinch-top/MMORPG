using System;
using System.Net;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class RegisterHandler : AMRpcHandler<Register_C2R, Register_R2C>
    {
        protected override async ETTask Run(Session session, Register_C2R request,Register_R2C response, Action reply)
        {
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();

                //验证假定的账号和密码
                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}'}}");
                if (result.Count >= 1)
                {

                    response.Error = MMOErrorCode.ERR_AccountAlreadyRegisted;
                    reply();
                    return;
                }

                // 前端请求有分区号,生成指定分区账号ID,否则生成随机分区账号ID
                AccountInfo newAccount;
                if (request.Partition > 0)
                   newAccount = ComponentFactory.CreateWithId<AccountInfo>(RealmHelper.GenerateId(request.Partition));
                else newAccount = ComponentFactory.CreateWithId<AccountInfo>(RealmHelper.GenerateId());;
                
                newAccount.Account = request.Account;
                newAccount.Password = request.Password;
                await dbProxy.Save(newAccount);

                // 生成玩家的用户信息
                UserInfo newUser = ComponentFactory.CreateWithId<UserInfo, string>(newAccount.Id, request.Account);
                await dbProxy.Save(newUser);

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