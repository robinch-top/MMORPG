using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
namespace ETModel
{
    /// <summary>
    /// 账号信息
    /// </summary>
    [BsonIgnoreExtraElements]
    public class GlobalInfo : Component
    {
        public ObjectId _id { get; set; }
        //类型
        public string Type { get; set; }

        // item
        public EquipInfo[] Equipments { get; set; }
    }
}