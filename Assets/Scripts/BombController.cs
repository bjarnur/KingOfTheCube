using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Base" || col.gameObject.name == "CreepyPlayer")
        {
            gameObject.SetActive(false); // hide rock
        }
        // TODO: Add explosion!
    }
}
