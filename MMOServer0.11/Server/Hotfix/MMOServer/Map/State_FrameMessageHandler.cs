using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ActorMessageHandler(AppType.Map)]
	public class State_FrameMessageHandler : AMActorLocationHandler<Unit, State_FrameMessage>
	{
		
		protected override async ETTask Run(Unit unit, State_FrameMessage message)
		{
			MoveState state = (MoveState)message.MoveInfo.State; // proto enum 转 c# enum
			byte route = message.MoveInfo.Route.ToByteArray()[0]; // bytestring 转 C# byte
			Vector3 p = new Vector3(message.MoveInfo.X,message.MoveInfo.Y,message.MoveInfo.Z);

			Move move = new Move(route, state,0, p, message.MoveInfo.YRotation);
			unit.GetComponent<UnitStateComponent>().SyncStateFrame(move);

			await ETTask.CompletedTask;
		}

		
	}
}