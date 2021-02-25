using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Player
{
    public class PlayerExperience : Componet.Experience
    {
        void Start()
        {

        }

        public override void OnDeath()
        {
            // 调用父类死亡方法，把所有有经验组件的角色，死亡后会引发的逻辑写在经验组件的父类中
            // 比如死亡扣经验的逻辑
            base.OnDeath();

            // 在聊天中输出一些信息
            // string message = "You died and lost experience.";
            // chat.TargetMsgInfo(message);
        }
    }
}
