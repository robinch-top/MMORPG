﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace Assets.MMORPG.Scripts.RPGGame.Items
{
    public partial struct Item
    {

        public int id;
        public int durability;

        /// <summary>
        /// Item结构体构造方法
        /// </summary>
        public Item(ScriptableItems.ScriptableItem data)
        {
            id = data.itemID;
            durability = data.maxDurability;
        }

        /// <summary>
        /// 每个道具的各项属性都存在一个ScriptableItem的资源数据实例中
        /// data属性通过item的id从静态属性ScriptableItem.dict中取出他的ScriptableItem实例
        /// 
        /// </summary>
        public ScriptableItems.ScriptableItem data
        {
            get
            {
                if (!ScriptableItems.ScriptableItem.dict.ContainsKey(id))
                    throw new KeyNotFoundException("没有hash=" + id + "的ScriptableItem。");
                return ScriptableItems.ScriptableItem.dict[id];
            }
        }
        ////////////////////////////////////////////////////////////////////////////
        // Item结构体各项属性取值
        // -> 获取ScriptableItem资源数据实例中各项属性值
        public string name => data.name;
        public int maxStack => data.maxStack;
        public int maxDurability => data.maxDurability;
        public long buyPrice => data.buyPrice;
        public long sellPrice => data.sellPrice;
        public long itemMallPrice => data.itemMallPrice;
        public bool sellable => data.sellable;
        public bool tradable => data.tradable;
        public bool destroyable => data.destroyable;
        public Sprite image => data.image;

        public float DurabilityPercent()
        {
            return (durability != 0 && maxDurability != 0) ? (float)durability / (float)maxDurability : 0;
        }

        // 用于检查有耐久道具的耐久性项是否有效
        public bool CheckDurability() =>
            maxDurability == 0 || durability > 0;
        // ToolTip
        public string ToolTip()
        {
            // 执行ScriptableItem资源对象的ToolTip()方法
            StringBuilder tip = new StringBuilder(data.ToolTip());

            // 只有当物品具有耐久性时才显示耐久性
            if (maxDurability > 0)
                tip.Replace("{DURABILITY}", (DurabilityPercent() * 100).ToString("F0"));

            return tip.ToString();
        }
    }
}