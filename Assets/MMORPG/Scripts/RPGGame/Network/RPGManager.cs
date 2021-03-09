using System.Collections.Generic;
using UnityEngine;

// RPGManager单例，对游戏层的管理与调用

namespace Mirror
{
    
    public enum UIState {UILogin,UIRegister,UISelection,UICreation}
    public partial class RPGManager : NetworkManager
    {   
        public static RPGManager singleton { get; private set; }
        public static NetworkIdentity localIdentity { get; private set; }
        
        
        [HideInInspector] public List<Player> playerClasses = new List<Player>(); 

        public Transform loginCameraLocation;
        public Transform selectionCameraLocation;
        public Transform SpawnLocation;

        public int selection = -1;
        public Transform[] selectedLocations;
        public Dictionary<int,Character> CharaWrap = new Dictionary<int, Character>();

        // 进入场景的角色CharaId
        public long CharaId = 0;
        // 上一个UI状态
        public UIState lastUI;
        // 网络状态
        public NetworkState state = NetworkState.Offline;

        public void Awake()
        {
            if (singleton == null) singleton = this;
            if (Application.isPlaying) DontDestroyOnLoad(gameObject);

            playerClasses = FindPlayerClasses();
        }

        // 被网络层调用，创建新角色后，刷新角色预览
        public void RefreshPreview(Character chara){
            // 保存Character
            CharaWrap.Add(chara.index,chara);

            GameObject playerObj = DataBase.CharacterLoad(chara, playerClasses,true);
            Player player = playerObj.GetComponent<Player>();
            // 刷新预览角色还是加载全部预览角色都把CharaId写入Player组件
            // SelectableCharacter就能获得选中的角色的CharaId
            player.CharaId = chara.CharaId;

            ShowPreview(playerObj, selectedLocations[chara.index-1], chara.index);
        }

        // 被网络层调用，登录后加载所有创建的角色进行预览
        public void LoadAllPreview(Character[] characters)
        {
            foreach (Character chara in characters)
            {
                // 保存Character
                CharaWrap.Add(chara.index,chara);
                
                GameObject playerObj = DataBase.CharacterLoad(chara, playerClasses,true);
                Player player = playerObj.GetComponent<Player>();
                // 刷新预览角色还是加载全部预览角色都把CharaId写入Player组件
                // SelectableCharacter就能获得选中的角色的CharaId
                player.CharaId = chara.CharaId;

                ShowPreview(playerObj, selectedLocations[chara.index-1], chara.index); 
            }
        }

        // 被网络层调用，角色进入场景
        public void LoadCharacterToScene(GameObject playerObj){
            playerObj.GetComponent<CameraController>().enabled = true;

            Transform spawn = GameObject.Find("SpawnLocation").transform;
            playerObj.transform.position = spawn.position;

            // 让Player持有当前Player实例，可静态调用
            Player.localPlayer = playerObj.GetComponent<Player>();  
            // 让MMOManager持有当前Identity实例，可静态调用
            localIdentity =  playerObj.GetComponent<NetworkIdentity>(); 

            // 通知网络层，CharaTOSceneFinished
            //CharaToSceneFinished.Invoke();

            ClearPreviews();
        }

        public void LoadOthersToScene(GameObject playerObj){
            Transform spawn = GameObject.Find("SpawnLocation").transform;
            playerObj.transform.position = spawn.position;
        }

        public GameObject CreatePlayer(Character chara){
            return DataBase.CharacterLoad(chara, playerClasses);
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
                // 世界地图摄像机跟随角色的
            }
        }

        void ShowPreview(GameObject playerObj, Transform location, int charaIndex){
            // 预览角色禁用背包,移除控制组件
            playerObj.GetComponent<PlayerInventory>().enabled = false;
            //CharacterMovement cm = playerObj.GetComponent<CharacterMovement>();
            CharacterController cc = playerObj.GetComponent<CharacterController>();
            //Destroy(cm,0);
            Destroy(cc,0);
            
            playerObj.transform.SetParent(location,false); 
            playerObj.transform.rotation = location.rotation;

            // 角色选择组件
            playerObj.AddComponent<SelectableCharacter>();
            playerObj.GetComponent<SelectableCharacter>().index = charaIndex;
        }

        public void ClearPreviews()
        {
            selection = -1;
            foreach (Transform location in selectedLocations)
                if (location.childCount > 0)
                    Destroy(location.GetChild(0).gameObject);

            CharaWrap = new Dictionary<int, Character>();
        }

        /// 可用的Location index编号
        public int AvailableLocation(){
            int index = -1;
            for(int i=0;i<selectedLocations.Length;++i)
                if (selectedLocations[i].childCount > 0){
                    index =  -1;
                }else{
                    index =  i+1;
                    return index;
                }
            return index;
        }

        /// 从spawnPrefabs中取得玩家类型的角色预制对象
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