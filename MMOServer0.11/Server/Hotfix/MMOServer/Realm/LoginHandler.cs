using System;
using System.Net;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class LoginHandler : AMRpcHandler<Login_C2R, Login_R2C>
    {
        protected override async ETTask Run(Session session, Login_C2R request,Login_R2C response,Action reply)
        {
            try
            {
                DBProxyComponent dbProxy = Game.Scene.GetComponent<DBProxyComponent>();
                //有多种操作数据库的方式 Json字符串模式可以提供多个条件/ID查找模式可以以Entity.Id查找：
                //UserInfo userInfo = await dbProxy.Query<UserInfo>(gamer.UserID, false);
                //先声明一个数据库操作Entity对象AccountInfo

                //验证请求的账号和密码
                List<ComponentWithId> result = await dbProxy.Query<AccountInfo>($"{{Account:'{request.Account}',Password:'{request.Password}'}}");

                if (result.Count != 1)
                {
                    response.Error = ErrorCode.ERR_AccountOrPasswordError;
                    reply();
                    return;
                }

                //已验证通过，可能存在其它地方有登录，要先踢下线
                AccountInfo account = (AccountInfo)result[0];
                await RealmHelper.KickOutPlayer(account.Id);

                int GateAppId;
                StartConfig config;
                //获取账号所在区服的AppId 索取登陆Key
                if (StartConfigComponent.Instance.GateConfigs.Count ==1)
                { //只有一个Gate服务器时当作AllServer配置处理
                    config = StartConfigComponent.Instance.StartConfig;
                }
                else
                { //有多个Gate服务器时当作分布式配置处理
                    GateAppId = RealmHelper.GetGateAppIdFromUserId(account.Id);
                    config = StartConfigComponent.Instance.GateConfigs[GateAppId - 1];
                }
                IPEndPoint innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;
                Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);
                GetLoginKey_G2R g2RGetLoginKey = (GetLoginKey_G2R)await gateSession.Call(new GetLoginKey_R2G() { UserId = account.Id });

                // *** 分配网关地址 *** //
                //如果有多台网关服务器，那就应该在realm上添加GateManagerComponent
                //可以管理所有在线的网关服务器，接收网关的负载状态，前端也可以向realm获取网关的负载状态
                //可以根据网关服务器的负载分配网关地址给客户端，也可以随机分配，也可以指定分配
                string outerAddress = config.GetComponent<OuterConfig>().Address2;

                response.GateAddress = outerAddress;
                response.GateLoginKey = g2RGetLoginKey.GateLoginKey;
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }
        }
    }
}