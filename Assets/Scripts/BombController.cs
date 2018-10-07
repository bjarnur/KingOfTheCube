using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public GameObject explosionPrefab;

    private void Start()
    {
        Physics.gravity = new Vector3(0, -1f, 0);
    }

    private void OnCollisionEnter(Collision col)
    {
        gameObject.SetActive(false); // hide rock
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        //explosion.transform.localPosition = transform.localPosition;
        Destroy(explosion, 1.5f);
    }
}
