using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace com.Dannis.FCUGameJame{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField]
        [Tooltip("隊伍 - TeamEnum")]

        protected TeamEnum Team;
        public TeamEnum team{
            get{return Team;}
            set{Team = value;}
        }

        [SerializeField]
        [Tooltip("玩家狀態")]

        protected PlayerState m_state;
        public PlayerState State{
            get{return m_state;}
            set{m_state = value;}
        }


        protected int HP_MAX = 100;
        public float MAX_HP{
            get{ return HP_MAX;}
        }
        [SerializeField]
        protected float m_current_hp = 100;
        public float Current_HP{
            get{ return m_current_hp;}
            set{  m_current_hp = value;}
        }
        protected int m_atk;
        protected int m_def;
        protected int m_speed;
        protected int MP_MAX;
        protected float m_current_mp;
        public float Current_MP{
            get{ return m_current_mp;}
            set{  m_current_mp = value;}
        }
        protected int m_mp_speed;

        [SerializeField]
        protected Animator anima;


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
                return;

            if (m_current_hp <= 0f)
            {
                // GameManager.Instance.LeaveRoom(); //改成播放死亡動畫
            }
        }

        // protected void CreatePlayerUI(){
        //     if (player_ui_prefab != null)
        //     {
        //         GameObject _uiGo = Instantiate(player_ui_prefab);
        //         _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        //     }
        //     else
        //         Debug.LogWarning("指標- GameObject PlayerUI 為空值", this);
        // }

        protected void InitializePlayer(){

            if (player_ui_prefab != null)
            {
                GameObject _uiGo = Instantiate(player_ui_prefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
                Debug.Log("生成player UI");
            }
            else
                Debug.LogWarning("指標- GameObject PlayerUI 為空值", this);
                
            if(!photonView.IsMine)
                return;
            
            int _id = photonView.ViewID / 1000;
            _id = _id % 2;
            Debug.LogFormat("該玩家隊伍{0}", _id);
            if(_id == 0)
                team = TeamEnum.TeamBlue;
            else
                team = TeamEnum.TeamRed;

            State = PlayerState.Walk;


            
            PlayerCameraFollow _cameraWork = this.GetComponent<PlayerCameraFollow>();
            if (_cameraWork != null)
            {
                    _cameraWork.enabled= true;
                    Debug.Log("相機初始化成功");
            }
            else 
                Debug.LogError("playerPrefab- PlayerCameraFollow component 遺失",this);

            anima = this.gameObject.GetComponent<Animator>();
        }

        public void ChangeHP(float blood){
            if(!photonView.IsMine)
                return;

            Current_HP += blood;
        }

        public void StartDizzy(float time, PlayerState _state){
            if(!photonView.IsMine && _state != PlayerState.Dizzy)
                return;
            m_state = _state;
            if(anima == null)
                anima = this.gameObject.GetComponent<Animator>();
            anima.SetBool("Dizzy", true);
            StartCoroutine(TimeCounter(time));
        }

        protected IEnumerator TimeCounter(float total_time){
            Debug.Log(total_time);
            while(true){
                total_time -= 1 * Time.deltaTime;
                yield return 0;
                if(total_time <= 0f){
                    anima.SetBool("Dizzy", false);
                    m_state = PlayerState.Walk;
                    break;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
            if(stream.IsWriting){
                //是本人，更新資訊給其他玩家
                stream.SendNext(team);
                stream.SendNext(Current_HP);
                stream.SendNext(m_current_mp);
                stream.SendNext(m_state);
            }
            else{
                //非本人，負責接受資訊
                team = (TeamEnum)stream.ReceiveNext();
                Current_HP = (float)stream.ReceiveNext();
                Current_MP = (float)stream.ReceiveNext();
                m_state = (PlayerState)stream.ReceiveNext();
            }
        }
    }

}