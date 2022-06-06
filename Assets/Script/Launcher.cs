using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace com.Dannis.FCUGameJame{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        private string game_version = "1";
        [Tooltip("遊戲室玩家人數上限. 當遊戲室玩家人數已滿額, 新玩家只能新開遊戲室來進行遊戲.")]
        [SerializeField]
        private byte max_player_per_room = 10;
        [SerializeField]
        private Button battle_button;
        [SerializeField]
        private TMP_Dropdown battle_scene_dropdown;
        [SerializeField]
        private bool isConnecting = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            // 確保所有玩家載入相同的遊戲場景
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            battle_button.onClick.AddListener(OnClickBattleButton);
            battle_scene_dropdown.onValueChanged.AddListener(number => OnBattleSceneChange(number));
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Connect(){
            isConnecting = true;
            // 檢查連線
            if(PhotonNetwork.IsConnected){
                // 已連線，隨機加入一間遊戲室
                PhotonNetwork.JoinRandomRoom();
            }
            else{
                // 嘗試與photon cloud 連線
                PhotonNetwork.GameVersion = game_version;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN 呼叫 OnConnectedToMaster(), 已連上 Photon Cloud.");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN 呼叫 OnDisconnected(){0}.", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN 呼叫 OnJoinRandomFailed(), 隨機加入遊戲室失敗 : "+message);
            // 隨機加入遊戲室失敗. 可能原因是 1. 沒有遊戲室 或 2. 有的都滿了.    
            // 因此我們自己開一個遊戲室.
            Debug.Log(max_player_per_room);
            PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = max_player_per_room});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN 呼叫 OnJoinedRoom(), 已成功進入遊戲室中.");

            if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
                Debug.Log("我是第一個進入Room的玩家");
                Debug.Log("進行載入單人戰局的動作");
                
                if( battle_scene_dropdown.value == 0){
                    PhotonNetwork.LoadLevel(1);
                }
                else if( battle_scene_dropdown.value == 1){
                    PhotonNetwork.LoadLevel(2);
                }
                else if( battle_scene_dropdown.value == 2){
                    PhotonNetwork.LoadLevel(3);
                }
            }

            // if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
            //     Debug.Log("我是第一個進入Room的玩家");
            //     Debug.Log("進行載入單人戰局的動作");

            //     PhotonNetwork.LoadLevel(1);
            // }
        }

        private void OnClickBattleButton(){
            Connect();
        }

        private void OnBattleSceneChange(int num){
            Debug.LogFormat("戰鬥場景號碼 : {0}", num);
            if(num == 0)
                max_player_per_room = 1;
            else if(num == 1)
                max_player_per_room = 2;
            else
                max_player_per_room = 10;
        }
    }
}