﻿using System.Collections;
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
            //gameObject.SetActive(true);

            GameObject kingObj = GameObject.Find("NetworkKing");
            if(kingObj != null)
            { 
                transform.localPosition = kingObj.transform.localPosition
                    + (kingObj.transform.localRotation * Vector3.forward * 0.4f);            
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
                        //GetComponent<PhotonView>().RPC(GameConstants.RPCTags.detonateBomb, PhotonTargets.All);
                        GameObject smoke = Instantiate(smokePrefab, transform.parent, false);
                        smoke.transform.localPosition = transform.localPosition;

                        //TODO hack to get smoke emitter correctly rotated, fix this            
                        if (transform.localPosition.x > 1.3 || transform.localPosition.x < -1.3)
                            smoke.GetComponent<SmokeParticleSystem>().axis = 1;

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
