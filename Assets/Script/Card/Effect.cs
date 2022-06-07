using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dannis.FCUGameJame{
    public class Effect : MonoBehaviour
    {
        [SerializeField]
        protected GameObject m_owner = null;
        [SerializeField]
        protected TeamEnum m_owner_team;
        public TeamEnum Team{
            get{return m_owner_team;}
        }
        [SerializeField]
        protected float counter_time = 9999f; //範圍持續時間
        [SerializeField]
        protected AbnormalType m_type;
        public AbnormalType Type{
            get { return m_type; }
        }
        [SerializeField]
        protected float m_effect_time; // 效果持續時間
        public float Effect_time{
            get{ return m_effect_time; }
        }
        [SerializeField]
        protected float m_heart_range; //傷害量
        [SerializeField]
        protected float m_health_range; // 回復量

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }


        //Dizzy Effect Constructor || ATK Effect Constructor
        public virtual void InitializeEffect(GameObject player, float time, float _effetc_time, AbnormalType _type){}
        
        //Health Effect Constructor
        public virtual void InitializeEffect(GameObject player, float _health, AbnormalType _type){}

        protected virtual void EffectWork(){
            this.gameObject.transform.parent.position = m_owner.transform.position;
            this.gameObject.transform.parent.rotation = m_owner.transform.rotation;
            
            // counter_time -= 1 * Time.deltaTime;

            // if(counter_time <= 0){
            //     Destroy(this.gameObject.transform.parent.gameObject);
            // }
        }

        // public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        //     if(stream.IsWriting){
        //         //是本人，更新資訊給其他玩家
        //         stream.SendNext(m_type);
        //         stream.SendNext(m_owner_team);
        //         stream.SendNext(m_effect_time);
        //         stream.SendNext(m_heart_range);
        //         stream.SendNext(m_health_range);
        //     }
        //     else{
        //         //非本人，負責接受資訊
        //         m_type = (AbnormalType)stream.ReceiveNext();
        //         m_owner_team = (TeamEnum)stream.ReceiveNext();
        //         m_effect_time = (float)stream.ReceiveNext();
        //         m_heart_range = (float)stream.ReceiveNext();
        //         m_health_range = (float)stream.ReceiveNext();
        //     }
        // }

        protected virtual void CardEffect(Collider other){}
    }
}