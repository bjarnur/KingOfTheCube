using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCameraController : MonoBehaviour {

    public GameObject king;

    private Vector3 cameraOffset;

	// Use this for initialization
	void Start () {
        cameraOffset = transform.position - king.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = king.transform.position + cameraOffset;
		
	}
}
