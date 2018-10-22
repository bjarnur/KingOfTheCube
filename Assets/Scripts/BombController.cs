using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public GameObject explosionPrefab;

    public void Awake()
    {
        transform.SetParent(GameObject.Find("WorldContainer").transform, false);
        //gameObject.SetActive(true);

        GameObject kingObj = GameObject.Find("NetworkKing");
        if(kingObj != null)
        { 
            transform.localPosition = kingObj.transform.localPosition
                + (kingObj.transform.localRotation * Vector3.forward * 0.4f);            
        }
    }

    private void Start()
    {
        //Physics.gravity = new Vector3(0, -0.075f, 0);
        var rb = GetComponent<Rigidbody>();
        rb.AddForce((new Vector3(0, -0.075f, 0) - Physics.gravity) * rb.mass);
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != GameConstants.ARPLAYERTAG) {
            //GetComponent<PhotonView>().RPC(GameConstants.RPCTags.detonateBomb, PhotonTargets.All);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
