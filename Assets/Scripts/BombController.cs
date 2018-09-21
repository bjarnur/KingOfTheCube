using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public GameObject explosionPrefab;

    private void Start()
    {
        Physics.gravity = new Vector3(0, -50.0f, 0);
    }

    private void OnCollisionEnter(Collision col)
    {
        gameObject.SetActive(false); // hide rock
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1.5f);
    }
}
