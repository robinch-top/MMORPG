using System.Collections.Generic;
using System.Threading;

namespace ETModel
{
    /// <summary>
    /// 房间配置
    /// </summary>
    public struct RoomConfig
    {
        /// 经验倍率
        public int Multiples { get; set; }

        public int BaseLevel { get; set; }

        public int MinLevel { get; set; }

        public int MaxLevel { get; set; }

        public long roomId { get; set;}

        public string roomNmae {get;set;}

        public long[] maps {get;set;}
        
        // 可清除属性，决定了在服务器上是常规地图房间不可清除，
        // 或是需要在用完清除的任务场景，副本，剧情场景地图
        public bool removable {get;set;}

        // 大于0需要前端载入新的地图场景,=0在现有前端场景
        // 有些任务场景,剧情场景是在现有前端场景地图进行的,但是服务器上是另一个room房间了.
        public long reloadMapScene {get;set;}

        public long minNumber {get;set;}
        public long maxNumber {get;set;}
    }

    /// <summary>
    /// Room组件 表示一个大地图区域，与更小的地图范围关系如下
    /// 地图单位：MapArea-> City/town/stronghold -> Landmark/building
    /// 地标：Landmark: Mountain,river, lake, sea, pool, marsh, hilltop,road,Cave,block

    /// 最小地图单位: mapArea,city,town,stronghold 用地图数据判断进入。这也是有独立地图图纸的最小地图单位
    /// 最小地图单位有Enity对象，包括平均分布的9个参考坐标。这样只要玩家在任何一个最小单位地图的任何坐标位置
    /// 服务器能较小代价的计算出离他最近的参考坐标，从来查询附近一定范围的所有地标

    /// landmark,building 用坐标范围判断接近
    /// 重要building: 地铁，码头，飞艇区，传送区（地图数据识别，非坐标范围判断）
    /// </summary>
    [ObjectSystem]
    public class RoomAwakeSystem : AwakeSystem<Room,RoomConfig>
    {
        public override void Awake(Room self, RoomConfig config)
        {
            self.Awake(config);
        }
    }
    public class Room : Entity
    {
        public RoomConfig config;

        public long roomId;
        
        /// 当前房间的所有所有玩家 空位为null
        public Dictionary<long,Gamer> gamers= new Dictionary<long, Gamer>();
        
        /// 房间中玩家的数量
        public int Count { get { return gamers.Count; } }

        //清房间waiting的cts
        public CancellationTokenSource CancellationTokenSource;

        public virtual void Awake(RoomConfig config)
        {
            this.config = config;
            this.roomId = config.roomId;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            for (int i = 0; i < gamers.Count; i++)
            {
                if (gamers[i] != null)
                {
                    gamers[i].Dispose();
                    gamers[i] = null;
                }
            }
        }
    }
}
