using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class CharacterAwakeSystem : AwakeSystem<Character, string,long>
    {
        public override void Awake(Character self, string name,long useID)
        {
            self.Awake(name,useID);
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class Character : Entity
    {
        public long UserId {get; set;}

        public long CharaId {get;set;}
        //昵称
        public string Name { get; set;}

        //性别
        public string Sex { get; set;}

        //种族
        public string Class {get; set;}

        //职业
        public string Race {get; set;}

        //等级
        public int Level {get; set;}

        //经验
        public long Experience {get; set;}

        //金钱 
        public int Money { get; set; }

        //未读邮件个数
        public int Mail { get; set; }

        //称号头衔
        public string Title { get; set; }

        //地图
        public long Map { get; set; }

        //区域编号
        public long Region { get; set; }

        //坐标 
        public float X;
        public float Y;
        public float Z;

        // 列表位置
        public int Index { get; set; }

        // 装备
        public EquipInfo[] Equipments {get; set; }

        //public List<Ca>
        public void Awake(string name,long userID)
        {
            UserId = userID;
            CharaId = this.Id;
            Name = name;
            Level = 1;
            Experience = 1;
            Sex = "男";
            Race = "";
            Title = "";
            Sex = "";
        }

    }
}