using UnityEngine.UI;
using Mirror;
using ETModel;
namespace MMOGame
{
	[Event(EventIdType.InitStart)]
    public class InitStart_CreateLogin : AEvent
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

    [Event(EventIdType.GameSelection)]
    public class Game_Selection : AEvent
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

    [Event(EventIdType.GameLogout)]
    public class Game_Logout : AEvent
    {
        public override void Run()
        {
            Manager.RPG.CameraTo(NetworkState.Login);
            Manager.RPG.state = NetworkState.Login;
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UISelection);
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILogin);
        }
    }

    [Event(EventIdType.GameEnterMap)]
    public class Game_Enter : AEvent
    {
        public override void Run()
        {
            // Loading过场
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILoading);
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UISelection);
        }
    }


    [Event(EventIdType.EnterMapFinish)]
    public class EnterMap_Finish : AEvent
    {
        public override void Run()
        {
            Manager.RPG.state = NetworkState.World;
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILoading);
        }
    }

    [Event(EventIdType.BackLobby)]
    public class Back_Selection : AEvent
    {
        public override void Run()
        {
            Manager.RPG.CameraTo(NetworkState.Lobby);
            Manager.RPG.state = NetworkState.Lobby;
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UISelection);
        }
    }
}
