using System.Collections.Generic;
using UnityEngine;


namespace Assets.MMORPG.Scripts.RPGGame.Items
{
    /// <summary>
    /// 道具容器组件，主要是持有道具的数据容器slots
    /// 通过itemName查找道具在数据容器slots中的位置
    /// 对有耐久类型道具，耐久度的处理方法
    /// </summary>
    public abstract class ItemContainer : MonoBehaviour
    {
        /// <summary>
        /// 背包、工具条、装备UI中的道具容器格子slots
        /// </summary>
        public SyncListItemSlot slots = new SyncListItemSlot();

        /// <summary>
        /// 通过itemName查找道具在容器中的位置
        /// </summary>
        /// <param name="itemName">道具名称</param>
        /// <returns></returns>
        public int GetItemIndexByName(string itemName)
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                ItemSlot slot = slots[i];
                if (slot.amount > 0 && slot.item.name == itemName)
                    return i;
            }
            return -1;
        }

        // durability //////////////////////////////////////////////////////////////
        /// <summary>
        /// 计算有耐久类型的道具总缺失耐久度
        /// </summary>
        /// <returns>总缺失耐久度</returns>
        public int GetTotalMissingDurability()
        {
            int total = 0;
            foreach (ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.data.maxDurability > 0)
                    total += slot.item.data.maxDurability - slot.item.durability;
            return total;
        }

        /// <summary>
        /// 修复有耐久类型的道具耐久度
        /// </summary>
        public void RepairAllItems()
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                if (slots[i].amount > 0)
                {
                    ItemSlot slot = slots[i];
                    slot.item.durability = slot.item.maxDurability;
                    slots[i] = slot;
                }
            }
        }
    }
}