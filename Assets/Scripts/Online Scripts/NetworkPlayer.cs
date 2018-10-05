using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkPlayer : Photon.MonoBehaviour {

    bool isAlive = true;
    public Vector3 position;
    public Quaternion rotation;
    public float larpSmoothing = 10f;
    
    // Use this for initialization
    void Start () {
        if(photonView.isMine)
        {
            gameObject.name = "Local Player";
            //GetComponent<CharacterCtrl>().enabled = true;
            GetComponent<PlayerController_AssemCube>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            gameObject.name = "Network Player";
            StartCoroutine("Alive");
            //this.transform.SetParent(OmniscientController.GetInstance().worldContainer);
        }
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

    /*
     For smooth transistion of networked players */
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
