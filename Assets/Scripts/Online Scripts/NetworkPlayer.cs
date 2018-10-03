using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(photonView.isMine)
        {
            GetComponent<PlayerController_AssemCube>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
