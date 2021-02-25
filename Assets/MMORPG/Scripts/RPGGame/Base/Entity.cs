using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Base
{
    public class Entity : MonoBehaviour
    {
        /// <summary>
        /// 角色中文名称  
        /// </summary>
        public string CNName;

        [Header("Components")]
        public Animator animator;
        new public Collider collider;
        public AudioSource audioSource;

        /// <summary>
        /// 角色战斗或脱战状态  
        /// </summary>
        public bool inbattle = false;

        [SerializeField] float _gold = 0f;
        /// <summary>
        /// 角色拥有的金币  
        /// </summary>
        public float gold
        {
            get { return _gold; }
            set { _gold = Mathf.Max(value, 0); }
        }

        /// <summary>
        /// 角色等级  
        /// </summary>
        public Componet.Level level;
        /// <summary>
        /// 角色血量
        /// </summary>
        public int health;
        /// <summary>
        /// 角色蓝量  
        /// </summary>
        public int mana;

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
        /// -><summary><c>OnDeath</c> 角色死亡时可以被子类调用的虚方法。
        /// 比如死亡时清除角色的目标属性值，这是任何种类的角色都要执行的，
        /// 所以在基类的OnDeath方法可以清除目标 target = null。
        /// 当需要实现在子类可以重写这个方法，但又不强制必须重写时定义虚方法
        /// </summary>
        public virtual void OnDeath()
        {
            // 清除目标
            target = null;
        }
        void Awake()
        {

        }
        // Use this for initialization
        void Start()
        {
            
        }
    }
}
