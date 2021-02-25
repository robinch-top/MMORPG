using UnityEngine;
using System.Collections;
using System;

namespace Assets.MMORPG.Scripts.RPGGame.Componet
{
    [RequireComponent(typeof(Level))]
    [DisallowMultipleComponent]
    public class Experience : MonoBehaviour
    {
        /// <summary>
        /// 绑定角色等级组件
        /// </summary>
        public Level level;

        /// <summary>
        /// 角色当前等级下的现有经验值
        /// </summary>
        [SerializeField] long _current = 0;
        public long current
        {
            get { return _current; }
            set { _current = value; }
        }

        // 暂时固定升级所需经验每级为1000
        [SerializeField] protected long _max = 1000;
        public long max { get { return _max; } }

        [Header("Death")]
        public float deathLossPercent = 0.05f;

        // helper functions ////////////////////////////////////////////////////////
        public float Percent() =>
            (current != 0 && max != 0) ? (float)current / (float)max : 0;

        // events //////////////////////////////////////////////////////////////////
        public virtual void OnDeath()
        {
            // 死亡失去经验，当然也可以设置死亡不掉经验，deathLossPercent属性设置为0
            current -= Convert.ToInt64(max * deathLossPercent);
        }
    }
}
