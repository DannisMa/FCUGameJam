using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Dannis.FCUGameJame{
    public class HealthEffect : Effect
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void InitializeEffect(GameObject player, float _health, AbnormalType _type){
            m_owner = player;
            m_owner_team = player.GetComponent<PlayerManager>().team;
            m_type = _type;

            float _hp = player.GetComponent<PlayerManager>().Current_HP;
            _hp += 20f;
            if(_hp > player.GetComponent<PlayerManager>().MAX_HP)
                _hp = player.GetComponent<PlayerManager>().MAX_HP;
            player.GetComponent<PlayerManager>().Current_HP = _hp;

            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}