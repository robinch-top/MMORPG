using UnityEngine;
using System;

using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Base
{
    //Player，NPC，Monster等实体的父对象类
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public partial class Entity : NetBehaviourNonAlloc
    {
        [Header("Components")]
        public Animator animator;
        new public Collider collider;
        public AudioSource audioSource;

        /// <summary>
        /// 基类绑定背包组件  
        /// </summary>
        public Componet.Inventory baseInventory;

        /// <summary>
        /// 角色战斗或脱战状态  
        /// </summary>
        public bool inbattle = false;

        [SerializeField] long _gold = 0;
        /// <summary>
        /// 角色拥有的金币  
        /// </summary>
        public long gold
        {
            get { return _gold; }
            set { _gold = Math.Max(value, 0); }
        }

        /// <summary>
        /// 角色动作状态
        /// </summary>
        [SerializeField] string _state = "IDLE";
        public string state => _state;

        /// <summary>
        /// 角色等级  
        /// </summary>
        public Componet.Level level;
        /// <summary>
        /// 角色血量
        /// </summary>
        public Ability.Health health;
        /// <summary>
        /// 角色蓝量  
        /// </summary>
        public Ability.Mana mana;
        /// <summary>
        /// 基类绑定装备组件  
        /// </summary>
        public Componet.Equipment baseEquipment;
        GameObject _target;
        /// <summary>
        /// 角色目标 
        /// </summary>
        public Entity target
        {
            get { return _target != null ? _target.GetComponent<Entity>() : null; }
            set { _target = value != null ? value.gameObject : null; }
        }

        void Update()
        {
            // 暂时什么也不用写,肯定会有需要写点什么的时候的
            // ...
        }

        // death /////////////////////////////////////////////////////////////
        /// -><summary><c>OnDeath</c> 角色死亡时可以被子类调用的虚方法,
        /// 清除角色的目标属性值。</summary>
        public virtual void OnDeath()
        {
            //调用死亡动画，声音，躺了
            Debug.Log("啊!我躺了"); //暂时用在输出面板打印一句话代替
                                // 清除目标
            target = null;
        }
    }
}
