using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject character;

    private Vector3 cameraOffset;

	void Start () {
        cameraOffset = transform.position - character.transform.position;
    }

    void LateUpdate () {
        //transform.position = character.transform.position + cameraOffset;

        float yOffset = character.transform.position.y;
        Vector3 charPosition = character.transform.position - new Vector3(0f, yOffset, 0f);
        transform.position = charPosition.normalized * 50;
        transform.position += new Vector3(0f, 35f, 0f);
        transform.LookAt(charPosition + new Vector3(0f, 23f, 0f));
    }
}
