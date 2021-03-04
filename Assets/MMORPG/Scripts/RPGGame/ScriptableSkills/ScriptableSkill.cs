using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.MMORPG.Scripts.RPGGame.Base;
using System.Text;

namespace Assets.MMORPG.Scripts.RPGGame.ScriptableSkills
{
    public abstract partial class ScriptableSkill : Base.ScriptableNonAlloc
    {
        [Header("Info")]
        public bool followupDefaultAttack;
        public string skillName;
        public int SkillID;
        public Componet.AttackType attackType; //物理系或法系技能;
        [SerializeField, TextArea(1, 30)] protected string toolTip;
        public Sprite image;
        public bool learnDefault;
        public bool showCastBar;
        public bool cancelCastIfTargetDied;
        public bool allowMovement;

        [Header("Requirements")]
        public ScriptableSkill predecessor;
        public int predecessorLevel = 1;
        public string requiredWeaponCategory = "";
        public LinearInt requiredLevel; // 需要角色 level
        public LinearLong requiredSkillExperience;

        [Header("Properties")]
        public int maxLevel = 1;
        public LinearInt manaCosts;
        public LinearFloat castTime;
        public LinearFloat cooldown;
        public LinearFloat castRange;

        [Header("Sound")]
        public AudioClip castSound;
        bool CheckWeapon(Base.Entity caster)
        {

            // 对武器的要求
            if (string.IsNullOrWhiteSpace(requiredWeaponCategory))
                return true;

            // 对武器类型的要求
            if (caster.baseEquipment.GetEquippedWeaponCategory().StartsWith(requiredWeaponCategory))
            {
                // 耐久度检查
                int weaponIndex = caster.baseEquipment.GetEquippedWeaponIndex();
                if (weaponIndex != -1)
                {
                    return caster.baseEquipment.slots[weaponIndex].item.CheckDurability();
                }
            }
            return false;
        }


        public virtual bool CheckSelf(Base.Entity caster, int skillLevel)
        {
            // 有武器, no cooldown, hp, mp?
            return caster.health.current > 0 &&
                   caster.mana.current >= manaCosts.Get(skillLevel) &&
                   CheckWeapon(caster);
        }
        // 2. 目标检查
        public abstract bool CheckTarget(Base.Entity caster);

        // 3. 距离检查
        public abstract bool CheckDistance(Base.Entity caster, int skillLevel, out Vector3 destination);

        // 4. 执行技能: deal damage, heal, launch projectiles, etc.
        public abstract void Apply(Base.Entity caster, int skillLevel);

        // events for client sided effects /////////////////////////////////////////
        public virtual void OnCastStarted(Base.Entity caster)
        {
            if (caster.audioSource != null && castSound != null)
                caster.audioSource.PlayOneShot(castSound);
        }

        public virtual void OnCastFinished(Base.Entity caster) { }
        // tooltip /////////////////////////////////////////////////////////////////
        public virtual string ToolTip(int level, bool showRequirements = false)
        {
            StringBuilder tip = new StringBuilder(toolTip);
            tip.Replace("{NAME}", skillName);
            tip.Replace("{LEVEL}", level.ToString());
            tip.Replace("{CASTTIME}", Utils.PrettySeconds(castTime.Get(level)));
            tip.Replace("{COOLDOWN}", Utils.PrettySeconds(cooldown.Get(level)));
            tip.Replace("{CASTRANGE}", castRange.Get(level).ToString());
            tip.Replace("{MANACOSTS}", manaCosts.Get(level).ToString());

            // 在必要时显示技能对等级或其它技能的要求
            if (showRequirements)
            {
                tip.Append("\n<b><i>Required Level: " + requiredLevel.Get(1) + "</i></b>\n" +
                           "<b><i>Required Skill Exp.: " + requiredSkillExperience.Get(1) + "</i></b>\n");
                if (predecessor != null)
                    tip.Append("<b><i>Required Skill: " + predecessor.name + " Lv. " + predecessorLevel + " </i></b>\n");
            }

            return tip.ToString();
        }
        // caching /////////////////////////////////////////////////////////////////
        static Dictionary<int, ScriptableSkill> cache;
        public static Dictionary<int, ScriptableSkill> dict
        {
            get
            {
                // not loaded yet?
                if (cache == null)
                {
                    // 从 resources 下加载所有的 ScriptableSkill
                    ScriptableSkill[] skills = Resources.LoadAll<ScriptableSkill>("");

                    // 检查重复项，然后添加到缓存
                    List<string> duplicates = skills.ToList().FindDuplicates(skill => skill.name);
                    if (duplicates.Count == 0)
                    {
                        cache = skills.ToDictionary(skill => skill.SkillID, skill => skill);
                    }
                    else
                    {
                        foreach (string duplicate in duplicates)
                            Debug.LogError("Resources folder contains multiple ScriptableSkills with the name " + duplicate + ". If you are using subfolders like 'Warrior/NormalAttack' and 'Archer/NormalAttack', then rename them to 'Warrior/(Warrior)NormalAttack' and 'Archer/(Archer)NormalAttack' instead.");
                    }
                }
                return cache;
            }
        }
    }
}
