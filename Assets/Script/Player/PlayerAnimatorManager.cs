using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Dannis.FCUGameJame
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        [SerializeField]
        private float directionDampTime = 0.25f;

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            if(!animator){
                Debug.LogError("PlayerAnimatorManager - Animator component 遺失", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(photonView.IsMine == false && PhotonNetwork.IsConnected == true)
                return;
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
            // 只有在跑動時, 才可以跳躍.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }

            if(!animator)
                return;
            
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }

            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
    }
}
