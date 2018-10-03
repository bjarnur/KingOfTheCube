using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkPlayer : Photon.MonoBehaviour {

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        if(photonView.isMine){
            if (this.tag == GameConstants.ARPLAYERTAG)  {
                GetComponent<CharacterCtrl>().enabled = true;
                GetComponent<Rigidbody>().useGravity = true;
            }
            else {
                GetComponent<PlayerController_AssemCube>().enabled = true;
            }
        }
        else {
            this.transform.SetParent(OmniscientController.GetInstance().worldContainer);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting){
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else {
            transform.localPosition = (Vector3)stream.ReceiveNext();
            transform.localRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
