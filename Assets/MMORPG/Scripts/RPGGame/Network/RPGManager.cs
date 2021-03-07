using System.Collections.Generic;
using UnityEngine;

// RPGManager单例，对游戏层的管理与调用

namespace Mirror
{
    public enum NetworkState { Offline, Login, Lobby, World }
    public partial class RPGManager : NetworkManager
    {   
        public static RPGManager singleton { get; private set; }
        public static NetworkIdentity localIdentity { get; private set; }
        
        public NetworkState state = NetworkState.Offline;
        [HideInInspector] public List<Player> playerClasses = new List<Player>(); 

        public Transform loginCameraLocation;
        public Transform selectionCameraLocation;
        public Transform SpawnLocation;

        public int selection = -1;
        public long selectedCharaID = -1;

        public Transform[] selectedLocations;

        /// <summary>
        /// 获取uiRoot
        /// </summary>
        public GameObject uiRoot;

        /// <summary>
        /// 获取Loading
        /// </summary>
        public GameObject loading;
        

        public void Awake()
        {
            if (singleton == null) singleton = this;
            if (Application.isPlaying) DontDestroyOnLoad(gameObject);

            playerClasses = FindPlayerClasses();

        }

        public void LoadCharacterToScene(GameObject go){
            go.GetComponent<CameraController>().enabled = true;
            Transform spawn = GameObject.Find("SpawnLocation").transform;
            go.transform.position = spawn.position;

            // 让Player持有当前Player实例，可静态调用
            Player.localPlayer = go.GetComponent<Player>();  
            // 让MMOManager持有当前Identity实例，可静态调用
            localIdentity =  go.GetComponent<NetworkIdentity>(); 
        }

        public void ResetPlayer(){
            Player.localPlayer.gameObject.GetComponent<CameraController>().enabled = false;
            Camera.main.transform.SetParent(transform.root, false);
            Destroy(Player.localPlayer.gameObject);
            
            Player.localPlayer = null;
            localIdentity = null;
        }

        public void CameraTo(NetworkState state){
            switch(state){
                case NetworkState.Login:
                    Camera.main.transform.position = loginCameraLocation.position;
                    Camera.main.transform.rotation = loginCameraLocation.rotation;
                    break;
                case NetworkState.Lobby:
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