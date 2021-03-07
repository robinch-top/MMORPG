using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class NetworkBehaviour : MonoBehaviour
{
    /// 通过netIdentity实例的isLocalPlayer属性判断是不是当前角色
    public bool isLocalPlayer => netIdentity.isLocalPlayer;


    /// 缓存netIdentity
    NetworkIdentity netIdentityCache;
    public NetworkIdentity netIdentity
    {
        get
        {
            if (netIdentityCache == null)
            {
                netIdentityCache = GetComponent<NetworkIdentity>();
            }
            if (netIdentityCache == null)
            {
                Debug.LogError("There is no NetworkIdentity on " + name + ". Please add one.");
            }
            return netIdentityCache;
        }
    }
}