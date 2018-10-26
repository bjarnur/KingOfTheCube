using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public GameObject explosionPrefab;
    public GameObject smokePrefab;

    public bool isMultiplayer = true;
    public bool isAR = true;    

    public void Awake()
    {
        if(isMultiplayer)
        {
            transform.SetParent(GameObject.Find("WorldContainer").transform, false);            
            //transform.SetParent(GameObject.Find("Wrapper").transform, false);

            GameObject kingObj = GameObject.FindWithTag(GameConstants.GameObjectsTags.king);            
            if (kingObj != null)
            {
                Vector3 localFwd = kingObj.transform.localRotation * Vector3.forward;
                transform.localPosition = kingObj.transform.localPosition + (localFwd * 0.4f);            
            }
        }

    }

    private void Start()
    {
        //Physics.gravity = new Vector3(0, -0.075f, 0);
        var rb = GetComponent<Rigidbody>();
        if(isAR)
        {
            rb.AddForce((new Vector3(0, -0.075f, 0) - Physics.gravity) * rb.mass);
        }
        else
        {
            rb.AddForce((new Vector3(0.0f, -500.0f, 0.0f) - Physics.gravity) * rb.mass);
        }
    }


    private void OnCollisionEnter(Collision col)
    {
        if(isMultiplayer)
        {
            if (col.gameObject.tag != GameConstants.ARPLAYERTAG)
            {
                    int reject = Random.Range(0, 5);
                    if (reject == 0)
                    {
                        GetComponent<PhotonView>().RPC("PlantSmoke", PhotonTargets.All);
                    }
                    PhotonNetwork.Destroy(this.gameObject);
                }
        }
        else
        {
            Destroy(this.gameObject);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 1.5f);
        }     
    }
}
