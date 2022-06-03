using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dannis.FCUGameJame{
    public class ShortFightEffect : Effect
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            EffectWork();
        }
        public override void InitializeEffect(GameObject player, float _time, float atk, AbnormalType _type){
            m_owner = player;
            m_owner_team = player.GetComponent<PlayerManager>().team;
            counter_time = _time;
            m_heart_range = atk;
            m_type = _type;
        }

        void OnTriggerEnter(Collider other){
            if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerManager>().team != Team)
                CardEffect(other);
        }

        protected override void CardEffect(Collider other)
        {
            Debug.Log("開始受傷 : " + m_heart_range);
            
            if(m_heart_range == 0)
                m_heart_range = -45;
            other.gameObject.GetComponent<PlayerManager>().ChangeHP(m_heart_range);
        }

    }
}