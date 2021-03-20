using System.Collections.Generic;
using System.Threading;

namespace ETModel
{
    [ObjectSystem]
    public class InstanceAwakeSystem : AwakeSystem<Instance,RoomConfig>
    {
        public override void Awake(Instance self, RoomConfig config)
        {
            self.Awake(config);
        }
    }
    public sealed class Instance : Room
    {

        public override void Awake(RoomConfig config)
        {
            this.config = config;
            this.roomId = config.roomId;
        }

        
    }
}
