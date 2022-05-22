using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField]
    protected TeamEnum Owner;
    [SerializeField]
    protected ArrayList inside_players = new ArrayList();
    protected bool timer_counting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
