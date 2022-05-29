using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace com.Dannis.FCUGameJame{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Player characater prefab.")]
        [SerializeField]
        private GameObject player_prefab;
        [SerializeField]
        private Button leave_btn;
        public static GameManager Instance;
        
        void Awake(){
            leave_btn = GameObject.Find("Leave Button").GetComponent<Button>();
        }

        void Start(){
            leave_btn.onClick.AddListener(LeaveRoom);
            Instance = this;
            if(player_prefab == null){
                Debug.LogErrorFormat("動態生成玩家角色{0}", Application.loadedLevelName);
            }
            else if(PlayerManager.Local_Player_instance == null){
                print("IS NULL");
                PhotonNetwork.Instantiate(player_prefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("{0} 進入遊戲室", newPlayer.NickName);

            if(PhotonNetwork.IsMasterClient){
                Debug.Log("我是Master Client.");
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("{0} 離開遊戲室", newPlayer.NickName);

            if(PhotonNetwork.IsMasterClient){
                Debug.Log("我是Master Client.");
                LoadArena();
            }
        }

        public void LeaveRoom(){
            PhotonNetwork.LeaveRoom();
        }

        private void LoadArena(){
            if(!PhotonNetwork.IsMasterClient)
                Debug.LogError("我不是Master Client, 不做載入場景的動作");
            
            Debug.LogFormat("載入{0}人的場景", PhotonNetwork.CurrentRoom.PlayerCount);

            PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.PlayerCount);// 載入情景
        }
    }
}