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
        [SerializeField]
        private int MAX_PLAYER;
        [SerializeField]
        private GameObject[] m_respawm_points;
        [Tooltip("Player characater prefab.")]
        [SerializeField]
        private GameObject player_prefab;
        [SerializeField]
        private GameObject score_prefab;
        [SerializeField]
        private GameObject score_object;
        [SerializeField]
        private Button leave_btn;
        public static GameManager Instance;
        protected bool already_start = false;
        protected int player_counter = 0;
        
        void Awake(){
            leave_btn = GameObject.Find("Leave Button").GetComponent<Button>();
        }

        void Start(){
            leave_btn.onClick.AddListener(LeaveRoom);
            Instance = this;
            player_prefab = Resources.Load(GameConst.Characater_name) as GameObject;

            player_prefab.GetComponent<CharacterController>().enabled = true;
            player_prefab.GetComponent<PlayerCameraFollow>().enabled = true;
            player_prefab.GetComponent<PlayerControllor>().enabled = true;
            player_prefab.GetComponent<PlayerManager>().enabled = true;
            player_prefab.GetComponent<PhotonView>().enabled = true;

            int point_num = PhotonNetwork.CountOfPlayers;
            Debug.LogFormat("玩家人數 : {0}", point_num);
            if(point_num <= 0)
                point_num = 0;
            else
                point_num -= 1;

            PhotonNetwork.Instantiate(player_prefab.name, m_respawm_points[point_num].transform.position, Quaternion.identity, 0);
            player_counter ++;
            if(PhotonNetwork.IsMasterClient){
                Debug.Log("我是Master Client.");
                score_object = PhotonNetwork.Instantiate("GameUI/"+score_prefab.name, score_prefab.transform.position, Quaternion.identity, 0);
                Debug.Log("生成記分板");

                if(player_counter == MAX_PLAYER){
                    score_object.GetComponent<ScoreboardControllor>().StareGame();
                }else{
                    Debug.LogFormat("玩家人數目前:{0}", PhotonNetwork.CountOfPlayers);
                }
            }
        }

        void Update(){
            // Debug.Log("運作一");
            // if(already_start)
            //     return;
            // Debug.Log("運作二");
            // if(PhotonNetwork.IsMasterClient){
            //     if(PhotonNetwork.CountOfPlayers == MAX_PLAYER){
            //         score_object.GetComponent<ScoreboardControllor>().StareGame();
            //         already_start = true;
            //     }else{
            //         Debug.LogFormat("玩家人數目前:{0}", PhotonNetwork.CountOfPlayers);
            //     }
            // }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("{0} 進入遊戲室", newPlayer.NickName);
            player_counter++;

            int point_num = PhotonNetwork.CountOfPlayers;
            Debug.LogFormat("玩家人數 : {0}", point_num);
            
            if(PhotonNetwork.IsMasterClient){
                if(player_counter == MAX_PLAYER){
                    score_object.GetComponent<ScoreboardControllor>().StareGame();
                }else{
                    Debug.LogFormat("玩家人數目前:{0}", PhotonNetwork.CountOfPlayers);
                }
            }
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.LogFormat("{0} 離開遊戲室", newPlayer.NickName);
            player_counter--;
        }

        public void LeaveRoom(){
            player_prefab.GetComponent<CharacterController>().enabled = false;
            player_prefab.GetComponent<PlayerCameraFollow>().enabled = false;
            player_prefab.GetComponent<PlayerControllor>().enabled = false;
            player_prefab.GetComponent<PlayerManager>().enabled = false;
            player_prefab.GetComponent<PhotonView>().enabled = false;
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