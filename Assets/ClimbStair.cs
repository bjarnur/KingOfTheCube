using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbStair : MonoBehaviour {

    GameObject stair;
    public string stairAnimationName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Colision con: " + other.gameObject.name);
        if (stairAnimationName != null && other.gameObject.tag == "Creepy") {
            other.gameObject.GetComponent<Animator>().Play(stairAnimationName);
        }
    }
}
