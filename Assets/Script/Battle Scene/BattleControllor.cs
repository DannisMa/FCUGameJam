using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleControllor : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject respawn_point;
    [SerializeField]
    private GameObject[] flag_prefabs;
    [SerializeField]
    private GameObject[] flag_points;
    [SerializeField]
    private GameObject[] flags;
    [SerializeField]
    private GameObject[] flag_icons;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player, respawn_point.transform.position, respawn_point.transform.rotation);
        StartCoroutine(InstantiateFlag());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InstantiateFlag(){
        int index = 0;
        yield return 0;
        foreach(GameObject point in flag_points){
            flags[index] = Instantiate(flag_prefabs[index], point.transform.position, flag_prefabs[index].transform.rotation);
            flag_icons[index].SetActive(false);
            index ++;
            yield return 0;
        }
        flags[3].GetComponent<Flag>().OnOwnerChange += OccupyFlag;
    }

    public void OccupyFlag(){
        flag_icons[3].SetActive(true);
        flags[3].GetComponent<Flag>().Effect();
    }
}
