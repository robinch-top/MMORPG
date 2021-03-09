using UnityEngine;
using ETModel;
using Mirror;
namespace MMOGame
{
	[MessageHandler]
	public class M2C_CreateUnitsHandler : AMHandler<CreateUnits_M2C>
	{
		protected override async ETTask Run(ETModel.Session session, CreateUnits_M2C message)
		{	
			UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();

			foreach (UnitInfo unitInfo in message.Units)
			{
				if (unitComponent.Get(unitInfo.UnitId) != null)
				{
					continue;
				}

				// 获取角色信息
				CharacterMessage_G2C charaMessage = (CharacterMessage_G2C)await Net.Gate.Call(
					new GetCharacter_C2G(){
						UserId = unitInfo.UserId,
						CharaId = unitInfo.CharaId
					}
				);

				Character chara = GateHelper.CharacterByData(charaMessage.Character);
				GameObject go = Manager.RPG.CreatePlayer((Character)chara);
				
				Unit unit = UnitFactory.Create(unitInfo.UnitId,go);
				unit.UserId = unitInfo.UserId;

				// 调用本地角色进入场景方法
				if(GamerComponent.Instance.MyUser.UserId == unitInfo.UserId){
					UnitComponent.Instance.MyUnit = unit;
					Manager.RPG.LoadCharacterToScene(go);
				}else{
					Manager.RPG.LoadOthersToScene(go);
				}

			}
			
			await ETTask.CompletedTask;
		}
	}
}
