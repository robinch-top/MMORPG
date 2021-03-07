using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using ETModel;
using MMOGame;

// MMOManager单例，对网络层的管理与调用

namespace Mirror
{
    public partial class MMOManager : MonoBehaviour
    {
        public static MMOManager singleton { get; private set; }

        public void Awake()
        {
            if (singleton == null) singleton = this;
        }

        public void CmdQuitGame(){
            MapHelper.Back2LobbyAsync().Coroutine();
        }

    }
}