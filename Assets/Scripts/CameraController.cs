using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject character;

    private Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
        cameraOffset = transform.position - character.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = character.transform.position + cameraOffset;
		
	}
}
