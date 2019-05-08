using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour {

    public float larpSmoothing = 0.1f;

    private float distanceMoved = 0.0f;
    private float movementRange = 2.0f;

    private bool goingUp;
    private Vector3 targetPosition;

    // Use this for initialization
    void Start ()
    {
        Vector3 initalOffset = new Vector3(0.0f, 0.07f, 0.0f);
        transform.position = transform.parent.position + initalOffset;

        targetPosition = transform.localPosition;
        targetPosition.y += movementRange;
        goingUp = true;

        StartCoroutine("IndicatorBounce");
    }
    
    IEnumerator IndicatorBounce()
    {
        while (true)
        {
            Debug.Log("Going up: " + goingUp + ", local pos: " + transform.localPosition + ", target pos: " + targetPosition);
            if(goingUp)
            {
                if(transform.localPosition.y > targetPosition.y - 0.1)
                { 
                    targetPosition.y -= movementRange;
                    goingUp = false;
                }
            }
            else
            {
                if (transform.localPosition.y < targetPosition.y + 0.1)
                {
                    targetPosition.y += movementRange;
                    goingUp = true;
                }
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * larpSmoothing);
            yield return null;
        }
    }
}
