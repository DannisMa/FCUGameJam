using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dannis.FCUGameJame{
    public class DizzyEffect : Effect
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

        public override void InitializeEffect(GameObject player, float time, float _effetc_time, AbnormalType _type){
            m_owner = player;
            m_owner_team = player.GetComponent<PlayerManager>().team;
            counter_time = time;
            m_effect_time = _effetc_time;
            m_type = _type;
        }

        void OnTriggerEnter(Collider other){
            if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerManager>().team != Team)
                CardEffect(other);
        }

        protected override void CardEffect(Collider other)
        {
            Debug.Log("開始暈眩 : " + m_effect_time);
            other.gameObject.GetComponent<PlayerManager>().StartDizzy(m_effect_time, PlayerState.Dizzy);
        }
    }
}
