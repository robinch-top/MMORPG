using System;
using System.Text;
using UnityEngine;

/// <summary>struct <c>ItemSlot</c> 
/// 背包格子属性方法结构体，类似的还有 :</summary>
/// <see cref="Item"/>
/// <see cref="ItemSlot"/>
/// <see cref="Equipment"/>
/// <see cref="Skill"/> - <see cref="Buff"/>
// Item            道具属性方法结构体
// ItemSlot        背包格子属性方法结构体
// Equipment       装备属性方法结构体
// Skill,Buff      技能,buff属性方法结构体

[Serializable]
public partial struct ItemSlot
{
    /// <summary>
    /// 格子中的道具
    /// </summary>
    public Item item;
    /// <summary>
    /// 格子中道具数量
    /// </summary>
    public int amount;

    /// <summary>
    /// ItemSlot结构体构造方法
    /// </summary>
    public ItemSlot(Item item, int amount=1)
    {
        this.item = item;
        this.amount = amount;
    }

    public int DecreaseAmount(int reduceBy)
    {
        int limit = Mathf.Clamp(reduceBy, 0, amount);
        amount -= limit;
        return limit;
    }

    public int IncreaseAmount(int increaseBy)
    {
        int limit = Mathf.Clamp(increaseBy, 0, item.maxStack - amount);
        amount += limit;
        return limit;
    }

    // ToolTip
    public string ToolTip()
    {
        if (amount == 0) return "";

        // 执行Item属性方法结构体的ToolTip()方法
        StringBuilder tip = new StringBuilder(item.ToolTip());
        tip.Replace("{AMOUNT}", amount.ToString());
        return tip.ToString();
    }
}

// 背包道具数据容器对象
public class SyncListItemSlot : SyncList<ItemSlot> {}