using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public GameObject explosionPrefab;
    public GameObject smokePrefab;

    public bool isMultiplayer = true;
    public bool isAR = true;    

    private bool hasCollided = false;

    public void Awake()
    {
        if(isMultiplayer)
        {
            transform.SetParent(GameObject.FindWithTag(GameConstants.GameObjectsTags.worldContainer).transform, false);
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
        if (hasCollided) return;

        if(isMultiplayer)
        {
            if (col.gameObject.tag != GameConstants.ARPLAYERTAG)
            {
                    hasCollided = true;
                    int reject = Random.Range(0, 5);
                    if (reject == 0)
                    {
                        GetComponent<PhotonView>().RPC(GameConstants.RPCTags.plantSmoke, PhotonTargets.All);
                    }
                    else
                    {
                        GetComponent<PhotonView>().RPC(GameConstants.RPCTags.plantExplosion, PhotonTargets.All);
                    }                    
                    PhotonNetwork.Destroy(this.gameObject);
             }
        }
        else
        {
            hasCollided = true;
            Destroy(this.gameObject);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 1.5f);
        }     
    }
}
