using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace com.Dannis.FCUGameJame{
    public class Flag : MonoBehaviourPun, IPunObservable
    {
        public delegate void OnTeanEnumChangeDelegate(int owner, int flag);
        public event OnTeanEnumChangeDelegate OnOwnerChange;

        [SerializeField]
        protected  TeamEnum m_owner;
        public TeamEnum owner{
            get{ return m_owner;}
            set{
                if(value == TeamEnum.None) 
                    return;
                m_owner = value;
                if(OnOwnerChange != null)
                        OnOwnerChange((int)owner, (int)m_flag_type);
                }
        }

        protected FlagEnum m_flag_type;
        public FlagEnum flag_type{
            get { return m_flag_type; }
        }


        [SerializeField]
        protected ArrayList inside_players = new ArrayList();
        protected bool timer_counting = false;
        protected Coroutine timer_corontine = null;
        protected int max_time = 15;
        [SerializeField]
        protected int current_time = 15;

        [SerializeField]
        protected GameObject range;
        [SerializeField]
        protected  Material blue_material;
        [SerializeField]
        protected  Material red_material;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        protected void WhenPlayerIn(Collider other){
            if( !photonView.IsMine )
                return;
                
            if(other.gameObject.tag != "Player")
                return;
            inside_players.Add(other.gameObject);
        }

        protected void WhenPlayerStay(Collider other){
            if( !photonView.IsMine )
                return;

            if(other.gameObject.tag != "Player")
                return;

            TeamEnum player_team = other.gameObject.GetComponent<PlayerManager>().team;
            if(owner == player_team)
                return;

            bool can_occupy = true;
            foreach(GameObject p in inside_players){
                if(p.GetComponent<PlayerManager>().team != player_team){
                    can_occupy = false;
                    StopCoroutine(timer_corontine);
                    break;
                }
            }

            if(!can_occupy)
                return;
            if(!timer_counting)
                timer_corontine = StartCoroutine(CountDown());
            if(current_time <= 0){
                owner = player_team;
                if( owner == TeamEnum.TeamBlue){
                    range.GetComponent<Renderer>().material = blue_material;
                }
                else if( owner == TeamEnum.TeamRed ){
                    range.GetComponent<Renderer>().material = red_material;
                }
            }
        }

        protected void WhenPlayerOut(Collider other){
            if( !photonView.IsMine )
                return;
                            
            if(other.gameObject.tag != "Player")
                return;
            inside_players.Remove(other.gameObject);
            timer_counting = false;
            if(timer_corontine != null)
                StopCoroutine(timer_corontine);
            current_time = max_time;
        }

        protected IEnumerator CountDown(){
            while(current_time > 0){
                timer_counting = true;
                yield return new WaitForSeconds(1);
                current_time--;
            }
        }

        public virtual void Effect(){
            Debug.Log("Effect");
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
            if(stream.IsWriting){
                //???????????????????????????????????????
                stream.SendNext(owner);
                // Debug.Log(this.name+"?????????????????????"+ owner);
            }
            else{
                //??????????????????????????????
                owner = (TeamEnum)stream.ReceiveNext();
                // Debug.Log(this.name+"?????????????????????"+ owner);
                if( owner == TeamEnum.TeamBlue){
                    range.GetComponent<Renderer>().material = blue_material;
                }
                else if( owner == TeamEnum.TeamRed ){
                    range.GetComponent<Renderer>().material = red_material;
                }
            }
        }    
    }
}