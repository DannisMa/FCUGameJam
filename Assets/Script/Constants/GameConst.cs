using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConst
{
    private static string characater_name = "Pico Chen Fighter";
    public static string Characater_name{
        get{return characater_name;}
        set{
            if(value == characater_name)
                return;
            characater_name = value;
        }
    }
}
