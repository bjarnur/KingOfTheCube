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

        if (col.gameObject.tag != GameConstants.ARPLAYERTAG)
        {
            hasCollided = true;
            int reject = Random.Range(0, 5);
            if (reject == 0)
            {
                if (isMultiplayer)
                    GetComponent<PhotonView>().RPC(GameConstants.RPCTags.plantSmoke, PhotonTargets.All);
                else
                    PlantSmoke();
            }
            else
            {
                if (isMultiplayer)
                    GetComponent<PhotonView>().RPC(GameConstants.RPCTags.plantExplosion, PhotonTargets.All);
                else
                    PlantExplosion();
        }

        if (isMultiplayer)
            PhotonNetwork.Destroy(this.gameObject);
        else
            Destroy(this.gameObject);
        }
    }

    private void PlantSmoke()
    {
        GameObject smoke = Instantiate(smokePrefab, transform.parent, false);
        smoke.transform.localPosition = transform.localPosition;

        SmokeParticleSystem smokeCtrl = smoke.GetComponent<SmokeParticleSystem>();
        smokeCtrl.numberOfParticles = 150;
        smokeCtrl.sidewaysSpreadFactor = 4;
        smokeCtrl.forwardSpreadFactor = 2;
        smokeCtrl.verticalSpreadFactor = 3;
        smokeCtrl.verticalSpreadVariance = 3;
        smokeCtrl.rejectionRate = 7;
        smokeCtrl.scaleWithTime = 1.005f;
        smokeCtrl.decelerateWithTime = 0.99f;
        smokeCtrl.scale = 0.1f;
        smokeCtrl.systemLife = 15f;
        smokeCtrl.particleLife = 5f;
        smokeCtrl.systemType = 0;

        //Need to do this last, some variables are used at startup
        smokeCtrl.enabled = true;
    }

    private void PlantExplosion()
    {
        Debug.Log("Plating explosin");

        GameObject explosion = Instantiate(smokePrefab, transform.parent, false);
        explosion.transform.localPosition = transform.localPosition;

        SmokeParticleSystem explosionCtrl = explosion.GetComponent<SmokeParticleSystem>();
        explosionCtrl.numberOfParticles = 50;
        explosionCtrl.sidewaysSpreadFactor = 16;
        explosionCtrl.forwardSpreadFactor = 16;
        explosionCtrl.verticalSpreadFactor = 0;
        explosionCtrl.verticalSpreadVariance = 6;
        explosionCtrl.rejectionRate = 3;
        explosionCtrl.scaleWithTime = 1.005f;
        explosionCtrl.decelerateWithTime = 0.9f;
        explosionCtrl.scale = 0.1f;
        explosionCtrl.systemLife = 1.5f;
        explosionCtrl.particleLife = 0.5f;
        explosionCtrl.systemType = 1;

        //Need to do this last, some variables are used at startup
        explosionCtrl.enabled = true;
    }
}
