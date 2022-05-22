using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllor : MonoBehaviour
{
    private CharacterController characterController;
    private Animator anima;
    private float move_speed = 4.0f;
    private float jump_speed = 5.0f;
    private float rotate = 180.0f;　// 旋轉速度
    private float horizontalMove , verticalMove;
    private Vector3 dir; //儲存移動的方向    Vector3 move_direction = Vector3.zero, vDir = Vector3.zero;
    private float gravity = 9.81f;
    private Vector3 velocity; //控制Y軸加速度

    void Awake(){
        characterController = GetComponent<CharacterController>();
        anima = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKey(KeyCode.A) ){ transform.Rotate( 0, -rotate* Time.deltaTime, 0 ); }
        if ( Input.GetKey(KeyCode.D) ){ transform.Rotate( 0, rotate* Time.deltaTime, 0 ); }
        horizontalMove = Input.GetAxis("Horizontal") * move_speed;
        verticalMove   = Input.GetAxis("Vertical") * move_speed;
        dir = transform.forward * verticalMove + transform.right * horizontalMove;
        
        if(dir != Vector3.zero){
            anima.SetBool("Walk", true);
        }
        else{
            anima.SetBool("Walk", false);
        }

        characterController.Move(dir * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && characterController.isGrounded){
            velocity.y = jump_speed;
        }

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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
}
