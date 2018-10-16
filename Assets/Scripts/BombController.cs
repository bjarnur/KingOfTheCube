using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public GameObject explosionPrefab;

    public void Awake()
    {
        gameObject.SetActive(true);
        gameObject.transform.SetParent(GameObject.Find("WorldContainer").transform, false);
    }

    private void Start()
    {
        Physics.gravity = new Vector3(0, -1f, 0);
    }

    private void OnCollisionEnter(Collision col)
    {
        gameObject.SetActive(false); // hide rock
        GameObject explosion = Instantiate(explosionPrefab, transform.parent);
        Destroy(explosion, 1.5f);
        GetComponent<PhotonView>().RPC("DetonateBomb", PhotonTargets.AllBuffered);
    }
}
