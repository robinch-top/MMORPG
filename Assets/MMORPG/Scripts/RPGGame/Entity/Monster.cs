using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Entity
{
    public class Monster : Base.Entity
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
        /// -><summary><c>OnDeath</c> 怪物死亡时调用的方法，重写了基类方法。</summary>
        public override void OnDeath()
        {
            //调用基类方法
            base.OnDeath();
            //输出自己的死亡宣言
            Debug.Log("我就这么无声无息的死去了...");
        }
    }
}
