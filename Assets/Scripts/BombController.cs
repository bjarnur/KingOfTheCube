using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public GameObject explosionPrefab;
    public GameObject smokePrefab;

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
        Physics.gravity = new Vector3(0, -1, 0);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != GameConstants.ARPLAYERTAG) {
            //GetComponent<PhotonView>().RPC(GameConstants.RPCTags.detonateBomb, PhotonTargets.All);
            GameObject smoke = Instantiate(smokePrefab, transform.parent, false);
            smoke.transform.localPosition = transform.localPosition;

            //TODO hack to get smoke emitter correctly rotated, fix this
            if (transform.localPosition.x > 1.3 || transform.localPosition.x < -1.3)
                smoke.GetComponent<SmokeParticleSystem>().axis = 1;

            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
