using System;
using System.Text;
using UnityEngine;
namespace Assets.MMORPG.Scripts.RPGGame.Items
{
    /// 格子对象

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
        public ItemSlot(Item item, int amount = 1)
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
    public class SyncListItemSlot : Data.SyncList<ItemSlot> {}
}