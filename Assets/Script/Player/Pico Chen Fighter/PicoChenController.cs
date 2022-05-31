using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dannis.FCUGameJame{   
    public class PicoChenController : PlayerControllor
    {
        // Start is called before the first frame update
        void Start()
        {
            if(!photonView.IsMine)
                return;
            InitializeController();
        }

        // Update is called once per frame
        void Update()
        {
            if(!photonView.IsMine)
                return;
            PlayerMove();
        }
    }
}