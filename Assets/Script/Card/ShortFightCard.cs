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
            InitializeCard(15f, "Cards/Short", "Cards/Short Range");
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
                
                if(effect_prefab == null)
                    effect_prefab = Resources.Load(effect_prefab_path, typeof(GameObject));
                    effect_gameobject = PhotonNetwork.Instantiate(effect_prefab_path, player.transform.position, player.transform.rotation, 0);
                    effect_gameobject.transform.GetChild(0).GetComponent<Effect>().InitializeEffect(player, 1.5f, -45f, AbnormalType.Attack);
            }
            else if(transform.localPosition.y < -50f){
                Debug.Log("切換屬性");
            }
        }
    }
}