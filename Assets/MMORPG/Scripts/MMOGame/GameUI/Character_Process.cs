using Vector3 = UnityEngine.Vector3;
using Mirror;
using ETModel;
namespace MMOGame
{
    [Event(EventIdType.InitStart)]
    public class InitStart_CreateLogin : AEvent
    {
        public override void Run()
        {
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
}