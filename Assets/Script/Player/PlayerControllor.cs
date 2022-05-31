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
        protected Animator anima;
        protected float move_speed = 4.0f;
        [SerializeField]
        protected Vector3 direction;

        void Awake(){
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // if ( Input.GetKey(KeyCode.A) ){ transform.Rotate( 0, -rotate* Time.deltaTime, 0 ); }
            // if ( Input.GetKey(KeyCode.D) ){ transform.Rotate( 0, rotate* Time.deltaTime, 0 ); }
            // horizontalMove = Input.GetAxis("Horizontal") * move_speed;
            // verticalMove   = Input.GetAxis("Vertical") * move_speed;
            // dir = transform.forward * verticalMove + transform.right * horizontalMove;
            
            // if(dir != Vector3.zero){
            //     anima.SetBool("Walk", true);
            // }
            // else{
            //     anima.SetBool("Walk", false);
            // }

            // characterController.Move(dir * Time.deltaTime);

            // if(Input.GetButtonDown("Jump") && characterController.isGrounded){
            //     velocity.y = jump_speed;
            // }

            // velocity.y -= gravity * Time.deltaTime;
            // characterController.Move(velocity * Time.deltaTime);
        }

        private void FixedUpdate(){

        }

        void OnTriggerStay(Collider other){
            if(other.gameObject.tag == "Enemy")
                anima.SetBool("Attack", true);
        }
        void OnTriggerExit(Collider other){
            if(other.gameObject.tag == "Enemy")
                anima.SetBool("Attack", false);
        }

        protected void InitializeController(){
            characterController = GetComponent<CharacterController>();
            anima = GetComponent<Animator>();
            variable_joystick = Instantiate(variable_joystick_prefab, GameObject.Find("Battle Room Menu Canvas").transform);
        }

        protected void PlayerMove(){
            if(!photonView.IsMine || characterController == null)
                return;
            direction = new Vector3(variable_joystick.Direction.x, 0f, variable_joystick.Direction.y);
            if(direction != Vector3.zero){
                characterController.Move(direction * move_speed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
            }
        }
    }
}