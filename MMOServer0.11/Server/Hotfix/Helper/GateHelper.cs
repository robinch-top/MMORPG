using ETModel;
using System.Net;

namespace ETHotfix
{
    public static class GateHelper
    {
        /// <summary>
        /// 验证Session是否绑定了玩家
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static bool SignSession(Session session)
        {
            SessionUserComponent sessionUser = session.GetComponent<SessionUserComponent>();
            if (sessionUser == null || Game.Scene.GetComponent<UserComponent>().Get(sessionUser.User.UserId) == null)
            {
                return false;
            }
            return true;
        } 

        /// <summary>
        /// 获取Map 配置信息
        /// </summary>
        /// <returns></returns>
        public static RoomConfig GetMapConfig(long roomId)
        {
            RoomConfig config = new RoomConfig();

            // 查询数据库 此区域地图配置信息，这里暂时没实现，用写死的数据
            config.roomNmae = "黎明镇";
            config.roomId = roomId;
            config.BaseLevel = 5;
            config.MinLevel = 1;
            config.MaxLevel = 10;
            config.removable = false;
            config.reloadMapScene = 1001;
            config.Multiples = 1;
            config.maps = new long[3]{2001,2002,2003};
            config.minNumber = 0; //0为不限最大人数
            config.maxNumber = 0; //0为不限最少人数
            return config;
        }


        public static CharacterInfo CharacterInfoByData(Character character,bool hasEquip = true){
            CharacterInfo info = new CharacterInfo(){
                Name = character.Name,
                UserId = character.UserId,
                CharaId = character.CharaId,
                Class = character.Class,
                Level = character.Level,
                Experience = character.Experience,
                Money = character.Money,
                Mail = character.Mail,
                Title = character.Title,
                Map = character.Map,
                Region = character.Region,
                X = character.X,
                Y = character.Y,
                Z = character.Z,
                Index = character.Index
            };
            if(hasEquip) info.Equipments = To.RepeatedField<EquipInfo>(character.Equipments);
            return info;
        }
        

        /// <summary>
        /// 获取Map服务器的Session
        /// </summary>
        /// <returns></returns>
        public static Session GetMapSession()
        {
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            IPEndPoint mapIPEndPoint = config.MapConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
            Log.Debug(mapIPEndPoint.ToString());
            Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapIPEndPoint);
            return mapSession;
        }

    }
}