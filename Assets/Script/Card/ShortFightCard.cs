using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Dannis.FCUGameJame{
    public class ShortFightCard : Card
    {
        // Start is called before the first frame update
        void Start()
        {
            InitializeCard(4f, "Cards/Short", "Cards/Short Range", AbnormalType.Attack);
        }

        // Update is called once per frame
        void Update()
        {
           TimeCounter(); 
        }

        protected override void CardEffect()
        {
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