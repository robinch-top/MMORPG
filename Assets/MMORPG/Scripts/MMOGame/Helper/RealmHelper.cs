using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using ETModel;
namespace MMOGame
{
    public static class RealmHelper
    {
        public static async ETVoid Login(string account, string password)
        {
            UILoginComponent login = Game.Scene.GetComponent<UIComponent>().Get(UIType.UILogin).GetComponent<UILoginComponent>();

            // 请求realm验证
            Login_R2C messageRealm = (Login_R2C)await Net.Realm.Call(new Login_C2R() { Account = account, Password = password });
            Net.Realm.Dispose();
            login.prompt.text = "正在登录中...";

            // 判断realm验证结果
            if (messageRealm.Error == ErrorCode.ERR_AccountOrPasswordError)
            {
                login.prompt.text = "登录失败,账号或密码错误";
                login.account.text = "";
                login.password.text = "";
                login.isLogining = false;
                return;
            }

            // 创建与保存网关session
            Session gate = Game.Scene.GetComponent<NetOuterComponent>().Create(messageRealm.GateAddress);
            if (SessionComponent.Instance == null)
                Game.Scene.AddComponent<SessionComponent>().Session = gate;
            else
                SessionComponent.Instance.Session = gate;
            
            // 请求登录网关
            LoginGate_G2C messageGate = (LoginGate_G2C)await gate.Call(new LoginGate_C2G() { GateLoginKey = messageRealm.GateLoginKey });
            
            // 判断登陆网关结果
            if (messageGate.Error == ErrorCode.ERR_ConnectGateKeyError)
            {
                login.prompt.text = "连接网关服务器超时";
                login.account.text = "";
                login.password.text = "";
                gate.Dispose();
                login.isLogining = false;
                return;
            }
            
            // 登录网关成功
            login.prompt.text = "登陆成功";
        
            // 保存登录user
            User user = ComponentFactory.Create<User, long>(messageGate.UserId);
            GamerComponent.Instance.MyUser = user;

            //加载选角大厅界面
            Game.EventSystem.Run(EventIdType.CharacterSelection);
        }

        public static async ETVoid Register(string account, string password)
        {
            Register_R2C message = (Register_R2C)await Net.Realm.Call(new Register_C2R() { Account = account, Password = password });
            Net.Realm.Dispose();

            UIRegisterComponent reg = Game.Scene.GetComponent<UIComponent>().Get(UIType.UIRegister).GetComponent<UIRegisterComponent>();
            reg.isRegistering = false;

            if (message.Error == MMOErrorCode.ERR_AccountAlreadyRegisted)
            {
                reg.prompt.text = "注册失败，账号已被注册";
                reg.account.text = "";
                reg.password.text = "";
                return;
            }
            
            reg.prompt.text = "注册成功！";

            // 返回登录界面
            Game.EventSystem.Run(EventIdType.BackLogin);
        }

    }
}