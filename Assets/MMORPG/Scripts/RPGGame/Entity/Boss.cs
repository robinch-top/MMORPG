using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Entity
{
    public class Boss : Base.Entity
    {
        /// <summary>
        /// 角色职业图标
        /// </summary>
        public Sprite classIcon;
        /// <summary>
        /// 角色头像
        /// </summary>
        public Sprite portraitIcon;

        // death /////////////////////////////////////////////////////////////
        /// -><summary><c>OnDeath</c> Boss死亡时调用的方法，重写了基类方法。</summary>
        public override void OnDeath()
        {
            //调用基类方法
            base.OnDeath();
            //输出自己的死亡宣言
            Debug.Log("你不可能战胜我，我已经侵蚀你的灵魂，你将永远成为我的奴隶，哈哈哈...");
        }
    }
}
