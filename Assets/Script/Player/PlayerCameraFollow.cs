using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Dannis.FCUGameJame{
    public class PlayerCameraFollow : MonoBehaviourPun
    {
        [SerializeField]
        private GameObject player_camera;
        private Vector3 camera_offset = new Vector3(0, 2.5f, -3);
        [Tooltip("名字字串在角色頭頂的距離")]
        private Vector3 screen_offset = new Vector3(0f, 5f, -5f);

        // Start is called before the first frame update
        void Start()
        {
            if(!photonView.IsMine)
                return;
            player_camera = GameObject.Find("Main Camera");
            player_camera.transform.LookAt(this.gameObject.transform);
        }

        // Update is called once per frame
        void Update()
        {
            if(!photonView.IsMine)
                return;
            player_camera.transform.position = this.gameObject.transform.position + screen_offset;
            player_camera.transform.LookAt(this.gameObject.transform);
        }
    }
}
