using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    private void Start()
    {
        Physics.gravity = new Vector3(0, -50.0f, 0);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Base" || col.gameObject.name == "CreepyPlayer")
        {
            gameObject.SetActive(false); // hide rock
        }
        // TODO: Add explosion!
    }
}
