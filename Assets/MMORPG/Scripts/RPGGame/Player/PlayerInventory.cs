using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.MMORPG.Scripts.RPGGame.Player
{
    public class PlayerInventory : MonoBehaviour
    {

        // 背包、工具条、装备UI中的道具容器格子slots
        public List<Items.ItemSlot> slots = new List<Items.ItemSlot>();

        [Header("Components")]
        public Entity.Player player;

        [Header("Inventory")]
        public int size = 30;

        public KeyCode[] splitKeys = { KeyCode.LeftShift, KeyCode.RightShift };

        void Awake()
        {
            // 默认背包道具，先注释掉，这样角色背包中暂时是没有道具的。    
            // 之前课时，你应该创建了几种药剂道具的资源数据对象了，注意他们的编号
            ItemInfo[] itemsDefault = new ItemInfo[4]{
             new ItemInfo{id = 12001,amount = 30},
             new ItemInfo{id = 12002,amount = 15},
             new ItemInfo{id = 12004,amount = 10},
             new ItemInfo{id = 12005,amount = 6}
             };

            LoadPlayerInventory(itemsDefault);
        }

        // LoadPlayerInventory /////////////////////////////////////////////////
        /// -><summary> <para>加载角色的背包道具。</para>
        /// 角色初始时加载角色背包道具数据，生成背包的格子，再添加道具到格子中。</summary>
        public void LoadPlayerInventory(ItemInfo[] itemsInfo)
        {
            // 为背包添加空格
            for (int i = 0; i < player.inventory.size; ++i)
            {
                player.inventory.slots.Add(new Items.ItemSlot());
            }

            // 添加格子中的道具
            for (int k = 0; k < itemsInfo.Length; ++k)
            {
                ItemInfo item = itemsInfo[k];

                // 如果没有指定位置,将道具添加到下一个空格
                Add(new Items.Item(ScriptableItems.ScriptableItem.dict[item.id]), item.amount);
            }
        }

        /***************************************************************************/
        /* 角色背包的一些方法                                                 *                        
        /*=========================================================================*/
        // 计算背包有多少空格
        public int SlotsFree()
        {
            int free = 0;
            foreach (Items.ItemSlot slot in slots)
                if (slot.amount == 0)
                    ++free;
            return free;
        }

        // 计算背包有多少被道具占用的格子
        public int SlotsOccupied()
        {
            int occupied = 0;
            foreach (Items.ItemSlot slot in slots)
                if (slot.amount > 0)
                    ++occupied;
            return occupied;
        }

        // 计算背包中某种道具的总数量
        public int Count(Items.Item item)
        {
            int amount = 0;
            foreach (Items.ItemSlot slot in slots)
                if (slot.amount > 0 && slot.item.Equals(item))
                    amount += slot.amount;
            return amount;
        }

        // 从背包中移除指定数量某种道具
        public bool Remove(Items.Item item, int amount)
        {
            for (int i = 0; i < slots.Count; ++i)
            {
                Items.ItemSlot slot = slots[i];
                if (slot.amount > 0 && slot.item.Equals(item))
                {
                    // 从包含某种道具的格子尽量移除此种道具
                    amount -= slot.DecreaseAmount(amount);
                    slots[i] = slot;

                    // 达到指定数量
                    if (amount == 0) return true;
                }
            }

            // 背包中没有更多此种道具，达不到指定数量
            return false;
        }
        // 检测背包能否放入指定数量的某种道具
        public bool CanAdd(Items.Item item, int amount)
        {
            // 检测背包中每个格子
            for (int i = 0; i < slots.Count; ++i)
            {
                // 此格为空，在此格放此道具的最大堆叠量
                if (slots[i].amount == 0)
                    amount -= item.maxStack;
                // 此格不为空但也是此种道具，在此格放此道具剩余堆叠最
                else if (slots[i].item.Equals(item))
                    amount -= (slots[i].item.maxStack - slots[i].amount);

                // 达到指定数量
                if (amount <= 0) return true;
            }

            // 没有空格与剩余堆叠量放此种道具
            return false;
        }
        // CmdSwapInventoryInventory ////////////////////////////////////////////
        /// -><summary> 在背包中拖放道具,更换道具位置index调用的方法。</summary>
        public void CmdSwapInventoryInventory(int fromIndex, int toIndex)
        {
            if (InventoryOperationsAllowed() &&
                0 <= fromIndex && fromIndex < slots.Count &&
                0 <= toIndex && toIndex < slots.Count &&
                fromIndex != toIndex)
            {
                // swap them
               Items.ItemSlot temp = slots[fromIndex];
                slots[fromIndex] = slots[toIndex];
                slots[toIndex] = temp;
            }
        }
        public bool InventoryOperationsAllowed()
        {
            return false;
        }
        // drag & drop /////////////////////////////////////////////////////////////
        /// -><summary> 放下拖放道具到格子的事件方法。</summary>
        void OnDragAndDrop_InventorySlot_InventorySlot(int[] slotIndices)
        {
            if (slots[slotIndices[0]].amount > 0 && slots[slotIndices[1]].amount >= 0)
            {
                CmdSwapInventoryInventory(slotIndices[0], slotIndices[1]);
            }
        }
        // 增加指定数量某种道具到背包中
        public bool Add(Items.Item item, int amount)
        {
            // 有空格和足够的堆叠量放入指定的数量
            if (CanAdd(item, amount))
            {
                // 尽量放入包含此种道具还有堆叠量的格子
                for (int i = 0; i < slots.Count; ++i)
                {
                    if (slots[i].amount > 0 && slots[i].item.Equals(item))
                    {
                        Items.ItemSlot temp = slots[i];
                        amount -= temp.IncreaseAmount(amount);
                        slots[i] = temp;
                    }

                    // 达到指定数量
                    if (amount <= 0) return true;
                }

                // 还未达到指定数量，寻找空格放入此种道具
                for (int i = 0; i < slots.Count; ++i)
                {
                    if (slots[i].amount == 0)
                    {
                        int add = Mathf.Min(amount, item.maxStack);
                        slots[i] = new Items.ItemSlot(item, add);
                        amount -= add;
                    }

                    // 达到指定数量
                    if (amount <= 0) return true;
                }
                // 不能添加更多此种道具到背包，空间不足
                if (amount != 0) Debug.LogError("背包空间不够！添加道具到背包失败: " + item.name + " " + amount);
            }
            return false;
        }
    }
    [Serializable]
    public struct ItemInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public int durability { get; set; }
        public int index { get; set; }
    }
}