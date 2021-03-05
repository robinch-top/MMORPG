using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.ScriptableSkills
{
    /// 普通的近战攻击技能
    [CreateAssetMenu(menuName = "uMMORPG Skill/Target Damage", order = 999)]
    public class TargetDamageSkill : DamageSkill
    {
        public override bool CheckTarget(Base.Entity caster)
        {
            // 目标存在, 存活, not self, 可攻击?
            return caster.target != null && caster.CanAttack(caster.target);
        }

        public override bool CheckDistance(Base.Entity caster, int skillLevel, out Vector3 destination)
        {
            // 目标还在附近?
            if (caster.target != null)
            {
                destination = Base.Utils.ClosestPoint(caster.target, caster.transform.position);
                return Base.Utils.ClosestDistance(caster, caster.target) <= castRange.Get(skillLevel);
            }
            destination = caster.transform.position;
            return false;
        }

        public override void Apply(Base.Entity caster, int skillLevel)
        {
            // 直接造成基础伤害（装备属性形成）+ 技能伤害
            caster.combat
                .DealDamageAt(caster,
                                       attackType,
                                       damage.Get(skillLevel),
                                       stunChance.Get(skillLevel),
                                       stunTime.Get(skillLevel));
        }
    }
}
