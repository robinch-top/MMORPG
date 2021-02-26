using UnityEngine;
using System.Collections;
using System;

namespace Assets.MMORPG.Scripts.RPGGame.Ability
{
    public abstract class Ability : MonoBehaviour
    {
        int _current = 0;
        /// <summary>
        /// 能力值的当前数值
        /// </summary>
        public int current
        {
            get { return Mathf.Min(_current, max); }
            set
            {
                _current = Mathf.Clamp(value, 0, max);
            }
        }
        /// <summary>
        /// 角色重生后,能力值是否立即为满值
        /// </summary>
        public bool spawnFull = true;
        /// <summary>
        /// 能力值的最大值,基类定义为抽象属性
        /// </summary>
        public abstract int max { get; }

        /// <summary>
        /// 能力组件中获取所属角色Entity组件
        /// </summary>
        Base.Entity entity;
        /// <summary>
        /// 获取所属角色的Health组件,public但不序列化
        /// </summary>
        [NonSerialized]
        public Health health;
        /// <summary>
        /// 获取所属角色的Level组件,public但不序列化
        /// </summary>
        [NonSerialized]
        public Componet.Level level;
        public void Awake()
        {
            entity = GetComponent<Base.Entity>();
            health = GetComponent<Health>();
            level = GetComponent<Componet.Level>();

            // 重生时满数值
            if (spawnFull) current = max;

        }

        // Percent /////////////////////////////////////////////////////////////
        /// -> <summary>获取当前值与最大值的百分比。</summary>
        public float Percent() =>
            (current != 0 && max != 0) ? (float)current / (float)max : 0;

    }
}
