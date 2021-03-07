using System.Text;
using UnityEngine;

// 药剂类道具资源数据对象

[CreateAssetMenu(menuName="uMMORPG Item/Potion", order=999)]
public class PotionItem : UsableItem
{
    [Header("Potion")]
    public int usageHealth;
    public int usageMana;
    public int usageExperience;

    // 重写药剂类道具Use方法
    public override void Use(Player player, int index)
    {
        // 调用基类Use方法
        base.Use(player, index);

        // 增加 health/mana/etc.
        // 暂不实现道具使用逻辑...

        // 减少道具数量
        ItemSlot slot = player.inventory.slots[index];
        slot.DecreaseAmount(1);
        player.inventory.slots[index] = slot;
    }

    // PotionItem重写药剂类道具的tooltip
    public override string ToolTip()
    {
        StringBuilder tip = new StringBuilder(base.ToolTip());
        tip.Replace("{USAGEHEALTH}", usageHealth.ToString());
        tip.Replace("{USAGEMANA}", usageMana.ToString());
        tip.Replace("{USAGEEXPERIENCE}", usageExperience.ToString());
        return tip.ToString();
    }
}