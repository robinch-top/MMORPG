using System;
using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.Collections.Generic;
using Assets.MMORPG.Scripts.RPGGame.Ability;

namespace Assets.MMORPG.Scripts.RPGGame.Componet
{
    [Serializable] public class UnityEventSkill : UnityEvent<Skill.Skill> { }

    /// <summary>Class <c>SkillsComponet</c> 
    /// 用于实体对象的技能组件基础类,其它类似的还有:</summary>
    [DisallowMultipleComponent]
    public class SkillsComponet : Items.ItemContainer
    {
        [Header("Components")]
        public Base.Entity entity;
        public Level level;
        public Health health;
        public Mana mana;

        [Header("Skill Experience")]
        public long skillExperience = 0;

        /// <summary>
        /// 角色当前技能
        /// </summary>
        [HideInInspector] public int currentSkill = -1;

        /// <summary>
        /// 角色挂起技能
        /// </summary>
        [HideInInspector] public int pendingSkill = -1;
        [HideInInspector] public Vector3 pendingDestination;
        [HideInInspector] public bool pendingDestinationValid;


        [Header("Skills & Buffs")]
        /***************************************************************************
        * 用于持有实体的技能Scriptable对象的数组                                     *
        *   ScriptableSkill[] skillTemplates                                       *
        * 技能的数据容器                                                   *
        *   SyncListSkill skills                                                   *
        * buff的数据容器                                                   *
        *   SyncListBuff buffs                                                     *
        *=========================================================================*/
        protected ScriptableSkills.ScriptableSkill[] skillTemplates;
        public List<Skill.Skill> skills = new List<Skill.Skill>();
        // public List<Buff> buffs = new List<Buff>(); 
        [SerializeField] Transform _effectMount;
        public virtual Transform effectMount
        {
            get { return _effectMount; }
            set { _effectMount = value; }
        }

        [Header("Events")]
        public UnityEventSkill onSkillCastStarted;
        public UnityEventSkill onSkillCastFinished;


        // helper function 取得 skill index
        public int GetSkillIndexByName(string skillName)
        {
            for (int i = 0; i < skills.Count; ++i)
                if (skills[i].name == skillName)
                    return i;
            return -1;
        }

        public bool CastCheckSelf(Skill.Skill skill, bool checkSkillReady = true) =>
            skill.CheckSelf(entity, checkSkillReady);

        public bool CastCheckTarget(Skill.Skill skill) =>
            skill.CheckTarget(entity);

        public bool CastCheckDistance(Skill.Skill skill, out Vector3 destination) =>
            skill.CheckDistance(entity, out destination);

        /// <summary>还没有服务端这里模仿一个施放技能的方法 </summary>
        public void StartCast(Skill.Skill skill, int currentSkill)
        {
            // 开始施放技能并设置施放结束时间
            skill.castTimeEnd = NetworkTime.time + skill.castTime;
            // skill属性改变，保存到List中的引用
            skills[currentSkill] = skill;

            // 这里是直接调用前端要执行的方法，实际在服务端这是向前端发消息
            RpcCastStarted(skill);
        }

        /// <summary>还没有服务端这里模仿一个完成施放技能的方法 </summary>
        public void FinishCast(Skill.Skill skill, int currentSkill)
        {
            if (CastCheckSelf(skill, false) && CastCheckTarget(skill))
            {
                // 执行技能方法
                skill.Apply(entity);

                RpcCastFinished(skill);

                // decrease mana in any case
                mana.current -= skill.manaCosts;

                // skill属性改变，保存到List中的引用
                // start the cooldown
                skill.cooldownEnd = NetworkTime.time + skill.cooldown;
                skills[currentSkill] = skill;
            }
            else
            {
                currentSkill = -1;
            }
        }
        /// <summary>取消技能施放的方法，到后面课时这也只是服务端才需要的方法 </summary>
        public void CancelCast(bool resetCurrentSkill = true)
        {
            if (currentSkill != -1)
            {
                Skill.Skill skill = skills[currentSkill];

                // skill属性改变，保存到List中的引用
                skill.castTimeEnd = NetworkTime.time - skill.castTime;
                skills[currentSkill] = skill;

                // reset current skill
                if (resetCurrentSkill)
                    currentSkill = -1;
            }
        }

        /// <summary>前端施放技能调用的方法 </summary>
        public void RpcCastStarted(Skill.Skill skill)
        {
            // 判断是否活着
            if (health.current > 0)
            {
                // call scriptableskill event
                skill.data.OnCastStarted(entity);

                // call event
                onSkillCastStarted.Invoke(skill);
            }
        }
        /// <summary>前端结束施放技能调用的方法 </summary>
        public void RpcCastFinished(Skill.Skill skill)
        {
            // 判断是否活着
            if (health.current > 0)
            {
                // call scriptableskill event
                skill.data.OnCastFinished(entity);

                // call event
                onSkillCastFinished.Invoke(skill);
            }
        }
    }
}
