using Assets.MMORPG.Scripts.RPGGame.Entity;
using Assets.MMORPG.Scripts.RPGGame.Network;
using Assets.MMORPG.Scripts.RPGGame.Data;
using System.Collections.Generic;
using UnityEngine;
using Assets.MMORPG.Scripts.RPGGame.CameraController;

namespace Mirror
{
    public enum NetworkState { Offline, Lobby, World }
    public partial class MMOManager : NetworkManager
    {

        public static MMOManager singleton { get; private set; }
        public static NetworkState state = NetworkState.Offline;
        public static NetworkIdentity localIdentity { get; private set; }

        [HideInInspector] public List<Player> playerClasses = new List<Player>();


        public Transform selectionCameraLocation;
        public Transform loginCameraLocation;
        public Transform SpawnLocation;

        public int selection = -1;
        public long selectedCharaID = -1;

        public Transform[] selectedLocations;
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

            playerClasses = FindPlayerClasses();

        }

        public void LoadCharacterToScene(GameObject playerObj)
        {
            playerObj.GetComponent<CameraController>().enabled = true;
            Transform spawn = GameObject.Find("SpawnLocation").transform;
            playerObj.transform.position = spawn.position;

        }

        public void SetLocal(GameObject go)
        {
            Player.localPlayer = go.GetComponent<Player>();  // 让Player持有当前Player实例，可静态调用
            localIdentity = go.GetComponent<NetworkIdentity>(); // 让MMOManager持有当前Identity实例，可静态调用
        }

        public void BackToSelection()
        {
            Player.localPlayer.gameObject.GetComponent<CameraController>().enabled = false;
            Camera.main.transform.SetParent(transform.root, false);
            Destroy(Player.localPlayer.gameObject);

            Player.localPlayer = null;
            localIdentity = null;
        }
        public void CameraTo(string location)
        {
            switch (location)
            {
                case "Login":
                    Camera.main.transform.position = loginCameraLocation.position;
                    Camera.main.transform.rotation = loginCameraLocation.rotation;
                    break;
                case "Selection":
                    Camera.main.transform.position = selectionCameraLocation.position;
                    Camera.main.transform.rotation = selectionCameraLocation.rotation;
                    break;
            }
        }

        /// 从spawnPrefabs中取得玩家类型的角色对象
        public List<Player> FindPlayerClasses()
        {
            List<Player> classes = new List<Player>();
            foreach (GameObject prefab in spawnPrefabs)
            {
                Player player = prefab.GetComponent<Player>();
                if (player != null)
                    classes.Add(player);
            }
            return classes;
        }
    }
}
