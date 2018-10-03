using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(photonView.isMine)
        {
            if (this.tag == GameConstants.ARPLAYERTAG)  {
                GetComponent<CharacterCtrl>().enabled = true;
            }
            else {
                GetComponent<PlayerController_AssemCube>().enabled = true;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
