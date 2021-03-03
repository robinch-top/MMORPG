using System;
using UnityEngine;

namespace Assets.MMORPG.Scripts.RPGGame.Data
{
    [Serializable]
    public struct Character
    {
        public long roleID { get; set; }
        public string name { get; set; }
        public string account { get; set; }
        public string className { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public int level { get; set; }
        public int health { get; set; }
        public int mana { get; set; }
        public long experience { get; set; }
        public long skillExperience { get; set; }
        public long gold { get; set; }
        public bool gamemaster { get; set; }
        public bool online { get; set; }
        public DateTime lastsaved { get; set; }
        public int index { get; set; }
    }
    [Serializable]
    public struct Account
    {
        public long accountID { get; set; }
        public string password { get; set; }
    }

    // 道具信息结构
    [Serializable]
    public struct ItemInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public int amount { get; set; }
        public int durability { get; set; }
        public int index { get; set; }
    }

    // 装备信息结构
    [Serializable]
    public struct EquipInfo
    {
        public string CatType { get; set; }
        public int itemID { get; set; }
        public int amount { get; set; }
        public int endurance { get; set; }
        public int intellect { get; set; }

        public int strength { get; set; }
        public int agility { get; set; }
        public int spirit { get; set; }
        public int physical { get; set; }
    }
    // 装备格子信息结构
    [Serializable]
    public partial struct EquipSlotInfo
    {
        public string requiredCategory;
        public string catName;
        public Transform location;
    }
    // 技能信息结构
    [Serializable]
    public struct SkillInfo
    {
        public string name { get; set; }
        public int skillID { get; set; }
        public float distance { get; set; }
        public int level { get; set; }
        public float castTime { get; set; }
        public float cooldown { get; set; }
    }
}