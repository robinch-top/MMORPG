using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.MMORPG.Scripts.RPGGame.Player
{
    public class PlayerEquipment : Componet.Equipment
    {
        [Header("Components")]
        public Entity.Player player;
        public Animator animator;
        public PlayerInventory inventory;

        [Header("Equipment Info")]
        public Data.EquipSlotInfo[] slotInfo;

        Dictionary<string, Transform> skinBones = new Dictionary<string, Transform>();

        void Awake()
        {
            // 暂时写死一份战士装备信息数据在此
            // 这里没有提供的其它属性的数据,就会使用装备资源数据对象中设置的值
            Data.EquipInfo[] warriorEquipDatas = new Data.EquipInfo[8]{
            new Data.EquipInfo{CatType="Shoulders",itemID=20002,amount=1,
                endurance=5,strength=4,physical=3,intellect=2,agility=4},
            new Data.EquipInfo{CatType="Chest",itemID=20003,amount=1,
                endurance=10,strength=6,physical=1,intellect=5,agility=3},
            new Data.EquipInfo{CatType="Feet",itemID=20007,amount=1,
                physical=1,intellect=12,agility=3},
            new Data.EquipInfo{CatType="Weapon",itemID=20004,amount=1,
                endurance=2,strength=15,physical=9,spirit=6,agility=8},
            new Data.EquipInfo{CatType="Legs",itemID=20006,amount=1,
                endurance=5,strength=2,physical=2},
            new Data.EquipInfo{CatType="Hands",itemID=20005,amount=1,
                endurance=15,strength=9,physical=8,intellect=0,agility=4},
            new Data.EquipInfo{CatType="Head",itemID=20001,amount=1,
                endurance=8,strength=7,physical=4,spirit=2,agility=4},
            new Data.EquipInfo{CatType="Ammo",itemID=20011,amount=23,
                endurance=12,strength=5}
            };

            LoadPlayerEquipped(warriorEquipDatas);
        }
        void Start()
        {
            slots.Callback += OnEquipmentChanged;

            // 初始刷新一次装备模型
            // ...
        }

        // 加载角色装备
        public void LoadPlayerEquipped(Data.EquipInfo[] datas)
        {
            equipments = new Items.ItemSC[datas.Length];

            // 初始化角色装备ItemSC
            for (int k = 0; k < datas.Length; ++k)
            {
                Items.ItemSC itemEquip;
                if (datas[k].itemID > 0)
                {
                    itemEquip = CreateItemEquip(datas[k].CatType, datas[k].itemID, datas[k].amount);
                    equipments.SetValue(itemEquip, k);
                    SetEquipment(datas[k].CatType, datas[k]);
                }
                else
                {
                    itemEquip = CreateItemEquip(datas[k].CatType);
                    equipments.SetValue(itemEquip, k);
                }
            }

            // 为装备界面添加空格或者已有装备格子
            for (int i = 0; i < player.equipment.slotInfo.Length; ++i)
            {
                Data.EquipSlotInfo info = player.equipment.slotInfo[i];
                Items.ItemSC itemEquip = GetItemEquip(info.requiredCategory);
                player.equipment.slots.Add(itemEquip.item != null ?
                    new Items.ItemSlot(new Items.Item(itemEquip.item), itemEquip.amount) : new Items.ItemSlot());
            }
        }
        // 设置指定部位装备属性
        public void SetEquipment(string type, Data.EquipInfo data)
        {
            ScriptableItems.EquipmentItem item = GetItemEquip(type).item as ScriptableItems.EquipmentItem;
            if (data.endurance > 0) item.endurance = data.endurance;
            if (data.intellect > 0) item.intellect = data.intellect;
            if (data.agility > 0) item.agility = data.agility;
            if (data.strength > 0) item.strength = data.strength;
            if (data.spirit > 0) item.spirit = data.spirit;
            if (data.physical > 0) item.physical = data.physical;
        }

        // 创建指定部位ItemSC
        public Items.ItemSC CreateItemEquip(string type, int itemID = 0, int amount = 1)
        {
            Items.ItemSC itemEquip = new Items.ItemSC();
            if (itemID > 0)
                itemEquip.item = ScriptableItems.ScriptableItem.dict[itemID];
            itemEquip.amount = amount;
            itemEquip.type = type;
            return itemEquip;
        }

        // 获取指定部位ItemSC
        public Items.ItemSC GetItemEquip(string type)
        {
            int index = -1;
            for (int i = 0; i < equipments.Length; ++i)
            {
                if (equipments[i].type == type)
                {
                    index = i;
                    break;
                }
            }
            return equipments[index];
        }
        // OnEquipmentChanged /////////////////////////////////////////////////////////////
        // -><summary> 角色装备变化的回调方法。 </summary>
        void OnEquipmentChanged(Items.SyncListItemSlot.Operation op, int index, Items.ItemSlot oldSlot, Items.ItemSlot newSlot)
        {
            ScriptableItems.ScriptableItem oldItem = oldSlot.amount > 0 ? oldSlot.item.data : null;
            ScriptableItems.ScriptableItem newItem = newSlot.amount > 0 ? newSlot.item.data : null;
            if (oldItem != newItem)
            {
                // 更新装备模型
                // ...
            }
        }
        // drag & drop /////////////////////////////////////////////////////////////
        /// -><summary> 从背包拖放装备到装备格子的事件方法。</summary>
        void OnDragAndDrop_InventorySlot_EquipmentSlot(int[] slotIndices)
        {
            // 合并可堆叠装备到装备格子或从背包到装备格子交换装备
            if (inventory.slots[slotIndices[0]].amount > 0 && slots[slotIndices[1]].amount > 0
                && inventory.slots[slotIndices[0]].item.maxStack > 1
                && inventory.slots[slotIndices[0]].item.Equals(slots[slotIndices[1]].item))
            {
                MergeInventoryEquip(slotIndices[0], slotIndices[1]);
            }
            // swap?
            else
            {
                SwapInventoryEquip(slotIndices[0], slotIndices[1]);
            }

        }
        /// -><summary> 从装备界面拖放装备到背包格子的事件方法。</summary>
        void OnDragAndDrop_EquipmentSlot_InventorySlot(int[] slotIndices)
        {
            // 合并可堆叠装备到背包或从装备格子到背包交换装备
            if (slots[slotIndices[0]].amount > 0 && inventory.slots[slotIndices[1]].amount > 0
                && slots[slotIndices[0]].item.maxStack > 1
                && slots[slotIndices[0]].item.Equals(inventory.slots[slotIndices[1]].item))
            {
                MergeEquipInventory(slotIndices[0], slotIndices[1]);
            }
            // swap?
            else
            {
                SwapInventoryEquip(slotIndices[1], slotIndices[0]);
            }
        }
        // MergeInventoryEquip //////////////////////////////////////////////////////
        /// -><summary> 合并堆叠装备道具到装备界面格子。</summary>
        public void MergeInventoryEquip(int inventoryIndex, int equipmentIndex)
        {
            if (inventory.InventoryOperationsAllowed() &&
                0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
                0 <= equipmentIndex && equipmentIndex < slots.Count)
            {
                Items.ItemSlot slotFrom = inventory.slots[inventoryIndex];
                Items.ItemSlot slotTo = slots[equipmentIndex];
                if (slotFrom.amount > 0 && slotTo.amount > 0)
                {
                    if (slotFrom.item.Equals(slotTo.item))
                    {
                        // 尽可能的放入堆叠数量
                        int put = slotTo.IncreaseAmount(slotFrom.amount);
                        slotFrom.DecreaseAmount(put);

                        // 更新数据容器 list
                        inventory.slots[inventoryIndex] = slotFrom;
                        slots[equipmentIndex] = slotTo;
                    }
                }
            }
        }
        // MergeEquipInventory ////////////////////////////////////////////////////////
        /// -><summary> 合并堆叠装备道具到背包。</summary>
        public void MergeEquipInventory(int equipmentIndex, int inventoryIndex)
        {
            if (inventory.InventoryOperationsAllowed() &&
                0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
                0 <= equipmentIndex && equipmentIndex < slots.Count)
            {
                Items.ItemSlot slotFrom = slots[equipmentIndex];
                Items.ItemSlot slotTo = inventory.slots[inventoryIndex];
                if (slotFrom.amount > 0 && slotTo.amount > 0)
                {
                    // 确定是同样的类型
                    if (slotFrom.item.Equals(slotTo.item))
                    {
                        // 尽可能的放入堆叠数量
                        int put = slotTo.IncreaseAmount(slotFrom.amount);
                        slotFrom.DecreaseAmount(put);

                        // 更新数据容器 list
                        slots[equipmentIndex] = slotFrom;
                        inventory.slots[inventoryIndex] = slotTo;
                    }
                }
            }
        }

        // SwapInventoryEquip ///////////////////////////////////////////////////////
        /// -><summary> 从背包到装备界面交互装备道具。</summary>
        public void SwapInventoryEquip(int inventoryIndex, int equipmentIndex)
        {
            if (inventory.InventoryOperationsAllowed() &&
                0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
                0 <= equipmentIndex && equipmentIndex < slots.Count)
            {
                // 装备格子必须是空的或可装备的
                Items.ItemSlot slot = inventory.slots[inventoryIndex];
                if (slot.amount == 0 ||
                    slot.item.data is ScriptableItems.EquipmentItem itemData &&
                    itemData.CanEquip(player, inventoryIndex, equipmentIndex))
                {
                    // 交换
                    Items.ItemSlot temp = slots[equipmentIndex];
                    slots[equipmentIndex] = slot;
                    inventory.slots[inventoryIndex] = temp;
                }
            }
        }
        /// <summary>
        /// 设置指定部位装备的耐久属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        public void SetEquipmentDurability(string type, int val)
        {
            int index = GetEquipmentIndex(type);
            Items.ItemSlot itemSlot = slots[index];

            // 结构体属性发生改变
            itemSlot.item.durability = val;
            // 要更新到他所在的数据容器，结构体是值类型而不是引用类型。
            slots[index] = itemSlot;
        }

        /// <summary>
        /// 获取指定部位装备index
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetEquipmentIndex(string type)
        {
            int index = -1;
            for (int i = 0; i < slots.Count; ++i)
            {
                ScriptableItems.EquipmentItem item = slots[i].item.data as ScriptableItems.EquipmentItem;
                if (item.category == type)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }
}