using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInventory))]
public class PlayerEquipment : Equipment
{
    [Header("Components")]
    public Player player;
    public Animator animator;
    public PlayerInventory inventory;

    [Header("Equipment Info")]
    public EquipSlotInfo[] slotInfo;

    Dictionary<string, Transform> skinBones = new Dictionary<string, Transform>();

    void Awake()
    {
        // 遍历缓存骨骼数据
        foreach (SkinnedMeshRenderer skin in GetComponentsInChildren<SkinnedMeshRenderer>())
            foreach (Transform bone in skin.bones)
                skinBones[bone.name] = bone;
    }

    void Start()
    {
        slots.Callback += OnEquipmentChanged;

        // 初始刷新一次装备部件的骨骼蒙皮与动画
        for (int i = 0; i < slots.Count; ++i)
            RefreshLocation(i);
        // 初始刷新一次装备模型
        // ...
    }
    /// 刷新装备部件的骨骼蒙皮与动画
    public void RefreshLocation(int index)
    {
        ItemSlot slot = slots[index];
        EquipSlotInfo info = slotInfo[index];

        // 有效部件类别和有效位置
        if (info.requiredCategory != "" && info.location != null)
        {
            // 装备模型在info.location下面，刷新，需要清除之前的装备部件
            if (info.location.childCount > 0) Destroy(info.location.GetChild(0).gameObject);

            if (slot.amount > 0)
            {
                // 有装备模型，则进行设置
                EquipmentItem itemData = (EquipmentItem)slot.item.data;
                if (itemData.modelPrefab != null)
                {
                    // 加载模型半作为 info.location 的子物体
                    GameObject go = Instantiate(itemData.modelPrefab, info.location, false);
                    go.name = itemData.modelPrefab.name;

                    BonesAndRebind(go);

                    // 多模型组合成的装备部件，把下面的子物体模型也进行BonesAndRebind
                    if (go.GetComponentsInChildren<Transform>().Length >= 1)
                    {
                        foreach (Transform child in go.transform)
                        {
                            BonesAndRebind(child.gameObject);
                        }
                    }
                }
            }
        }
    }
    void BonesAndRebind(GameObject go)
    {
        // 生个模型都带有骨骼绑定数据，更换为此模型部件的骨骼数据
        SkinnedMeshRenderer equipmentSkin = go.GetComponentInChildren<SkinnedMeshRenderer>();
        if (equipmentSkin != null && CanReplaceAllBones(equipmentSkin))
            ReplaceAllBones(equipmentSkin);

        // 重新绑定骨骼与模型网格蒙皮动画
        Animator anim = go.GetComponent<Animator>();
        if (anim != null)
        {
            anim.runtimeAnimatorController = animator.runtimeAnimatorController;
            RebindAnimators();
        }
    }
    bool CanReplaceAllBones(SkinnedMeshRenderer equipmentSkin)
    {
        foreach (Transform bone in equipmentSkin.bones)
            if (!skinBones.ContainsKey(bone.name))
                return false;
        return true;
    }


    void ReplaceAllBones(SkinnedMeshRenderer equipmentSkin)
    {
        // 获取装备骨骼
        Transform[] bones = equipmentSkin.bones;

        // 更换每一个
        for (int i = 0; i < bones.Length; ++i)
        {
            string boneName = bones[i].name;
            if (!skinBones.TryGetValue(boneName, out bones[i]))
                Debug.LogWarning(equipmentSkin.name + " bone " + boneName + " not found in original player bones. Make sure to check CanReplaceAllBones before.");
        }

        // 重新指定骨骼
        equipmentSkin.bones = bones;
    }

    void RebindAnimators()
    {
        foreach (Animator anim in GetComponentsInChildren<Animator>())
            anim.Rebind();
    }
    // OnEquipmentChanged /////////////////////////////////////////////////////////////
    // -><summary> 角色装备变化的回调方法。 </summary>
    void OnEquipmentChanged(SyncListItemSlot.Operation op, int index, ItemSlot oldSlot, ItemSlot newSlot)
    {
        ScriptableItem oldItem = oldSlot.amount > 0 ? oldSlot.item.data : null;
        ScriptableItem newItem = newSlot.amount > 0 ? newSlot.item.data : null;
        if (oldItem != newItem)
        {
            // 更新装备模型
            RefreshLocation(index);
        }
    }
    // 加载角色装备
    public void LoadPlayerEquipped(EquipInfo[] datas)
    {
        equipments = new ItemSC[datas.Length];

        // 初始化角色装备ItemSC
        for (int k = 0; k < datas.Length; ++k)
        {
            ItemSC itemEquip;
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
            EquipSlotInfo info = player.equipment.slotInfo[i];
            ItemSC itemEquip = GetItemEquip(info.requiredCategory);
            player.equipment.slots.Add(itemEquip.item != null ?
                new ItemSlot(new Item(itemEquip.item), itemEquip.amount) : new ItemSlot());
        }
    }

    // 设置指定部位装备属性
    public void SetEquipment(string type, EquipInfo data)
    {
        EquipmentItem item = GetItemEquip(type).item as EquipmentItem;
        if (data.endurance > 0) item.endurance = data.endurance;
        if (data.intellect > 0) item.intellect = data.intellect;
        if (data.agility > 0) item.agility = data.agility;
        if (data.strength > 0) item.strength = data.strength;
        if (data.spirit > 0) item.spirit = data.spirit;
        if (data.physical > 0) item.physical = data.physical;
    }

    // 设置指定部位装备的耐久属性
    public void SetEquipmentDurability(string type, int val)
    {
        int index = GetEquipmentIndex(type);
        ItemSlot itemSlot = slots[index];

        // 结构体属性发生改变
        itemSlot.item.durability = val;
        // 要更新到他所在的数据容器，结构体是值类型而不是引用类型。
        slots[index] = itemSlot;
    }

    // 获取指定部位装备index
    public int GetEquipmentIndex(string type)
    {
        int index = -1;
        for (int i = 0; i < slots.Count; ++i)
        {
            EquipmentItem item = slots[i].item.data as EquipmentItem;
            if (item.category == type)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    // 创建指定部位ItemSC
    public ItemSC CreateItemEquip(string type, int itemID = 0, int amount = 1)
    {
        ItemSC itemEquip = new ItemSC();
        if (itemID > 0)
            itemEquip.item = ScriptableItem.dict[itemID];
        itemEquip.amount = amount;
        itemEquip.type = type;
        return itemEquip;
    }

    // 获取指定部位ItemSC
    public ItemSC GetItemEquip(string type)
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

    public bool InventoryOperationsAllowed()
    {
        return player.state == "IDLE" ||
               player.state == "MOVING" ||
               player.state == "CASTING";
    }

    // CmdUseItem /////////////////////////////////////////////////////////////
    /// -><summary> 判断装备是否可用和使用装备的方法。</summary>
    public void CmdUseItem(int index)
    {
        // 判断装备道具是否可用
        if (InventoryOperationsAllowed() &&
            0 <= index && index < slots.Count && slots[index].amount > 0 &&
            slots[index].item.data is UsableItem usable)
        {
            Item item = slots[index].item;
            usable.Use(player, index);
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


    // SwapInventoryEquip ///////////////////////////////////////////////////////
    /// -><summary> 从背包到装备界面交互装备道具。</summary>
    public void SwapInventoryEquip(int inventoryIndex, int equipmentIndex)
    {
        if (inventory.InventoryOperationsAllowed() &&
            0 <= inventoryIndex && inventoryIndex < inventory.slots.Count &&
            0 <= equipmentIndex && equipmentIndex < slots.Count)
        {
            // 格子必须是空的或道具是可装备的
            ItemSlot slot = inventory.slots[inventoryIndex];
            if (slot.amount == 0 ||
                slot.item.data is EquipmentItem itemData &&
                itemData.CanEquip(player, inventoryIndex, equipmentIndex))
            {
                // 交换
                ItemSlot temp = slots[equipmentIndex];
                slots[equipmentIndex] = slot;
                inventory.slots[inventoryIndex] = temp;
            }
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
            ItemSlot slotFrom = inventory.slots[inventoryIndex];
            ItemSlot slotTo = slots[equipmentIndex];
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
            ItemSlot slotFrom = slots[equipmentIndex];
            ItemSlot slotTo = inventory.slots[inventoryIndex];
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

}


