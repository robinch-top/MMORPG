using ETModel;
namespace ETModel
{
/// <summary>
/// MMOServer外网消息
/// </summary>
	[Message(OuterOpcode.EnterMap_C2G)]
	public partial class EnterMap_C2G : IRequest {}

	[Message(OuterOpcode.EnterMap_G2C)]
	public partial class EnterMap_G2C : IResponse {}

	[Message(OuterOpcode.CreateUnits_M2C)]
	public partial class CreateUnits_M2C : IActorMessage {}

	[Message(OuterOpcode.Sync_MoveMessage)]
	public partial class Sync_MoveMessage : IActorMessage {}

	[Message(OuterOpcode.State_FrameMessage)]
	public partial class State_FrameMessage : IActorLocationMessage {}

	[Message(OuterOpcode.MoveInfo)]
	public partial class MoveInfo {}

//获取大厅玩家信息请求
	[Message(OuterOpcode.GetUserInfo_InLobby_C2G)]
	public partial class GetUserInfo_InLobby_C2G : IRequest {}

	[Message(OuterOpcode.GetUserInfo_InLobby_G2C)]
	public partial class GetUserInfo_InLobby_G2C : IResponse {}

//----ET
	[Message(OuterOpcode.Actor_Test)]
	public partial class Actor_Test : IActorMessage {}

	[Message(OuterOpcode.C2M_TestRequest)]
	public partial class C2M_TestRequest : IActorLocationRequest {}

	[Message(OuterOpcode.M2C_TestResponse)]
	public partial class M2C_TestResponse : IActorLocationResponse {}

	[Message(OuterOpcode.Actor_TransferRequest)]
	public partial class Actor_TransferRequest : IActorLocationRequest {}

	[Message(OuterOpcode.Actor_TransferResponse)]
	public partial class Actor_TransferResponse : IActorLocationResponse {}

	[Message(OuterOpcode.C2G_EnterMap)]
	public partial class C2G_EnterMap : IRequest {}

	[Message(OuterOpcode.G2C_EnterMap)]
	public partial class G2C_EnterMap : IResponse {}

	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.M2C_CreateUnits)]
	public partial class M2C_CreateUnits : IActorMessage {}

	[Message(OuterOpcode.Frame_ClickMap)]
	public partial class Frame_ClickMap : IActorLocationMessage {}

	[Message(OuterOpcode.State_ClickMap)]
	public partial class State_ClickMap : IActorLocationMessage {}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	public partial class M2C_PathfindingResult : IActorMessage {}

	[Message(OuterOpcode.C2R_Ping)]
	public partial class C2R_Ping : IRequest {}

	[Message(OuterOpcode.R2C_Ping)]
	public partial class R2C_Ping : IResponse {}

	[Message(OuterOpcode.G2C_Test)]
	public partial class G2C_Test : IMessage {}

	[Message(OuterOpcode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(OuterOpcode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

}
namespace ETModel
{
	public static partial class OuterOpcode
	{
		 public const ushort EnterMap_C2G = 101;
		 public const ushort EnterMap_G2C = 102;
		 public const ushort CreateUnits_M2C = 103;
		 public const ushort Sync_MoveMessage = 104;
		 public const ushort State_FrameMessage = 105;
		 public const ushort MoveInfo = 106;
		 public const ushort GetUserInfo_InLobby_C2G = 107;
		 public const ushort GetUserInfo_InLobby_G2C = 108;
		 public const ushort Actor_Test = 109;
		 public const ushort C2M_TestRequest = 110;
		 public const ushort M2C_TestResponse = 111;
		 public const ushort Actor_TransferRequest = 112;
		 public const ushort Actor_TransferResponse = 113;
		 public const ushort C2G_EnterMap = 114;
		 public const ushort G2C_EnterMap = 115;
		 public const ushort UnitInfo = 116;
		 public const ushort M2C_CreateUnits = 117;
		 public const ushort Frame_ClickMap = 118;
		 public const ushort State_ClickMap = 119;
		 public const ushort M2C_PathfindingResult = 120;
		 public const ushort C2R_Ping = 121;
		 public const ushort R2C_Ping = 122;
		 public const ushort G2C_Test = 123;
		 public const ushort C2M_Reload = 124;
		 public const ushort M2C_Reload = 125;
	}
}
