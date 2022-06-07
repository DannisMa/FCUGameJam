using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace com.Dannis.FCUGameJame{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public delegate void OnPlayerStateChange(TeamEnum _team);
        public OnPlayerStateChange onPlayerDie;

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

        protected float m_max_respawn_time = 5f;
        [SerializeField]
        protected float m_current_respawn_time;
        [SerializeField]
        protected Vector3 m_respawn_point;

        [SerializeField]
        protected Animator anima;

        [Tooltip("指標- GameObject PlayerUI")]
        [SerializeField]
        protected GameObject player_ui_prefab;
        [SerializeField]
        private GameObject card_area_prefab;
        protected GameObject effect_prefab;
        protected GameObject effect_gameobject;

        protected void InitializePlayer(){
            m_respawn_point = this.gameObject.transform.position;
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

            GameObject _card_area = Instantiate(card_area_prefab, GameObject.Find("Battle Room Menu Canvas").transform);
            GameObject _card = _card_area.transform.GetChild(0).GetChild(0).gameObject;
            _card.GetComponent<Card>().SettingPlayer(this.gameObject);
            _card.GetComponent<Card>().SettingCanvas(GameObject.Find("Battle Room Menu Canvas"));
            _card.GetComponent<Card>().onCardEffect += CardEffect;

            _card = _card_area.transform.GetChild(1).GetChild(0).gameObject;
            _card.GetComponent<Card>().SettingPlayer(this.gameObject);
            _card.GetComponent<Card>().SettingCanvas(GameObject.Find("Battle Room Menu Canvas"));
            _card.GetComponent<Card>().onCardEffect += CardEffect;

            _card = _card_area.transform.GetChild(2).GetChild(0).gameObject;
            _card.GetComponent<Card>().SettingPlayer(this.gameObject);
            _card.GetComponent<Card>().SettingCanvas(GameObject.Find("Battle Room Menu Canvas"));
            _card.GetComponent<Card>().onCardEffect += CardEffect;
            // _card_area.transform.GetChild(0).GetChild(0).SendMessage("SettingPlayer", this.gameObject, SendMessageOptions.RequireReceiver);
            // _card_area.transform.GetChild(0).GetChild(0).SendMessage("SettingCanvas", GameObject.Find("Battle Room Menu Canvas"), SendMessageOptions.RequireReceiver);

            // _card_area.transform.GetChild(1).GetChild(0).SendMessage("SettingPlayer", this.gameObject,  SendMessageOptions.RequireReceiver);
            // _card_area.transform.GetChild(1).GetChild(0).SendMessage("SettingCanvas", GameObject.Find("Battle Room Menu Canvas"), SendMessageOptions.RequireReceiver);

            // _card_area.transform.GetChild(2).GetChild(0).SendMessage("SettingPlayer", this.gameObject, SendMessageOptions.RequireReceiver);
            // _card_area.transform.GetChild(2).GetChild(0).SendMessage("SettingCanvas", GameObject.Find("Battle Room Menu Canvas"), SendMessageOptions.RequireReceiver);
        }

       

        public void CardEffect(string name, AbnormalType type){
            m_state = PlayerState.Attack;

            photonView.RPC ("RPCGetEffectName", RpcTarget.All, name);
            if(type == AbnormalType.Health)
                anima.SetBool("Buff", true);
            else if(type == AbnormalType.Dizzy)
                anima.SetBool("Range Attack", true);
            else if(type == AbnormalType.Attack)
                anima.SetBool("Short Attack", true);
            // photonView.RPC ("RPCCardEffect", RpcTarget.All, name, type);
        }

        [PunRPC]
        protected void RPCCardEffect(string _name, AbnormalType _type){
            if(_type == AbnormalType.Health)
                effect_gameobject.transform.GetChild(0).GetComponent<Effect>().InitializeEffect(this.gameObject, 40f, _type);
            else if(_type == AbnormalType.Dizzy)
                effect_gameobject.transform.GetChild(0).GetComponent<Effect>().InitializeEffect(this.gameObject, 1.5f, 5f, _type);
            else if(_type == AbnormalType.Attack)
                effect_gameobject.transform.GetChild(0).GetComponent<Effect>().InitializeEffect(this.gameObject, 1f, -40f, _type);
        }

        [PunRPC]
        public void RPCGetEffectName(string name){
            Debug.LogFormat("正在取得預知物{0}", name);
            effect_prefab = Resources.Load(name, typeof(GameObject)) as GameObject;
            Debug.LogFormat("已經取得預知物{0}", effect_prefab.name);
        }

        public void EndAnima(){
            Debug.Log("END");
            anima.SetBool("Short Attack", false);
            anima.SetBool("Buff", false);
            anima.SetBool("Range Attack", false);
            State = PlayerState.Walk;
        }

        public void StartShortAtack()
        {
            if(!photonView.IsMine)
                return;
            Debug.Log("本地短距離攻擊");
            photonView.RPC ("RPCStartShortAtack", RpcTarget.All);
        }

        public void StartBuff()
        {
            if(!photonView.IsMine)
                return;
            Debug.Log("本地回復");
            photonView.RPC ("RPCSStartBuff", RpcTarget.All);
        }

        public void StartRangeAttack()
        {
            if(!photonView.IsMine)
                return;
            Debug.Log("本地遠距攻擊");
            photonView.RPC ("RPCSStartRangeAttack", RpcTarget.All);
        }

        [PunRPC]
        public void RPCStartShortAtack()
        {
            effect_gameobject = Instantiate(effect_prefab, this.gameObject.transform.position, Quaternion.identity);
            effect_gameobject.transform.GetChild(0).GetComponent<Effect>().InitializeEffect(this.gameObject, 0.4f, -40f, AbnormalType.Attack);
            Debug.Log("短攻擊");
            // anima.SetBool("Short Attack", false);
            // State = PlayerState.Walk;
        }

        [PunRPC]
        public void RPCSStartBuff()
        {
            effect_gameobject = Instantiate(effect_prefab, this.gameObject.transform.position, Quaternion.identity);
            effect_gameobject.transform.GetChild(0).GetComponent<Effect>().InitializeEffect(this.gameObject, 40f, AbnormalType.Health);
            Debug.Log("遠端回復");
            // anima.SetBool("Buff", false);
            // State = PlayerState.Walk;
        }

        [PunRPC]
        public void RPCSStartRangeAttack()
        {
            effect_gameobject = Instantiate(effect_prefab, this.gameObject.transform.position, Quaternion.identity);
            effect_gameobject.transform.GetChild(0).GetComponent<Effect>().InitializeEffect(this.gameObject, 0.4f, 5f, AbnormalType.Dizzy);
            Debug.Log("範圍");
            // anima.SetBool("Range Attack", false);
            // State = PlayerState.Walk;
        }

        public void ChangeHP(float blood){
            if(!photonView.IsMine)
                return;
            Debug.Log("改變血量"+ photonView.ViewID);
            Current_HP += blood;

            if(Current_HP <=0 ){
                anima.SetBool("Die", true);
                m_current_respawn_time = m_max_respawn_time;
                m_state = PlayerState.Die;
                onPlayerDie(Team);
            }
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

        protected void Respawn(){        
            if(!photonView.IsMine)
                return;

            if(m_state != PlayerState.Die)
                return;
                
            m_current_respawn_time -= 1*Time.deltaTime;

            if(m_current_respawn_time > 0f)
                return;
            CharacterController cc = this.gameObject.GetComponent<CharacterController>();
            cc.enabled = false;
            this.gameObject.transform.position = m_respawn_point;
            cc.enabled = true;
            m_state = PlayerState.Walk;
            anima.SetBool("Die", false);
            m_current_respawn_time = m_max_respawn_time;
            m_current_hp = MAX_HP;

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