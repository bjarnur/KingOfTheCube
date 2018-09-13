using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileController : MonoBehaviour {

    public Transform VirtualCube;
    public Transform Character;
	
	void Update () {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("#########\nTOUCHED\n#########");
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        }
    }
}
