using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRock : Photon.MonoBehaviour {

    public float larpSmoothing = 10;
    public GameObject smokePrefab;

    private bool bombIsLive = true;
    private Vector3 position;

    void Start()
    {
        if (photonView.isMine)
        {
            gameObject.name = "Local Rock";
        }
        else
        {
            gameObject.name = "Network Rock";
            StartCoroutine("UpdateNetworked");
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {        
        if (stream.isWriting)
        {
            var controller = GetComponent<BombController>();
            stream.SendNext(transform.localPosition);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
        }
    }

    /*
 For smooth transistion of networked players */
    IEnumerator UpdateNetworked()
    {
        while (gameObject.activeSelf)
        {
            if (transform.localPosition == Vector3.zero)
                transform.localPosition = position;
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * larpSmoothing);
            yield return null;
        }
    }

    [PunRPC]
    void DetonateBomb()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(this.gameObject);
        //bombIsLive = false;
        //position = Vector3.zero;
    }

    [PunRPC]
    void PlantSmoke()
    {
        GameObject smoke = Instantiate(smokePrefab, transform.parent, false);
        SmokeParticleSystem smokeCtrl = smoke.GetComponent<SmokeParticleSystem>();
        smoke.transform.localPosition = transform.localPosition;
    }
}
