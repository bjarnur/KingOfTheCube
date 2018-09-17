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
        //transform.position = character.transform.position + cameraOffset;

        transform.position = character.transform.position.normalized * 50;
        float yOffset = transform.position.y;
        transform.position += new Vector3(0f, 35f - yOffset, 0f);
        transform.LookAt(character.transform.position + new Vector3(0f, 27f - 2*yOffset, 0f));
    }
}
