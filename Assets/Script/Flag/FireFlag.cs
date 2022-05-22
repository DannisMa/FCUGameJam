using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlag : Flag
{
    int max_time = 15;
    [SerializeField]
    int current_time = 15;

    // Start is called before the first frame update
    void Start()
    {
        Owner = TeamEnum.None;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag != "Player")
            return;
        inside_players.Add(other.gameObject);
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.tag != "Player")
            return;

        TeamEnum player_team = other.gameObject.GetComponent<Player>().team;
        if(Owner == player_team)
            return;

        bool can_occupy = true;
        foreach(GameObject p in inside_players){
            if(p.GetComponent<Player>().team != player_team){
                can_occupy = false;
                StopCoroutine(CountDown());
                break;
            }
        }

        if(!can_occupy)
            return;
        if(!timer_counting)
            StartCoroutine(CountDown());
        if(current_time <= 0){
            Owner = player_team;
            print(player_team.ToString());
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag != "Player")
            return;
        inside_players.Remove(other.gameObject);
        timer_counting = false;
        current_time = max_time;
    }

    IEnumerator CountDown(){
        while(current_time > 0){
            timer_counting = true;
            yield return new WaitForSeconds(1);
            current_time--;
        }
    }
}