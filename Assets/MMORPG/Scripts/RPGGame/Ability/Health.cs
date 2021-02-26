using UnityEngine;
using System.Collections;

namespace Assets.MMORPG.Scripts.RPGGame.Ability
{
    // 角色生命值组件
    [RequireComponent(typeof(Componet.Level))]
    [DisallowMultipleComponent]
    public class Health : Ability
    {
        public Base.LinearInt baseHealth = new Base.LinearInt { baseValue = 100 };
        public int endurance = 0;

        // 计算总蓝量
        // 这里定每点耐力30生命
        public override int max
        {
            get
            {
                int baseThisLevel = baseHealth.Get(level.current);
                return baseThisLevel + endurance * 30;
            }
        }

        void Start()
        {
            if (!spawnFull) current = 20;
        }
    }
}
