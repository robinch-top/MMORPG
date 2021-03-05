using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.MMORPG.Scripts.RPGGame.Base;
using Mirror;
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
        public RPGGame.Player.PlayerInventory inventory => baseInventory as RPGGame.Player.PlayerInventory;

        /// <summary>
        /// 获取基类绑定的装备组件
        /// </summary>
        public RPGGame.Player.PlayerEquipment equipment => baseEquipment as RPGGame.Player.PlayerEquipment;
        /// <summary>
        /// 获取绑定的技能工具条组件
        /// </summary>
        public RPGGame.Player.PlayerSkillbar skillbar;
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
        /// 角色道具CD的数据容器，他是一个SyncDictionary<int, double>
        /// </summary>
        SyncDictionaryIntDouble itemCooldowns = new SyncDictionaryIntDouble();

        GameObject _nextTarget;
        /// <summary>
        /// 角色下一个目标 
        /// 想一下，为什么target放在Entity中，而nextTarget放在Player中
        /// </summary>
        public Base.Entity nextTarget
        {
            get { return _nextTarget != null ? _nextTarget.GetComponent<Base.Entity>() : null; }
            set { _nextTarget = value != null ? value.gameObject : null; }
        }
        void Start()
        {
            // 暂时就一个角色放在场景里，先起用赋值localPlayer
            //localPlayer = this;

        }


        // item cooldowns /////////////////////////////////////////////////////
        /// -><summary><c>GetItemCooldown</c> 获取道具的冷却时间</summary>
        public float GetItemCooldown(string cooldownCategory)
        {
            // 获取稳定的哈希以减少带宽
            int hash = cooldownCategory.GetStableHashCode();

            // 查找同类物品冷确
            if (itemCooldowns.TryGetValue(hash, out double cooldownEnd))
            {
                return NetworkTime.time >= cooldownEnd ? 0 : (float)(cooldownEnd - NetworkTime.time);
            }

            // none found
            return 0;
        }
        /// -><summary><c>SetItemCooldown</c> 设置道具的冷却时间</summary>
        public void SetItemCooldown(string cooldownCategory, float cooldown)
        {
            // 获取稳定的哈希以减少带宽
            int hash = cooldownCategory.GetStableHashCode();

            // save end time
            itemCooldowns[hash] = NetworkTime.time + cooldown;
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
        // CmdSetTarget /////////////////////////////////////////////////////////////
        /// -><summary>设置角色目标。</summary>
        public void CmdSetTarget(Network.NetworkIdentity ni)
        {
            // validate
            if (ni != null)
            {
                // 直接切换目标，或技能已经在施放中，则在技能释放后切换作为下一个目标
                if (state == "IDLE" || state == "MOVING" || state == "STUNNED")
                    target = ni.GetComponent<Base.Entity>();
                else if (state == "CASTING")
                    nextTarget = ni.GetComponent<Base.Entity>();
            }
        }
    }
    public class SyncDictionaryIntDouble : Data.SyncDictionary<int, double> { }
}
