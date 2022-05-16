using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingBattleControllor : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject respawn_point;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, respawn_point.transform.position, respawn_point.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
