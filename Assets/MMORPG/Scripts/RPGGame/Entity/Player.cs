using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Entity
{
    public class Player : Base.Entity
    {
        /// <summary>
        /// 经验
        /// </summary>
        public Componet.Experience experience;
        /// <summary>
        /// 角色职业图标
        /// </summary>
        public Sprite classIcon;
        /// <summary>
        /// 角色头像
        /// </summary>
        public Sprite portraitIcon;

        /// <summary>
        /// 全局本地玩家
        /// </summary>
        public static Player localPlayer;

        void Start()
        {
            //暂时就一个角色放在场景里，先起用赋值localPlayer
            localPlayer = this;
        }

        // death /////////////////////////////////////////////////////////////
        /// -><summary><c>OnDeath</c> 玩家死亡时调用的方法，重写了基类方法。</summary>
        public override void OnDeath()
        {
            //调用基类方法
            base.OnDeath();
            //输出自己的死亡宣言
            Debug.Log("光荣战死...");
        }
    }
}
