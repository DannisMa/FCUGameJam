using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Camera player_camera;
    private Vector3 camera_offset = new Vector3(0, 2.5f, -3);

    // Start is called before the first frame update
    void Start()
    {
        player_camera = Instantiate(Camera.main, gameObject.transform.position + camera_offset, gameObject.transform.rotation, gameObject.transform);
        player_camera.transform.Rotate(15f, player_camera.transform.rotation.y, player_camera.transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
