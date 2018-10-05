using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkPlayer : Photon.MonoBehaviour {


    bool isAlive = true;
    public Vector3 position;
    public Quaternion rotation;
    public float larpSmoothing = 10f;
    
	void Start () {
		if(photonView.isMine)
        {
            gameObject.name = "Local Player";
            if (this.tag == GameConstants.ARPLAYERTAG)  {
                GetComponent<CharacterCtrl>().enabled = true;
                GetComponent<Rigidbody>().useGravity = true;
            }
            else {
                GetComponent<PlayerController_AssemCube>().enabled = true;
            }
        }
        else
        {
            gameObject.name = "Network Player";
            StartCoroutine("Alive");
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
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    IEnumerator Alive()
    {
        while(isAlive)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * larpSmoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * larpSmoothing);

            yield return null;
        }
    }
}
