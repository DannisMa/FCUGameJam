using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Dannis.FCUGameJame{
    public class PlayerUI : MonoBehaviour
    {
        [Tooltip("玩家名稱")]
        [SerializeField]
        private Text player_name_text;
        [Tooltip("玩家血量")]
        [SerializeField]
        private Slider player_health_slider;
        private PlayerManager target;
        [Tooltip("名字字串在角色頭頂的距離")]
        [SerializeField]
        private Vector3 screen_offset = new Vector3(0f, 30f, 0f);
        float characater_controller_height = 0f;
        Transform targetTransform;
        Vector3 targetPosition;
        // Start is called before the first frame update
        void Start()
        {
            this.transform.SetParent(GameObject.Find("Battle Room Menu Canvas").GetComponent<Transform>(), false);
        }

        // Update is called once per frame
        void Update()
        {
            if (player_health_slider != null)
            {
                player_health_slider.value = target.Current_HP;
            }

            // 當有不明原因, Photon 沒有將 Player 相關的 instance 清乾淨時
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        void LateUpdate()
        {
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characater_controller_height;
                this.transform.position = 
                Camera.main.WorldToScreenPoint(targetPosition) 
                + screen_offset;
            }
        }   

        public void SetTarget(PlayerManager _target){
            if(_target == null){
                Debug.LogError("傳入的 PlayerManager instance 為空值", this);
                return;
            }
            target = _target;
            if (player_name_text != null)
            {
                player_name_text.text = target.photonView.Owner.NickName;
                // if(this.gameObject.GetComponent<PlayerManager>().team == TeamEnum.TeamRed)
                //     player_name_text.text = "Red : " + target.photonView.Owner.NickName;
                // else if(this.gameObject.GetComponent<PlayerManager>().team == TeamEnum.TeamBlue)
                //     player_name_text.text = "Blue : " + target.photonView.Owner.NickName;
                // else
                //     player_name_text.color = Color.yellow;
            }

            targetTransform = target.transform;
            CharacterController characterController = _target.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characater_controller_height = characterController.height;
            }
        }
    }
}
