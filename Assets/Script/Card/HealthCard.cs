using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Dannis.FCUGameJame{
    public class HealthCard : Card
    {
        // Start is called before the first frame update
        void Start()
        {
            InitializeCard(10f, "Cards/Cylinder", "Cards/Health Range", AbnormalType.Health);
        }

        // Update is called once per frame
        void Update()
        {
            TimeCounter();
        }
        
        protected override void CardEffect(){
            // if(!player.GetComponent<PhotonView>().IsMine)
            //     return;
            // if(transform.localPosition.y > 50f){
            //     Debug.Log("發動效果");
            //     onCardEffect(range_prefab_path);
            //     counter = MAX_TIME;
            //     float _hp = player.GetComponent<PlayerManager>().Current_HP;
            //     _hp += 20f;
            //     if(_hp > player.GetComponent<PlayerManager>().MAX_HP)
            //         _hp = player.GetComponent<PlayerManager>().MAX_HP;
            //     player.GetComponent<PlayerManager>().Current_HP = _hp;
            // }
            if(!player.GetComponent<PhotonView>().IsMine)
                return;
            if(transform.localPosition.y > 50f){
                Debug.Log("發動效果");
                counter = MAX_TIME;
                onCardEffect(effect_prefab_path, type);
            }
            else if(transform.localPosition.y < -50f){
                Debug.Log("切換屬性");
            }
        }
    }
}
