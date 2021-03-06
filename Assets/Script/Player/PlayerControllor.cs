using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Dannis.FCUGameJame{
    public class PlayerControllor : MonoBehaviourPun
    {
        [SerializeField]
        protected VariableJoystick variable_joystick_prefab;
        [SerializeField]
        protected VariableJoystick variable_joystick;
        [SerializeField]
        protected CharacterController characterController;
        [SerializeField]
        protected Animator anima;
        [SerializeField]
        protected PlayerManager player_manager;
        protected float move_speed = 5.0f;
        [SerializeField]
        protected Vector3 direction;

        void OnTriggerStay(Collider other){
            if(other.gameObject.tag == "Enemy")
                anima.SetBool("Attack", true);
        }
        void OnTriggerExit(Collider other){
            if(other.gameObject.tag == "Enemy")
                anima.SetBool("Attack", false);
        }

        protected void InitializeController(){
            if(!photonView.IsMine)
                return;
            characterController = GetComponent<CharacterController>();
            anima = GetComponent<Animator>();
            player_manager = GetComponent<PlayerManager>();
            variable_joystick = Instantiate(variable_joystick_prefab, GameObject.Find("Battle Room Menu Canvas").transform);           
        }

        protected void PlayerMove(){
            if(!photonView.IsMine)
                return;

            if(player_manager.State != PlayerState.Walk || characterController == null )
                return;
            direction = new Vector3(variable_joystick.Direction.x, 0f, variable_joystick.Direction.y);
            if(direction != Vector3.zero){
                characterController.Move(direction * move_speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
                anima.SetBool("Walk", true);
            }
            else
                anima.SetBool("Walk", false);
        }

    }
}