using System.Collections.Generic;

namespace ETModel
{
    /// <summary>
    /// 地图房间组件，对场景地图房间，副本房间，战场房间，战役房间，情景房间，队伍房间
    /// </summary>
    public class RoomComponent : Component
    {
        /// <summary>
        /// 所有游戏中的大地图房间
        /// </summary>
        public readonly Dictionary<long, Room> mapRooms = new Dictionary<long, Room>();

        /// <summary>
        /// 所有游戏中的副本房间
        /// </summary>
        public readonly Dictionary<long, Instance> copyRooms = new Dictionary<long, Instance>();

        /// <summary>
        /// 所有pvp战场房间
        /// </summary>
        //public readonly Dictionary<long, PVPRoom> pvpRooms = new Dictionary<long, PVPRoom>();

        /// <summary>
        /// 所有战役房间
        /// </summary>
        //public readonly Dictionary<long, BattleRoom> battleRooms = new Dictionary<long, BattleRoom>();

        /// <summary>
        /// 匹配中的玩家队列
        /// </summary>
        public readonly Queue<Gamer> pvpQueue = new Queue<Gamer>();
        public readonly Queue<Gamer> battleQueue = new Queue<Gamer>();

        public readonly Queue<long> worldMapQueue = new Queue<long>();
    }
}
