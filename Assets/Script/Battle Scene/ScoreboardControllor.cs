using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ScoreboardControllor : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    protected Text time_shower;
    protected float total_time = 210f;
    [SerializeField]
    protected GameObject blue_team_score_shower;
    protected float blue_team_score = 0f;
    [SerializeField]
    protected GameObject red_team_score_shower;
    protected float red_team_score = 0f;

    [SerializeField]
    protected GameObject[] m_flag_points = new GameObject[5];
    [SerializeField]
    protected GameObject[] m_flag_prefabs;
    protected GameObject[] m_flag = new GameObject[5];

    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.IsMine)
            return;
        
        blue_team_score_shower.transform.localScale = new Vector3(0f, 1f, 1f);
        red_team_score_shower.transform.localScale = new Vector3(0f, 1f, 1f);
        for(int i = 0 ; i < 5 ; i++){
            m_flag_points[i] = GameObject.Find("Flag 0"+(i+1).ToString());
            m_flag[i] = PhotonNetwork.Instantiate("Flag/" + m_flag_prefabs[i].gameObject.name, m_flag_points[i].transform.position, Quaternion.AngleAxis(90, Vector3.left), 0);

        }
    }

    // Update is called once per frame
    void Update()
    {
        blue_team_score_shower.transform.localScale = new Vector3(blue_team_score, 1f, 1f);
        red_team_score_shower.transform.localScale = new Vector3(red_team_score, 1f, 1f);

        if(!photonView.IsMine)
            return;

        blue_team_score += 0.005f*Time.deltaTime;
        red_team_score += 0.005f*Time.deltaTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
            if(stream.IsWriting){
                //是本人，更新資訊給其他玩家
                stream.SendNext(total_time);
                stream.SendNext(blue_team_score);
                stream.SendNext(red_team_score);
            }
            else{
                //非本人，負責接受資訊
                total_time = (float)stream.ReceiveNext();
                blue_team_score = (float)stream.ReceiveNext();
                red_team_score = (float)stream.ReceiveNext();
            }
    }
}
