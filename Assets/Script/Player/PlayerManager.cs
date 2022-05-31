using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace com.Dannis.FCUGameJame{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("隊伍 - TeamEnum")]

        protected TeamEnum Team;
        public TeamEnum team{
            get{return Team;}
            set{Team = value;}
        }


        protected int HP_MAX;
        protected int m_current_hp;
        public int Current_HP{
            get{ return m_current_hp;}
        }
        protected int m_atk;
        protected int m_def;
        protected int m_speed;
        protected int MP_MAX;
        protected int m_current_mp;
        protected int m_mp_speed;


        // [SerializeField]
        // private static GameObject m_local_player_instance;
        // public static GameObject Local_Player_instance{
        //     get{return m_local_player_instance;}
        //     set{
        //         if(m_local_player_instance == null){
        //             m_local_player_instance = value;
        //         }
        //     }
        // }


        [Tooltip("指標- GameObject PlayerUI")]
        [SerializeField]
        protected GameObject player_ui_prefab;

        void Awake()
        {
            // if (photonView.IsMine)
            // {
            //     PlayerManager.Local_Player_instance = this.gameObject;
            // }
            // DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {   
            
        }

        // Update is called once per frame
        void Update()
        {
            if(photonView.IsMine)
                ProcessInputs();

            if (m_current_hp <= 0f)
            {
                // GameManager.Instance.LeaveRoom(); //改成播放死亡動畫
            }
        }

        protected void CreatePlayerUI(){
            if (player_ui_prefab != null)
            {
                GameObject _uiGo = Instantiate(player_ui_prefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
                Debug.LogWarning("指標- GameObject PlayerUI 為空值", this);
        }

        protected void InitializePlayerTeam(){
            if(!photonView.IsMine)
                return;
                
            int _id = photonView.ViewID / 1000;
            _id = _id % 2;
            Debug.LogFormat("該玩家隊伍{0}", _id);
            if(_id == 0)
                team = TeamEnum.TeamBlue;
            else
                team = TeamEnum.TeamRed;
        }

        protected void InitializeCamera(){
            if(!photonView.IsMine)
                return;

            PlayerCameraFollow _cameraWork = this.GetComponent<PlayerCameraFollow>();
            if (_cameraWork != null)
            {
                    _cameraWork.enabled= true;
                    Debug.Log("相機初始化成功");
            }
            else 
                Debug.LogError("playerPrefab- PlayerCameraFollow component 遺失",this); 
        }

        protected void ProcessInputs()
        {
            // // 按下發射鈕
            // if (Input.GetButtonDown("Fire1"))
            // {
            //     if (!IsFiring)
            //     {
            //         IsFiring = true;
            //     }
            // }
            // // 放開發射鈕
            // if (Input.GetButtonUp("Fire1"))
            // {
            //     if (IsFiring)
            //     {
            //         IsFiring = false;
            //     }
            // }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            // if (!other.name.Contains("Beam"))
            // {
            //     return;
            // }
            //     Health -= 0.1f;
        }
        
        void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            // if (!other.name.Contains("Beam"))
            // {
            //     return;
            // }
            // Health -= 0.1f * Time.deltaTime;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
            if(stream.IsWriting){
                //是本人，更新資訊給其他玩家
                stream.SendNext(team);
                stream.SendNext(m_current_hp);
                stream.SendNext(m_current_mp);
            }
            else{
                //非本人，負責接受資訊
                this.team = (TeamEnum)stream.ReceiveNext();
                this.m_current_hp = (int)stream.ReceiveNext();
                this.m_current_mp = (int)stream.ReceiveNext();
            }
        }
    }

}