using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlag : Flag
{
    // Start is called before the first frame update
    void Start()
    {
        m_flag_type = FlagEnum.Water;
        owner = TeamEnum.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        WhenPlayerIn(other);
    }

    void OnTriggerStay(Collider other){
        WhenPlayerStay(other);
    }

    void OnTriggerExit(Collider other){
        WhenPlayerOut(other);
    }

    public override void Effect(){
        Debug.Log(flag_type);
    }
}
