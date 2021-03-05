using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Mirror;
using Assets.MMORPG.Scripts.RPGGame.ScriptableSkills;

namespace Assets.MMORPG.Scripts.RPGGame.Skill
{
 //技能的属性方法结构体
[Serializable]
public partial struct Skill
    {
        public int skilid;
        public int level;
        public double castTimeEnd;
        public double cooldownEnd;

        public Skill(ScriptableSkill data)
        {
            skilid = data.SkillID;

            // 默认学会的技能等级显示为1
            level = data.learnDefault ? 1 : 0;

            castTimeEnd = cooldownEnd = NetworkTime.time;
        }

        // 方便访问技能资源数据对象的包装
        public ScriptableSkill data
        {
            get
            {
                if (!ScriptableSkill.dict.ContainsKey(skilid))
                    throw new KeyNotFoundException("There is no ScriptableSkill with hash=" + skilid + ". Make sure that all ScriptableSkills are in the Resources folder so they are loaded properly.");
                return ScriptableSkill.dict[skilid];
            }
        }
        public string name => data.name;
        public float castTime => data.castTime.Get(level);
        public float cooldown => data.cooldown.Get(level);
        public float castRange => data.castRange.Get(level);
        public int manaCosts => data.manaCosts.Get(level);
        public bool followupDefaultAttack => data.followupDefaultAttack;
        public Sprite image => data.image;
        public bool learnDefault => data.learnDefault;
        public bool showCastBar => data.showCastBar;
        public bool cancelCastIfTargetDied => data.cancelCastIfTargetDied;
        public bool allowMovement => data.allowMovement;
        public int maxLevel => data.maxLevel;
        public ScriptableSkill predecessor => data.predecessor;
        public int predecessorLevel => data.predecessorLevel;
        public string requiredWeaponCategory => data.requiredWeaponCategory;
        public int upgradeRequiredLevel => data.requiredLevel.Get(level + 1);
        public long upgradeRequiredSkillExperience => data.requiredSkillExperience.Get(level + 1);

        public bool CheckSelf(Base.Entity caster, bool checkSkillReady = true)
        {
            return (!checkSkillReady || IsReady()) &&
                   data.CheckSelf(caster, level);
        }
        public bool CheckTarget(Base.Entity caster) { return data.CheckTarget(caster); }
        public bool CheckDistance(Base.Entity caster, out Vector3 destination) { return data.CheckDistance(caster, level, out destination); }
        public void Apply(Base.Entity caster) { data.Apply(caster, level); }
        // tooltip
        public string ToolTip(bool showRequirements = false)
        {
            // 未学习技能（0级）仍应显示1级的工具提示
            int showLevel = Mathf.Max(level, 1);

            StringBuilder tip = new StringBuilder(data.ToolTip(showLevel, showRequirements));

            // 添加 system hooks
            Base.Utils.InvokeMany(typeof(Skill), this, "ToolTip_", tip);

            // 只显示升级，如果已学会，并且不是最高技能等级
            if (0 < level && level < maxLevel)
            {
                tip.Append("\n<i>Upgrade:</i>\n" +
                           "<i>  Required Level: " + upgradeRequiredLevel + "</i>\n" +
                           "<i>  Required Skill Exp.: " + upgradeRequiredSkillExperience + "</i>\n");
            }

            return tip.ToString();
        }

        // 离技能施放时间结束还有多少时间
        public float CastTimeRemaining() => NetworkTime.time >= castTimeEnd ? 0 : (float)(castTimeEnd - NetworkTime.time);

        // 正在施放技能，如果剩余施法时间大于0
        public bool IsCasting() => CastTimeRemaining() > 0;

        // 离技能冷却结束还有多少时间
        public float CooldownRemaining() => NetworkTime.time >= cooldownEnd ? 0 : (float)(cooldownEnd - NetworkTime.time);

        public bool IsOnCooldown() => CooldownRemaining() > 0;

        public bool IsReady() => !IsCasting() && !IsOnCooldown();

    }
}
