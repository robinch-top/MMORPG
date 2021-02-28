using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Assets.MMORPG.Scripts.RPGGame.Entity
{
    public class Player : Base.Entity
    {
        /// <summary>
        /// 绑定经验组件
        /// </summary>
        public Componet.Experience experience;

        /// <summary>
        /// 获取基类绑定的背包组件属性
        /// </summary>
        public RPGGame.Player.PlayerInventory inventory;

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

        /// <summary>
        /// 角色道具CD的数据容器，简单版本可以是一个Dictionary<int, double>
        /// </summary>
        Dictionary<int, double> itemCooldowns = new Dictionary<int, double>();


        void Start()
        {
            // 暂时就一个角色放在场景里，先起用赋值localPlayer
            localPlayer = this;

        }
        // item cooldowns /////////////////////////////////////////////////////
        /// -><summary><c>GetItemCooldown</c> 获取道具的冷却时间</summary>
        public float GetItemCooldown(string cooldownCategory)
        {
            // 暂时不写，等待后面课时
            // ...

            // none found
            return 0;
        }
        /// -><summary><c>SetItemCooldown</c> 设置道具的冷却时间</summary>
        public void SetItemCooldown(string cooldownCategory, float cooldown)
        {
            // 暂时不写，等待后面课时
            // ...
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
