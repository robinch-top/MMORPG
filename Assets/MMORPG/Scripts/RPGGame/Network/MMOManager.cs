using Assets.MMORPG.Scripts.RPGGame.Entity;
using Assets.MMORPG.Scripts.RPGGame.Network;
using Assets.MMORPG.Scripts.RPGGame.Data;
using System.Collections.Generic;
using UnityEngine;
namespace Mirror
{
    public enum NetworkState { Offline, Lobby, World }
    public partial class MMOManager : MonoBehaviour
    {

        public static MMOManager singleton { get; private set; }
        public static NetworkState state = NetworkState.Offline;

        public static NetworkIdentity localIdentity { get; set; }

        /// <summary>
        /// 角色选择场景中创建的Player列表
        /// </summary>
        public List<Player> players;
        /// <summary>
        /// 玩家账号下创建的角色列表
        /// </summary>
        public List<Character> charas;

        public static string account;
        public static long accountID;

        public void Awake()
        {
            if (singleton == null) singleton = this;
            if (Application.isPlaying) DontDestroyOnLoad(gameObject);

            /// 实际上是选择角色界面选择角色进入游戏场景后才有localPlayer，localIdentity
            /// 目前当前角色直接放在场景中，就直接在这里获取了
            Player.localPlayer = GameObject.Find("Player(bobo)").GetComponent<Player>();  // 让Player持有当前Player实例，可静态调用
            localIdentity = GameObject.Find("Player(bobo)").GetComponent<NetworkIdentity>(); // 让MMOManager持有当前Identity实例，可静态调用

        }

    }
}
