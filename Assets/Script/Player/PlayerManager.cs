using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace com.Dannis.FCUGameJame{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("指標 - GameObject Beams")]
        [SerializeField]
        private GameObject beams;
        private bool IsFiring;

        [Tooltip("玩家的血量")]
        public float Health = 1f;
        [SerializeField]
        private static GameObject m_local_player_instance;
        public static GameObject Local_Player_instance{
            get{return m_local_player_instance;}
            set{
                if(m_local_player_instance == null){
                    m_local_player_instance = value;
                }
            }
        }

        [Tooltip("指標- GameObject PlayerUI")]
        [SerializeField]
        private GameObject player_ui_prefab;

        void Awake()
        {
            if(beams == null)
            {
                Debug.LogError("<Color = Red><a> 指標 - GameObject Beams 為空值</a></Color>", this);
            }
            else
            {
                beams.SetActive(false);
            }

            if (photonView.IsMine)
            {
                PlayerManager.Local_Player_instance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            CameraWork _cameraWork = this.GetComponent<CameraWork>();
            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else 
            {
                Debug.LogError("playerPrefab- CameraWork component 遺失", 
                    this); 
            }

            if (player_ui_prefab != null)
            {
                GameObject _uiGo = Instantiate(player_ui_prefab);
                _uiGo.SendMessage("SetTarget", this, 
                    SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("指標- GameObject PlayerUI 為空值", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(photonView.IsMine)
                ProcessInputs();
            if (beams != null && IsFiring != beams.activeSelf)
            {
                beams.SetActive(IsFiring);
            }
            if (Health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }

        private void ProcessInputs()
        {
            // 按下發射鈕
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            // 放開發射鈕
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beam"))
            {
                return;
            }
                Health -= 0.1f;
        }
        
        void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            Health -= 0.1f * Time.deltaTime;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
            if(stream.IsWriting){
                //是本人，更新資訊給其他玩家
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else{
                //非本人，負責接受資訊
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
        }
    }

}