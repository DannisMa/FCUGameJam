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
    protected int MAX_HP = 100;
    protected int m_hp;
    protected int m_atk;
    protected int m_def;
    protected int m_speed;
    protected PlayerType m_player_type;
    protected PlayerType weak_player_type;
    protected PlayerType strong_player_type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
