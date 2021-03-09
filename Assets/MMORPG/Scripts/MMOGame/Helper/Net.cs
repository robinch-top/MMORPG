using UnityEngine;
using System.Collections.Generic;
using ETModel;
using Mirror;
namespace MMOGame
{
    /// <summary>
    /// 获取网络session等辅助方法
    /// </summary>
    public static class Net
    {
        /// 登录后可调用
        public static long UserId{
            get{return GamerComponent.Instance.MyUser.UserId;}
        }
        
        /// 登录后网关请求可调用
        public static Session Gate{
            get{ return SessionComponent.Instance.Session;}
        }

        /// realm请求可调用
        public static Session Realm{
            get{ return Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);}
        }
    }
}