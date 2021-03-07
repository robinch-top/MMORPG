using System;
public static class IdGenerater
{
	private static ushort value;
	private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

	public static long GenerateId()
	{
		long time = ClientNowSeconds();

		return (time << 16) + ++value;
	}

	public static long ClientNowSeconds()
	{
		return (DateTime.UtcNow.Ticks - epoch) / 10000000;
	}
	
}
