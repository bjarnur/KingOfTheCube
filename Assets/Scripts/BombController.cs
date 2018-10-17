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
        Physics.gravity = new Vector3(0, -0.01f, 0);
    }

    private void OnCollisionEnter(Collision col)
    {
        gameObject.SetActive(false); // hide rock
        //GameObject explosion = Instantiate(explosionPrefab, transform.parent);
        //Destroy(explosion, 1.5f);
        GetComponent<PhotonView>().RPC("DetonateBomb", PhotonTargets.All);
    }
}
