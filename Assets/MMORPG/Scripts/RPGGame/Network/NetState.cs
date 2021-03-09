namespace Mirror
{

    // 世界地图，副本地图等服务网络状态
    public enum WorldState
    {
        Idle,
        Connecting,
        Connected,
        Closed,
    }

    // 网关服务网络状态
    public enum NetworkState 
    { 
        Offline, 
        Login, 
        Lobby, 
        World 
    }
}   