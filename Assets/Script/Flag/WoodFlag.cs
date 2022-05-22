using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodFlag : Flag
{
    // Start is called before the first frame update
    void Start()
    {
        m_flag_type = FlagEnum.Wood;
        owner = TeamEnum.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
