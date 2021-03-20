using ETModel;
using System.Net;
using Google.Protobuf;

namespace ETHotfix
{
    public static class MapHelper
    {
        public static Session GetGateSession()
        {
            StartConfigComponent config = Game.Scene.GetComponent<StartConfigComponent>();
            IPEndPoint gateIPEndPoint = config.GateConfigs[0].GetComponent<InnerConfig>().IPEndPoint;
            //Log.Debug(gateIPEndPoint.ToString());
            Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(gateIPEndPoint);
            return gateSession;
        }

        public static void BroadcastMove(Move move,Unit unit){
            Sync_MoveMessage message = new Sync_MoveMessage{
                UnitId = unit.Id,
                MoveInfo = new MoveInfo{
                    UnitId = unit.Id,
                    Route = ByteString.CopyFrom(move.route), // 转为proto bytestring
                    YRotation = move.yRotation,
                    State = (StateInfo)move.state, // 转为proto enum
                    Frame = move.frame,
                    X = move.position.x,
                    Y = move.position.y,
                    Z = move.position.z
                }
            };
            unit.room.Broadcast(message);
        }

    }
}