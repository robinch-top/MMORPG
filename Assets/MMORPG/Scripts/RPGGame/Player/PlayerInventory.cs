using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.MMORPG.Scripts.RPGGame.Player
{
    public class PlayerInventory : Componet.Inventory
    {

        // 背包、工具条、装备UI中的道具容器格子slots
        // public List<Items.ItemSlot> slots = new List<Items.ItemSlot>();

        [Header("Components")]
        public Entity.Player player;

        [Header("Inventory")]
        public int size = 30;

        public KeyCode[] splitKeys = { KeyCode.LeftShift, KeyCode.RightShift };


        void Awake()
        {
            //默认背包道具
            Data.ItemInfo[] itemsDefault = new Data.ItemInfo[6]{
            new Data.ItemInfo{id = 12001,amount = 30},
            new Data.ItemInfo{id = 12002,amount = 15},
            new Data.ItemInfo{id = 12004,amount = 10},
            new Data.ItemInfo{id = 12005,amount = 6},
            // 背包中添加一件武器装备
            new Data.ItemInfo{id = 20010,amount = 1},
            new Data.ItemInfo{id = 20011,amount = 16}
        };

            LoadPlayerInventory(itemsDefault);
        }

        // LoadPlayerInventory /////////////////////////////////////////////////
        /// -><summary> <para>加载角色的背包道具。</para>
        /// 角色初始时加载角色背包道具数据，生成背包的格子，再添加道具到格子中。</summary>
        public void LoadPlayerInventory(Data.ItemInfo[] itemsInfo)
        {
            // 为背包添加空格
            for (int i = 0; i < player.inventory.size; ++i)
            {
                player.inventory.slots.Add(new Items.ItemSlot());
            }

            // 添加格子中的道具
            for (int k = 0; k < itemsInfo.Length; ++k)
            {
                Data.ItemInfo item = itemsInfo[k];

                if (item.index > 0)
                {
                    // 用带道具的格子替换指定位置的格子
                    Insert(CreateSlot(item.id, item.amount), item.index);
                }
                else
                {
                    // 如果没有指定位置,将道具添加到下一个空格
                    Add(CreateItem(item.id), item.amount);
                }
            }
        }

        // Insert /////////////////////////////////////////////////////////////
        /// -><summary> <para>添加道具到背包的指定格子。</para>
        /// 这个方法只能添加道具到指定的空格,因为出判断指定的格子是不是空格外,并没有更多的判断,
        /// 不过使用Insert的情况,通常是加载背包数据,生成背包中的道具用到,要添加的位置肯定是空格 </summary>
        public void Insert(Items.ItemSlot slot, int index)
        {
            if (player.inventory.slots[index].item.id == 0)
                player.inventory.slots[index] = slot;
            else
                print("背包此格有物品！！");
        }

        // 创建道具格子
        public Items.ItemSlot CreateSlot(int itemID, int amount = 1)
        {
            Items.Item item = new Items.Item(ScriptableItems.ScriptableItem.dict[itemID]);
            return new Items.ItemSlot(item, amount);
        }
        // 创建道具
        public Items.Item CreateItem(int itemID)
        {
            return new Items.Item(ScriptableItems.ScriptableItem.dict[itemID]);
        }

        public bool InventoryOperationsAllowed()
        {
            return player.state == "IDLE" ||
                   player.state == "MOVING" ||
                   player.state == "CASTING";
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

        // CmdInventorySplit ////////////////////////////////////////////////////
        /// -><summary> 在背包中拆分道具堆叠调用的方法。</summary>
        public void CmdInventorySplit(int fromIndex, int toIndex)
        {
            if (InventoryOperationsAllowed() &&
                0 <= fromIndex && fromIndex < slots.Count &&
                0 <= toIndex && toIndex < slots.Count &&
                fromIndex != toIndex)
            {
                Items.ItemSlot slotFrom = slots[fromIndex];
                Items.ItemSlot slotTo = slots[toIndex];
                if (slotFrom.amount >= 2 && slotTo.amount == 0)
                {
                    slotTo = slotFrom; // copy the value

                    slotTo.amount = slotFrom.amount / 2;
                    slotFrom.amount -= slotTo.amount;

                    slots[fromIndex] = slotFrom;
                    slots[toIndex] = slotTo;
                }
            }
        }

        // CmdInventoryMerge ////////////////////////////////////////////////////
        /// -><summary> 在背包中合并道具堆叠调用的方法。</summary>
        public void CmdInventoryMerge(int fromIndex, int toIndex)
        {
            if (InventoryOperationsAllowed() &&
                0 <= fromIndex && fromIndex < slots.Count &&
                0 <= toIndex && toIndex < slots.Count &&
                fromIndex != toIndex)
            {
                Items.ItemSlot slotFrom = slots[fromIndex];
                Items.ItemSlot slotTo = slots[toIndex];
                if (slotFrom.amount > 0 && slotTo.amount > 0)
                {
                    if (slotFrom.item.Equals(slotTo.item))
                    {
                        // merge from -> to
                        int put = slotTo.IncreaseAmount(slotFrom.amount);
                        slotFrom.DecreaseAmount(put);

                        // put back into the list
                        slots[fromIndex] = slotFrom;
                        slots[toIndex] = slotTo;
                    }
                }
            }
        }


        // CmdUseItem /////////////////////////////////////////////////////////////
        /// -><summary> 判断道具是否可用和使用道具的方法。</summary>
        public void CmdUseItem(int index)
        {
            // 判断道具是否可用
            if (InventoryOperationsAllowed() &&
                0 <= index && index < slots.Count && slots[index].amount > 0 &&
                slots[index].item.data is ScriptableItems.UsableItem usable)
            {
                Items.Item item = slots[index].item;
                usable.Use(player, index);
            }
        }


        // drag & drop /////////////////////////////////////////////////////////////
        /// -><summary> 放下拖放道具到格子的事件方法。</summary>
        void OnDragAndDrop_InventorySlot_InventorySlot(int[] slotIndices)
        {
            if (slots[slotIndices[0]].amount > 0 && slots[slotIndices[1]].amount > 0 &&
                slots[slotIndices[0]].item.Equals(slots[slotIndices[1]].item))
            {
                CmdInventoryMerge(slotIndices[0], slotIndices[1]);
            }
            // split?
            else if (Base.Utils.AnyKeyPressed(splitKeys))
            {
                CmdInventorySplit(slotIndices[0], slotIndices[1]);
            }
            // swap?
            else
            {
                CmdSwapInventoryInventory(slotIndices[0], slotIndices[1]);
            }
        }

    }
}