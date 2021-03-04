﻿using UnityEngine;
using System.Collections;

namespace Assets.MMORPG.Scripts.RPGGame.Ability
{
    // 角色蓝量组件
    [RequireComponent(typeof(Componet.Level))]
    [DisallowMultipleComponent]
    public class Mana : Ability
    {

        public Base.LinearInt baseMana = new Base.LinearInt { baseValue = 100 };
        // 缓存实现IManaBonus的组件实例
        IManaBonus[] _bonusComponents;
        IManaBonus[] bonusComponents =>
            _bonusComponents ?? (_bonusComponents = GetComponents<IManaBonus>());

        // 计算总蓝量
        // 这里定每点智力16蓝
        public override int max
        {
            get
            {
                int baseThisLevel = baseMana.Get(level.current);

                return baseThisLevel + intellect * 16;
            }
        }

        //获取智力总量
        public int intellect
        {
            get
            {
                int bonus = 0;
                foreach (IManaBonus bonusComponent in bonusComponents)
                    bonus += bonusComponent.GetIntellectBonus();
                return bonus;
            }
        }
        // 基础的单位时间回蓝量
        public int baseRecovery = 2;
        // 获取总单位时间回蓝量
        // 可以在这里增加通过装备，技能等提高单位时间的恢复量，但这一点功能的实现放到以后的课程
        public override int recovery
        {
            get
            {
                int bonus = 0;
                foreach (IManaBonus bonusComponent in bonusComponents)
                    bonus += bonusComponent.GetManaRecoveryBonus();
                return baseRecovery + bonus;
            }
        }

        // 如果设置为非重生满能力值，这里设置重生蓝量为10
        void Start()
        {
            if (!spawnFull) current = 10;
        }
    }
}