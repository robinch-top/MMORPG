using UnityEngine;
using System.Collections;
namespace Assets.MMORPG.Scripts.RPGGame.Componet
{
    // 基本的背包组件
    // 主要是添加与移除道具的方法，判断是否能添加指定堆叠数据的道具到背包中
    // 背包中有多少空格，多少格子被占用

    // 怪物的背包不需要更多功能，直接使用的就是这个类
    // 角色的背包需要更多功能使用PlayerInvertory继承这个类

    [DisallowMultipleComponent]
    public class Inventory : Items.ItemContainer
    {
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
}
