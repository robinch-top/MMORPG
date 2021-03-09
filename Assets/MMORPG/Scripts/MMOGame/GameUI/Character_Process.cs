using UnityEngine.UI;
using Mirror;
using ETModel;
namespace MMOGame
{
	[Event(EventIdType.GameLogin)]
    public class Game_Login : AEvent
    {
        public override void Run()
        {
            Manager.RPG.CameraTo(NetworkState.Login);
            Manager.RPG.state = NetworkState.Login;
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILogin);
        }
    }

    [Event(EventIdType.GameRegister)]
    public class Game_Register : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UIRegister);
        }
    }

    // 登录成功后才到达Selection界面，所以在这里改变网络状态为lobby大厅
    [Event(EventIdType.CharacterSelection)]
    public class Character_Selection : AEvent
    {
        public override void Run()
        {
            Manager.RPG.CameraTo(NetworkState.Lobby);
            Manager.RPG.state = NetworkState.Lobby;
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UISelection);
        }
    }

    [Event(EventIdType.CharacterCreation)]
    public class Character_Creation : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UISelection);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UICreation);
        }
    }

    [Event(EventIdType.CharaCreateFinish)]
    public class CharaCreate_Finish : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UICreation);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UISelection);
        }
    }

    // 退出登录回到登录界面
    [Event(EventIdType.GameLogout)]
    public class Game_Logout : AEvent
    {
        public override void Run()
        {
            Manager.RPG.CameraTo(NetworkState.Login);
            Manager.RPG.state = NetworkState.Offline;
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UISelection);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILogin);
        }
    }

    [Event(EventIdType.GameEnterMap)]
    public class Game_EnterMap : AEvent
    {
        public override void Run()
        {
            // Loading过场
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILoading);
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UISelection);
        }
    }

    // 成功进入世界地图，所以在这里改变网络状态为World
    [Event(EventIdType.EnterMapFinish)]
    public class EnterMap_Finish : AEvent
    {
        public override void Run()
        {
            Manager.RPG.state = NetworkState.World;
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILoading);
        }
    }

    [Event(EventIdType.BackLogin)]
    public class Back_Login : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIRegister);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILogin);
        }
    }

    // 从游戏地图返回大厅，所以在这里改变网络状态为Lobby
    [Event(EventIdType.BackLobby)]
    public class Back_Lobby : AEvent
    {
        public override void Run()
        {
            Manager.RPG.CameraTo(NetworkState.Lobby);
            Manager.RPG.state = NetworkState.Lobby;
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UISelection);
        }
    }
}
