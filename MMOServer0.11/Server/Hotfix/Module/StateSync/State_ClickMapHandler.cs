using ETModel;
using PF;
using UnityEngine;

namespace ETHotfix
{
	[ActorMessageHandler(AppType.Map)]
	public class State_ClickMapHandler : AMActorLocationHandler<Unit, State_ClickMap>
	{
		protected override async ETTask Run(Unit unit, State_ClickMap message)
		{
			
			Vector3 target = new Vector3(message.X, message.Y, message.Z);
			unit.GetComponent<UnitPathComponent>().MoveTo(target).Coroutine();
			
			await ETTask.CompletedTask;
		}
	}
}