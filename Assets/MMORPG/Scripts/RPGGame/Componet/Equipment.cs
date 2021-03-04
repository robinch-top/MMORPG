
using Assets.MMORPG.Scripts.RPGGame.Ability;
using Assets.MMORPG.Scripts.RPGGame.Items;
using Assets.MMORPG.Scripts.RPGGame.ScriptableItems;
using UnityEngine;

namespace Assets.MMORPG.Scripts.RPGGame.Componet
{

    // 用于实体对象的装备组件基础类,其它类似的还有:
    // Skills - PlayerSkills
    // Equipment - PlayerEquipment
    // Inventory - PlayerInventory

    [DisallowMultipleComponent]
    public abstract class Equipment : ItemContainer, ICombatBonus, IHealthBonus, IManaBonus
    {
        /***************************************************************************
    * 用于持有实体的装备Scriptable资源数据对象的数组                               *
    * ItemSC[] equipments                                                      *
    *=========================================================================*/
        public ItemSC[] equipments;


        /***************************************************************************
        * 获取装备提升生命，蓝的自动回复属性                                          *
        *=========================================================================*/
        /// -> <summary>获取装备生命回复值的接口方法</summary>
        public int GetHealthRecoveryBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).healthRecovery;
            return bonus;
        }
        /// -> <summary>获取装备蓝量回复值的接口方法</summary>
        public int GetManaRecoveryBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).manaRecovery;
            return bonus;
        }
        /***************************************************************************
   * 获取装备基本能力属性的接口方法                                              *
   *=========================================================================*/
        /// <summary>获得装备的血量属性值</summary>
        public int GetHealthBonus(int baseHealth)
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).healthBonus;
            return bonus;
        }
        /// <summary>获得装备的蓝量属性值</summary>
        public int GetManaBonus(int baseMana)
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).manaBonus;
            return bonus;
        }
        /// <summary>获得装备的耐力属性值</summary>
        public int GetEnduranceBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).endurance;
            return bonus;
        }
        /// <summary>获得装备的智力属性值</summary>
        public int GetIntellectBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).intellect;
            return bonus;
        }
        /***************************************************************************
   * 获取装备攻防相关基础属性的接口方法                                           *
   *=========================================================================*/
        /// <summary>获得装备的敏捷属性值</summary>
        public int GetAgilityBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).agility;
            return bonus;
        }
        /// <summary>获得装备的力量属性值</summary>
        public int GetStrengthBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).strength;
            return bonus;
        }
        /// <summary>获得装备的精神属性值</summary>
        public int GetSpiritBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).spirit;
            return bonus;
        }
        /***************************************************************************
   * 获取装备伤害属性的接口方法                                                 *
   *=========================================================================*/
        /// <summary>秒伤,从武器装备获得的dps值</summary>
        public float GetDpsBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).dpsBonus;
            return bonus;
        }
        /// <summary>从武器装备获得的攻速值</summary>
        public float GetAttackRateBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).attackRate;
            return bonus;
        }
        /// <summary>武器伤害值,当有些技能可以产生武器伤害时可以调用</summary>
        public int GetHurtBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).hurtBonus;
            return bonus;
        }
        /// <summary>获得装备法术伤害强度</summary>
        public int GetMagicBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).magic;
            return bonus;
        }
        /// <summary>获得装备物理攻击强度
        /// 除从秒伤得来的物理伤害外，额外产生的的物理伤害加成</summary>
        public int GetPhysicalBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).physical;
            return bonus;
        }

        /***************************************************************************
        * 获取装备防御属性的接口方法                                                 *
        *=========================================================================*/
        /// <summary> 获得装备防御护甲属性值 </summary>
        public int GetArmorBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).armorBonus;
            return bonus;
        }
        /// <summary> 获得装备的法术抗性值 </summary>
        public int GetMagicDefenseBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).magicDefense;
            return bonus;
        }
        /// <summary> 获得装备的物理抗性值 </summary>
        public int GetPhysicalDefenseBonus()
        {
            int bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).physicalDefense;
            return bonus;
        }
        /// <summary> 获得装备的闪避机率 </summary>
        public float GetDodgeBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).dodgeChanceBonus;
            return bonus;
        }
        /// <summary> 获得装备的抵抗伤害比率 </summary>
        public float GetBlockBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).blockChanceBonus;
            return bonus;
        }
        /***************************************************************************
    * 获取装备命中属性的接口方法                                                 *
    *=========================================================================*/
        /// <summary> 获得装备的法术命中 </summary>
        public float GetMagicHitrateBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).magicHitrate;
            return bonus;
        }
        /// <summary> 获得装备的物理命中 </summary>
        public float GetPhysicHitrateBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).physicHitrate;
            return bonus;
        }
        /***************************************************************************
    * 获取装备暴击属性的接口方法                                                 *
    *=========================================================================*/
        /// <summary> 获得装备的法术暴击 </summary>
        public float GetMagicCriticalBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).magicCritical;
            return bonus;
        }
        /// <summary> 获得装备的物理暴击 </summary>
        public float GetPhysicCriticalBonus()
        {
            float bonus = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.CheckDurability())
                    bonus += ((EquipmentItem)slot.item.data).physicCritical;
            return bonus;
        }


        ////////////////////////////////////////////////////////////////////////////
        // 帮助函数
        /// <summary>查找武器装备的索引,如果没有武器装备，返回-1。</summary>
        public int GetEquippedWeaponIndex()
        {
            // (avoid FindIndex to minimize allocations)
            for (int i = 0; i < slots.Count; ++i)
            {
                ItemSlot slot = slots[i];
                if (slot.amount > 0 && slot.item.data is WeaponItem)
                    return i;
            }
            return -1;
        }
        /// <summary>获取当前装备的武器类别，检查技能是否可以用这个武器施放。
        /// 如果没有，则返回 "" </summary>
        public string GetEquippedWeaponCategory()
        {
            int index = GetEquippedWeaponIndex();
            return index != -1 ? ((WeaponItem)slots[index].item.data).category : "";
        }
    }
}