using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllor : MonoBehaviour
{
    private CharacterController characterController;
    private Animator anima;
    private float move_speed = 0.3f;
    private float rotate_speed = 90f;
    [SerializeField]
    private float jump_hight = 9.0f;
    float gravity = 20f;
    Vector3 move_direction = Vector3.zero, vDir = Vector3.zero;

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
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f) {
            // 開啟奔跑動畫
            anima.SetBool("Walk", true);
            // 這裡旋轉人物使他朝向我們想移動的方向
            transform.rotation = Quaternion.LookRotation(new Vector3(-h, 0, -v));
            Vector3 movedir = new Vector3(-h, -20, -v);
            // 用角色控制器讓他移動
            characterController.Move(movedir * move_speed);
        } else {
            // 切換到靜止動畫
            anima.SetBool("Walk", false);
        }
    }

    private void FixedUpdate(){

    }
}
