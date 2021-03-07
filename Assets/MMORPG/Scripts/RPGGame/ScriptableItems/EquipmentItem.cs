using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName="uMMORPG Item/Equipment", order=999)]
public class EquipmentItem : UsableItem
{   
    [Header("Equipment Attributes")]

    /// <summary>
    /// 装备位置
    /// </summary>
    public string category;
    
    /// <summary>
    /// 装备模型预制体
    /// </summary>
    public GameObject modelPrefab;

    /// <summary>
    /// 装备耐力值
    /// </summary>
    public int endurance;
    /// <summary>
    /// 装备智力值
    /// </summary>
    public int intellect;

    /// <summary>
    /// 装备力量值
    /// </summary>
    public int strength;
    /// <summary>
    /// 装备敏捷值
    /// </summary>
    public int agility; 
    /// <summary>
    /// 装备精神值
    /// </summary>
    public int spirit; 

    /// <summary>
    /// 装备物理攻击值
    /// </summary>
    public int physical; 
    /// <summary>
    /// 装备法术攻击值
    /// </summary>
    public int magic; 

    /// <summary>
    /// 装备法术暴击与命中值
    /// </summary>
    public float magicCritical;
    public float magicHitrate;
    
    /// <summary>
    /// 装备物理暴击与命中值
    /// </summary>
    public float physicCritical;
    public float physicHitrate;

    /// <summary>
    /// 装备闪避率
    /// </summary>
    public float dodgeChanceBonus; 
    /// <summary>
    /// 装备格挡/抵抗率
    /// </summary>
    public float blockChanceBonus; 
    
    /// <summary>
    /// 装备血量
    /// </summary>
    public int healthBonus;
    /// <summary>
    /// 装备蓝量
    /// </summary>
    public int manaBonus;
    /// <summary>
    /// 武器装备秒伤
    /// </summary>
    public float dpsBonus;
    /// <summary>
    /// 武器攻速
    /// </summary>
    public float attackRate;
    /// <summary>
    /// 装备武器伤害值
    /// </summary>
    public int hurtBonus;
    /// <summary>
    /// 装备护甲值
    /// </summary>
    public int armorBonus;
    /// <summary>
    /// 装备法术防御值
    /// </summary>
    public int magicDefense;
    /// <summary>
    /// 装备物理防御值
    /// </summary>
    public int physicalDefense;

    /// <summary>
    /// 装备生命回复值
    /// </summary>
    public int healthRecovery;
    /// <summary>
    /// 装备回蓝值
    /// </summary>
    public int manaRecovery;

    
    // CanUse /////////////////////////////////////////////////////////////
    /// -><summary>  检测背包能否放入指定数量的某种道具。
    /// 判断装备道具是否可用的方法,
    /// 需要判断装备界面是否有匹配他的装备界面位置,
    /// 需要判断装备是否有冷却时间和耐久度（有些饰品装备需要主动使用才对属性生效的道具，是有冷却时间的）。</summary>
    public override bool CanUse(Player player, int index, Item item)
    {
        return FindEquipableSlotFor(player, index) != -1;
    }
    // 查找可让道具，装备上去的装备界面格子index
    int FindEquipableSlotFor(Player player, int inventoryIndex)
    {
        for (int i = 0; i < player.equipment.slots.Count; ++i)
            if (CanEquip(player, inventoryIndex, i))
                return i;
        return -1;
    }
    // 能否把装备放到equipmentIndex位置的装备格子
    public bool CanEquip(Player player, int inventoryIndex, int equipmentIndex)
    {
        EquipSlotInfo slotInfo = player.equipment.slotInfo[equipmentIndex];
        string requiredCategory = slotInfo.requiredCategory;

        return requiredCategory != "" &&
               category.StartsWith(requiredCategory);
    }

    
    // Use /////////////////////////////////////////////////////////////
    /// -><summary>  使用道具的方法
    /// 在背包中点击调用使用道具方法，会装备到装备界面上对应的装备格子上
    /// 将已装备的道具拖到工具条上，点击使用此道具激活道具方法也定义在装备的Use方法中，比如饰品</summary>
    public override void Use(Player player, int index)
    {
        base.Use(player, index);

        // 找到一个接受这个类别的装备格子，然后装备它
        int equipmentIndex = FindEquipSlot(player);
        if (equipmentIndex != -1)
        {
            ItemSlot inventorySlot = player.inventory.slots[index];
            ItemSlot equipmentSlot = player.equipment.slots[equipmentIndex];

            // => merge 比如把箭矢，弹丸放到投射类弹药的格子
            // 是合并还是交换判断的标准: 同种道具，且可堆叠
            if (inventorySlot.amount > 0 && equipmentSlot.amount > 0 
                && inventorySlot.item.maxStack >1 
                && inventorySlot.item.Equals(equipmentSlot.item))
            {
                player.equipment.MergeInventoryEquip(index, equipmentIndex);
            }
            // 从背包到装备界面交换装备
            else
            {
                player.equipment.SwapInventoryEquip(index, equipmentIndex);
                //Debug.Log("换上装备");
            }
            
        }
    }
    // 遍历装备界面的各category位置，找到与此装备category匹配的格子
    public int FindEquipSlot(Player player){
        for (int i = 0; i < player.equipment.slots.Count; ++i)
            if (category.StartsWith(player.equipment.slotInfo[i].requiredCategory))
                return i;
        return -1;
    }

    // tooltip
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{ENDURANCE}", endurance.ToString());
        tip.Replace("{INTELECT}", intellect.ToString());
        tip.Replace("{STRENGTH}", strength.ToString());
        tip.Replace("{PHYSICAL}", physical.ToString());
        tip.Replace("{MAGIC}", magic.ToString());
        tip.Replace("{SPIRIT}", spirit.ToString());
        tip.Replace("{AGILITY}", agility.ToString());
        tip.Replace("{MAGICCRITICAL}", magicCritical.ToString("F1"));
        tip.Replace("{PHYSICCRITICAL}", physicCritical.ToString("F1"));
        tip.Replace("{MAGICHITRATE}", magicHitrate.ToString("F1"));
        tip.Replace("{PHYSICHITRATE}", physicHitrate.ToString("F1"));

        tip.Replace("{CATEGORY}", category);
        tip.Replace("{DAMAGEBONUS}", hurtBonus.ToString());
        tip.Replace("{ARMORBONUS}", armorBonus.ToString());
        tip.Replace("{DEMAGIC}", magicDefense.ToString());
        tip.Replace("{DEPHYSICAL}", physicalDefense.ToString());
        tip.Replace("{HEALTHBONUS}", healthBonus.ToString());
        tip.Replace("{MANABONUS}", manaBonus.ToString());
        tip.Replace("{DODGEBONUS}", dodgeChanceBonus.ToString());
        tip.Replace("{BLOCKCHANCEBONUS}", blockChanceBonus.ToString("F1"));
        
        return tip.ToString();
    }
}

