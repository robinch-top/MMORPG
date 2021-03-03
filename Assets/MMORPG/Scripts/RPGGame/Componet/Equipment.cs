
using UnityEngine;

namespace Assets.MMORPG.Scripts.RPGGame.Componet
{

// 用于实体对象的装备组件基础类,其它类似的还有:
// Skills - PlayerSkills
// Equipment - PlayerEquipment
// Inventory - PlayerInventory

[DisallowMultipleComponent]
public abstract class Equipment : Items.ItemContainer
{
    /***************************************************************************
    * 用于持有实体的装备Scriptable资源数据对象的数组                                 *
    * ItemSC[] equipments                                                   *
    *=========================================================================*/
    public Items.ItemSC[] equipments ;

    
    ////////////////////////////////////////////////////////////////////////////
    // 帮助函数，查找武器装备的索引
    //-> 如果没有武器装备，返回-1。
    public int GetEquippedWeaponIndex()
    {
        // (avoid FindIndex to minimize allocations)
        for (int i = 0; i < slots.Count; ++i)
        {
           Items. ItemSlot slot = slots[i];
            if (slot.amount > 0 && slot.item.data is ScriptableItems.WeaponItem)
                return i;
        }
        return -1;
    }

    // 获取当前装备的武器类别，检查技能是否可以用这个武器施放。
    // 如果没有，则返回 "" 
    public string GetEquippedWeaponCategory()
    {
        int index = GetEquippedWeaponIndex();
        return index != -1 ? ((ScriptableItems.WeaponItem)slots[index].item.data).category : "";
    }
}
}