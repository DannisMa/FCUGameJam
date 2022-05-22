using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private TeamEnum Team = TeamEnum.TeamBlue;
    public TeamEnum team{
        get{return Team;}
        set{Team = team;}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
