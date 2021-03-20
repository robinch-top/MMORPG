using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class UserInfoAwakeSystem : AwakeSystem<UserInfo, string>
    {
        public override void Awake(UserInfo self, string name)
        {
            self.Awake(name);
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : Entity
    {
        //昵称
        public string UserName { get; set; }

        //电话
        public long Phone { get; set; }

        //邮箱
        public string Email { get; set; }

        //性别
        public string Sex { get; set; }

        //称号头衔
        public string Title { get; set; }

        public int LastPlay { get; set; }

        //public List<Ca>
        public void Awake(string name)
        {
            UserName = name;
            Title = "";
            Email = "";
            Sex = "";
            LastPlay = 0;
        }

    }
}