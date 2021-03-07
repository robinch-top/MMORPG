using UnityEngine;
using TMPro;
using System;
using Mirror;
// ============================================
// 施法时间与效果加成关系暂时没有
// 法术免疫随N次数递减（16 秒後从新计算）规则暂时没有
// 未考虑主属性对单手/双手武器的差别作用，如需要增加 float PrefHand(ActOnType type)
// 未考虑主属性对不同职业的差别作用,如需要增加 int PrefProfession(ActOnType type)
    //-> 力量/敏捷型物理职业对力量/敏捷到物攻强度换算不同
    //-> 物理职业对敏捷到物理暴击/闪避的换算不同
    //-> 法术职业对精神到回蓝、法术暴击的换算不同
    //-> 物理/法术职业从武器获得的近战攻击换算不同

public enum AttackType{ physical ,magic }
public enum DamageType { Normal, Block, Crit }

[RequireComponent(typeof(Level))]
[DisallowMultipleComponent]
public class Combat :  NetBehaviourNonAlloc
{
    /// <summary>
    /// 角色是否无敌不可战胜
    /// </summary>
    public bool invincible = false;

    /// <summary>
    /// 获取角色的Level组件,public但不序列化
    /// </summary>
    [NonSerialized]
    public Level level;

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab;

    ////////////////////////////////////////////////////////////////////////////
    // 获取及缓存 角色上的装备，技能，宠物坐骑等组件实例
    ICombatBonus[] _bonusComponents;
    ICombatBonus[] bonusComponents =>
        _bonusComponents ?? (_bonusComponents = GetComponents<ICombatBonus>());


    public LinearInt baseArmor = new LinearInt{baseValue=10};
    public LinearFloat baseDodge = new LinearFloat{baseValue=0.3f}; 


    ////////////////////////////////////////////////////////////////////////////
    // 攻防相关的基础属性的get方法
    // -> 通过下面这些方法计算角色的各种基础属性值
    /// <summary>获得角色敏捷属性值</summary>
    public int agility
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetAgilityBonus();
            return bonus;
        }
    }
    /// <summary>获得角色力量属性值</summary>
    public int strength
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetStrengthBonus();
            return bonus;
        }
    }
    /// <summary>获得角色精神属性值</summary>
    public int spirit
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetSpiritBonus();
            return bonus;
        }
    }
    /// <summary>获得角色智力属性值</summary>
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

    ////////////////////////////////////////////////////////////////////////////
    // 攻击伤害属性的get方法
    // -> 通过下面这些方法计算角色的各种战斗相关属性值
    /// <summary>总物理攻击强度，结合秒伤与物理攻击强度</summary>
    /// 总攻击强度 = 秒伤*攻速 + 额外的物理伤害加成
    /// 课程只做了单武器,如果你要做双手武器,那就是分别计算左右手的攻击强度与速度
    public int attackStrength
    {
        get
        {
            int bonus = 0;
            bonus += (int)(damagePerSecond*attackRate)+physicalDamage;
            return bonus;
        }
    }
    /// <summary>秒伤,从武器装备获得的dps值</summary>
    public float damagePerSecond
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetDpsBonus();
            return bonus;
        }
    }
    /// <summary>从武器装备获得的攻速值</summary>
    public float attackRate
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetAttackRateBonus();
            return bonus;
        }
    }
    /// <summary>武器伤害值,当有些技能可以产生武器伤害时可以调用</summary>
    public int hurtDamage
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetHurtBonus();
            return  bonus;
        }
    }
    /// <summary>物理攻击强度，除从秒伤得来的物理伤害外，额外产生的的物理伤害加成</summary>
    public int physicalDamage
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetPhysicalBonus();
            return bonus + strength / 6 + agility / 12 ;
        }
    }
    /// <summary>获得法术伤害强度</summary>
    public int magicDamage
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetMagicBonus();
            return bonus + (int)spirit/4;
        }
    }


    ////////////////////////////////////////////////////////////////////////////
    // 伤害防御属性的get方法
    // -> 通过下面这些方法计算角色的各种防御相关属性值
    /// <summary> 获得防御护甲属性值 </summary>
    public int armorDefense
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetArmorBonus();
            return baseArmor.Get(level.current) + bonus + agility * 2;
        }
    }
    /// <summary> 获得的物理抗性值 </summary>
    public int physicalDefense{
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetPhysicalDefenseBonus();
            return bonus;
        }
    }
    /// <summary> 获得的法术抗性值 </summary>
    public int magicDefense
    {
        get
        {
            int bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetMagicDefenseBonus();
            return bonus;
        }
    }
    /// <summary> 获得的抵抗伤害比率 </summary>
    public float blockChance
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetBlockBonus();
            return bonus;
        }
    }
    /// <summary> 获得的闪避机率 </summary>
    public float dodgeChance
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetDodgeBonus();
            return baseDodge.Get(level.current) + bonus + (float)agility/20f;
        }
    }

    
    ////////////////////////////////////////////////////////////////////////////
    // 命中属性的get方法
    // -> 通过下面这些方法计算角色的命中属性值
    /// <summary> 获得的法术命中 </summary>
    public float magicHitrate
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetMagicHitrateBonus();
            return bonus;
        }
    }
    /// <summary> 获得的物理命中 </summary>
    public float physicHitrate
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetPhysicHitrateBonus();
            return bonus;
        }
    }


    ////////////////////////////////////////////////////////////////////////////
    // 暴击属性的get方法
    // -> 通过下面这些方法计算角色的暴击属性值
    /// <summary> 获得的法术暴击 </summary>
    public float magicCritical
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetMagicCriticalBonus();
            return  bonus + (float)intellect/42f;
        }
    }
    /// <summary> 获得的物理暴击 </summary>
    public float physicCritical
    {
        get
        {
            float bonus = 0;
            foreach (ICombatBonus bonusComponent in bonusComponents)
                bonus += bonusComponent.GetPhysicCriticalBonus();
            return  bonus + (float)agility/35f;
        }
    }


    public void Start()
    {
        level = GetComponent<Level>();
    }

    /// <summary> 根据施放的攻击技能类型返回物理或法术攻击数值 </summary>
    public int Attack(AttackType type)
    {
        int damage = 0;
        if(type == AttackType.magic){
            damage = magicDamage;
        }else if(type == AttackType.physical)
        {
            damage = attackStrength;
        }
        // 对伤害进行浮动
        System.Random r1 = new System.Random();
        System.Random r2 = new System.Random();
        // 用两个随机值相减的方式，这样会以20%左右的damage数值上下浮动
        float d = damage*(r1.Next(5,45)*0.01f) - damage*(r2.Next(9,25)*0.01f); 
        return damage + (int)d;
    }

    /// <summary> 根据施放的攻击技能类型返回物理或法术防御数值 Defensive attack </summary>
    public int DeAttack(AttackType type)
    {
         int defense = 0;
        if(type == AttackType.magic){
            defense = magicDefense;
        }else if(type == AttackType.physical)
        {
            defense = physicalDefense;
        }
        return defense;
    }

    /// <summary> 根据施放的攻击技能类型返回物理或法术暴击数值 </summary>
    public float AttackCritical(AttackType type){
        float critical = 0;
        if(type == AttackType.magic){
            critical = magicCritical;
        }else if(type == AttackType.physical)
        {
            critical = physicCritical + 30; //暂时加30暴击看效果
        }
        return critical;
    }

    /// <summary>还没有服务端这里模仿一个处理技能伤害的方法 </summary>
    public virtual void DealDamageAt(Entity caster, AttackType type, int skillDamage,  float stunChance=0, float stunTime=0)
    {
        DamageType damageType = DamageType.Normal;

        Entity victim = caster.target; // 目标实体
        Combat vCombat = victim.combat; // 目标的Combat组件
        

        int damageDealt = 0;
        int amount = caster.combat.Attack(type) + skillDamage;
        int deAmount = vCombat.DeAttack(type);

        // 是不是不可战胜的,比如npc
        if (!vCombat.invincible)
        {
            // dodge
            if (UnityEngine.Random.value < vCombat.dodgeChance/100)
            {
                damageType = DamageType.Block;
            }
            // deal damage
            else
            {
                // 减少去防御与抵抗效果
                damageDealt = Mathf.Max(amount - deAmount, 1);

                // 触发暴击
                if (UnityEngine.Random.value < AttackCritical(type)/100)
                {
                    damageDealt = damageDealt*2;
                    damageType = DamageType.Crit;
                }

                // 处理伤害结果
                victim.health.current -= damageDealt;

            }

            // 通知发起攻击方武器掉耐久和防御攻击方装备掉耐久
            // ...

            // 攻击目标是否死亡
            // ...
        }

        // 判断目标aggro范围,决定是否继续追击
        // victim.OnAggro(entity);

        // 这里是直接调用前端受到伤害的方法，实际在服务端这是向前端发消息
        vCombat.RpcOnReceivedDamaged(victim,damageDealt, damageType);

        // 是否有机率造成目标眩晕
        if (UnityEngine.Random.value < stunChance)
        {
            // 只需要更新角色的stunTimeEnd属性，角色的状态update中就会更新眩晕状态
            double newStunEndTime = NetworkTime.time + stunTime;
            victim.stunTimeEnd = Math.Max(newStunEndTime, caster.stunTimeEnd);
        }
    }

    /// <summary>前端受到伤害调用方法 </summary>
    public void RpcOnReceivedDamaged(Entity victim,int amount, DamageType damageType)
    {
        // 伤害跳动数字
        ShowDamagePopup(victim,amount, damageType);

        // 调用委托方法，角色受伤动作等
        // ...
    }

    /// <summary>显示伤害数字方法 </summary>
    void ShowDamagePopup(Entity victim,int amount, DamageType damageType)
    {
        // 产生伤害弹出UI（如果有的话）并设置文本
        if (damagePopupPrefab != null)
        {
            // 这里是将伤害数字显示在角色右侧，如果放在头顶最简单不需要做坐标转换
            // 如果将数字显示在挡住角色前面的位置，要做世界空间UI的自定义着色器
            Bounds bounds = victim.collider.bounds;
            Vector3 position = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
            GameObject popup = Instantiate(damagePopupPrefab, position, Quaternion.identity);

            // 了解 Translate ，不论攻击者视角怎么围绕目标转，
            // 伤害数字都在目标右侧一定距离显示
            popup.transform.Translate(Camera.main.transform.right*1.5f);
            if (damageType == DamageType.Normal)
                popup.GetComponentInChildren<TextMeshPro>().text = amount.ToString();
            else if (damageType == DamageType.Block)
                popup.GetComponentInChildren<TextMeshPro>().text = "<i>格挡!</i>";
            else if (damageType == DamageType.Crit)
                popup.GetComponentInChildren<TextMeshPro>().text = amount + " 暴击!";
        }
    }
}