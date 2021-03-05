using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.MMORPG.Scripts.RPGGame.Player
{
    [Serializable]
    public struct SkillbarEntry
    {
        public string reference;
        public KeyCode hotKey;
    }

    [RequireComponent(typeof(PlayerEquipment))]
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(PlayerSkills))]
    public class PlayerSkillbar : MonoBehaviour
    {
        [Header("Components")]
        public Entity.Player player;
        public PlayerEquipment equipment;
        public PlayerInventory inventory;
        public PlayerSkills skills;


        /// <summary>
        /// 给工具条格子绑定键位
        /// </summary>
        public SkillbarEntry[] slots =
        {
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha1},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha2},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha3},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha4},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha5},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha6},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha7},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha8},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha9},
        new SkillbarEntry{reference="", hotKey=KeyCode.Alpha0},
    };
        public void Start()
        {
            // 从本地或者服务端加载工具条数据
            // 如果从服务端加载数据，可以加一个信息数据作为参数传入Load方法
            Load();
        }


        void OnDestroy()
        {
            if (Entity.Player.localPlayer)
                Save();
        }

        // skillbar ////////////////////////////////////////////////////////////////
        // 工具条的数据都是保存到本地，
        // 只有退出游戏时，或者加一个方法每分钟提交一次到服务端，现在还没来实现这个
        void Save()
        {
            // 保存 skillbar到PlayerPrefs
            // 每个角色都有自己的工具条数据
            for (int i = 0; i < slots.Length; ++i)
                PlayerPrefs.SetString(name + "_skillbar_" + i, slots[i].reference);

            PlayerPrefs.Save();
        }
        void Load()
        {
            print("loading skillbar for " + name);
            List<Skill.Skill> learned = player.skills.skills.Where(skill => skill.level > 0).ToList();
            for (int i = 0; i < slots.Length; ++i)
            {
                // 如果有保存的技能数据，加载数据到工具条
                if (PlayerPrefs.HasKey(name + "_skillbar_" + i))
                {
                    string entry = PlayerPrefs.GetString(name + "_skillbar_" + i, "");

                    if (skills.HasLearned(entry) ||
                        inventory.GetItemIndexByName(entry) != -1 ||
                        equipment.GetItemIndexByName(entry) != -1)
                    {
                        slots[i].reference = entry;
                    }
                }
                // 如果工具条上没有拖技能进来，就用默认学会的技能到工具条
                else if (i < learned.Count)
                {
                    slots[i].reference = learned[i].name;
                }
            }
        }
        // drag & drop /////////////////////////////////////////////////////////////
        void OnDragAndDrop_InventorySlot_SkillbarSlot(int[] slotIndices)
        {
            slots[slotIndices[1]].reference = inventory.slots[slotIndices[0]].item.name;
        }

        void OnDragAndDrop_EquipmentSlot_SkillbarSlot(int[] slotIndices)
        {
            slots[slotIndices[1]].reference = equipment.slots[slotIndices[0]].item.name;
        }

        void OnDragAndDrop_SkillsSlot_SkillbarSlot(int[] slotIndices)
        {
            slots[slotIndices[1]].reference = player.skills.skills[slotIndices[0]].name;
        }

        void OnDragAndDrop_SkillbarSlot_SkillbarSlot(int[] slotIndices)
        {
            string temp = slots[slotIndices[0]].reference;
            slots[slotIndices[0]].reference = slots[slotIndices[1]].reference;
            slots[slotIndices[1]].reference = temp;
        }


        void OnDragAndClear_SkillbarSlot(int slotIndex)
        {
            slots[slotIndex].reference = "";
        }
    }
}
