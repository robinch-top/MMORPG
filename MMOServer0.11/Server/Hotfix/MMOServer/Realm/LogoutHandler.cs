using System;
using System.Net;
using ETModel;
using System.Collections.Generic;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class LogoutHandler : AMHandler<Logout_C2R>
    {
        protected override async ETTask Run(Session session, Logout_C2R message)
        {
            //玩家退出登录
            await RealmHelper.KickOutPlayer(message.UserId,false);
        }
    }
}