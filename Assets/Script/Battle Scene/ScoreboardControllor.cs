using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
namespace com.Dannis.FCUGameJame{
    public class ScoreboardControllor : MonoBehaviourPun, IPunObservable
    {
        [SerializeField]
        protected Text time_shower;
        protected float total_time = 210f;
        [SerializeField]
        protected float current_time;
        protected float flag_point_score = 0.0025f;
        [SerializeField]
        protected GameObject blue_team_score_shower;
        protected float blue_team_score = 0f;
        protected float blue_team_increa_scroe = 0f;
        [SerializeField]
        protected GameObject red_team_score_shower;
        protected float red_team_score = 0f;
        protected float red_team_increa_scroe = 0f;

        [SerializeField]
        protected GameObject[] m_flag_points = new GameObject[5];
        [SerializeField]
        protected GameObject[] m_flag_prefabs;
        protected GameObject[] m_flag = new GameObject[5];
        [SerializeField]
        protected Image[] m_flag_icon = new Image[5];
        [SerializeField]
        protected GameObject m_wating_panel;
        [SerializeField]
        protected Text m_wating_panel_text;
        [SerializeField]
        protected bool game_start = false;

        // Start is called before the first frame update
        void Start()
        {
            if(!photonView.IsMine)
                return;

            current_time = total_time;

            blue_team_score_shower.transform.localScale = new Vector3(0f, 1f, 1f);
            red_team_score_shower.transform.localScale = new Vector3(0f, 1f, 1f);
            for(int i = 0 ; i < 5 ; i++){
                m_flag_points[i] = GameObject.Find("Flag 0"+(i+1).ToString());
                m_flag[i] = PhotonNetwork.Instantiate("Flag/" + m_flag_prefabs[i].gameObject.name, m_flag_points[i].transform.position, Quaternion.AngleAxis(90, Vector3.left), 0);
                m_flag[i].GetComponent<Flag>().OnOwnerChange += ChangeFlagIcon;
            }
        }

        // Update is called once per frame
        void Update()
        {
            blue_team_score_shower.transform.localScale = new Vector3(blue_team_score, 1f, 1f);
            red_team_score_shower.transform.localScale = new Vector3(red_team_score, 1f, 1f);

            if(!photonView.IsMine || game_start != true)
                return;

            current_time -= 1f*Time.deltaTime;

            blue_team_score += blue_team_increa_scroe*Time.deltaTime;
            red_team_score += red_team_increa_scroe*Time.deltaTime;

            string min = (Mathf.Floor(current_time/60f)).ToString();
            string sec = (Mathf.Floor(current_time%60f)).ToString();
            if(time_shower.text != (min+":"+sec))
                ChangTimeShower((min+":"+sec));

            if(blue_team_score_shower.transform.localScale.x >= 1f || red_team_score_shower.transform.localScale.x >= 1f || current_time <= 0f){
                Debug.Log("檢查獲勝");
                CheckWhoWin();
            }
        }

        protected void ChangeFlagIcon(int owner, int flag){
            // Debug.Log(owner+" : "+flag);
            photonView.RPC ("RPCChangeFlagIcon", RpcTarget.All, owner, flag);
        }

        [PunRPC]
        protected void RPCChangeFlagIcon(int owner, int flag){
            Debug.Log(owner+" : "+flag);
            if(owner == 0){
                m_flag_icon[flag].color = Color.blue;

                blue_team_increa_scroe += flag_point_score;
                red_team_increa_scroe -= flag_point_score;
                if(red_team_increa_scroe < 0f){
                    red_team_increa_scroe = 0f;
                }
            }
            else{
                m_flag_icon[flag].color = Color.red;

                blue_team_increa_scroe -= flag_point_score;
                red_team_increa_scroe += flag_point_score;
                if(blue_team_increa_scroe < 0f){
                    blue_team_increa_scroe = 0f;
                }
            }
        }

        protected void ChangTimeShower(string time){
            photonView.RPC ("RPCChangeTimeShower", RpcTarget.All, time);
        }

        protected void CheckWhoWin(){
            int win_team = 0;
            if(blue_team_increa_scroe > red_team_increa_scroe){
                Debug.Log("藍隊獲勝");
                win_team = 0;
            }else if( red_team_increa_scroe > blue_team_increa_scroe){
                Debug.Log("紅隊獲勝");
                win_team = 1;
            }else{
                Debug.Log("平手");
                win_team = 3;
            }
            photonView.RPC("RPCCheckWhoWin", RpcTarget.All, win_team);
        }

        [PunRPC]
        public void RPCCheckWhoWin(int win_team){
            game_start = false;
            m_wating_panel.SetActive(true);
            if(win_team == 0){
                Debug.Log("RPC 藍隊獲勝");
                m_wating_panel_text.text = "藍隊獲勝";
                m_wating_panel_text.color = Color.cyan;
            }else if(win_team == 1){
                Debug.Log("RPC 紅隊獲勝");
                m_wating_panel_text.text = "紅隊獲勝";
                m_wating_panel_text.color = Color.red;
            }else{
                Debug.Log("RPC 平手獲勝");
                m_wating_panel_text.text = "平手";
                m_wating_panel_text.color = Color.green;
            }
        }

        [PunRPC]
        protected void RPCChangeTimeShower(string time){
            time_shower.text = time;
        }

        public void StareGame(){
            photonView.RPC ("RPCStareGame", RpcTarget.All);
        }

        [PunRPC]
        protected void RPCStareGame(){
            m_wating_panel.SetActive(false);
            game_start = true;
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
}