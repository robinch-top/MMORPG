using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Mirror;
// 网络身份组件
// 暂时只定义少量属性和方法，可验证网络身份是否当前角色
[DisallowMultipleComponent]
public sealed class NetworkIdentity : MonoBehaviour
{
    // 判断此netIdentity 是否是MMOManager.localIdentity
    public bool isLocalPlayer => RPGManager.localIdentity == this;

    // 关于FormerlySerializedAs 可查看了解 https://www.xuanyusong.com/archives/3823
    // 如果还没遇到同样问题先放过，遇到时记得用
    [FormerlySerializedAs("m_SceneId"), HideInInspector]
    public ulong sceneId;

    public uint netId { get; internal set; }
    public static readonly Dictionary<uint, NetworkIdentity> spawned = new Dictionary<uint, NetworkIdentity>();

    NetworkBehaviour[] networkBehavioursCache;
    public NetworkBehaviour[] NetworkBehaviours
    {
        get
        {
            if (networkBehavioursCache == null)
            {
                CreateNetworkBehavioursCache();
            }
            return networkBehavioursCache;
        }
    }

    void CreateNetworkBehavioursCache()
    {
        networkBehavioursCache = GetComponents<NetworkBehaviour>();
        if (NetworkBehaviours.Length > 64)
        {
            //logger.LogError($"Only 64 NetworkBehaviour components are allowed for NetworkIdentity: {name} because of the dirtyComponentMask", this);
            // Log error once then resize array so that NetworkIdentity does not throw exceptions later
            Array.Resize(ref networkBehavioursCache, 64);
        }
    }
    
}