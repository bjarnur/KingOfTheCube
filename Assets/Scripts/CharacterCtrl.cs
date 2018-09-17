using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour {

    /**********************\
        Tunable fields
    \**********************/

    public float speed = 0.1f;

    Transform world;
    public void SetWorld(Transform world)
    {
        this.world = world;
    }

    /*********************\
        Private fields
    \*********************/

    Animator animator;
    Rigidbody rb;

    // 0 & 2 = Moving along X
    // 1 & 3 = Moving along Z
    int side = 2;    
    float angle = 0;

    //TODO: Set these values appropriately with respect to level dimensions
    private float xBoundsMin = -1.55f;
    private float xBoundsMax = 1.55f;
    private float zBoundsMin = -1.55f;
    private float zBoundsMax = 1.55f;

    bool jumping = false;
    bool climbing = false;
    bool goingRight = false;


    /*********************\
        Unity functions
    \*********************/

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
	}	
	
	void Update () {
        bool touch = false;
        float firstTouchDir = 0f;
        bool jump = false;

        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    touch = true;
                    firstTouchDir = Input.GetTouch(0).position.x < Screen.width / 2 ? -1f : 1f;
                    break;
            }

            if (Input.touchCount > 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    float secondTouchDir = Input.GetTouch(1).position.x < Screen.width / 2 ? -1f : 1f;
                    jump = secondTouchDir != firstTouchDir;
                }
            }
        }

        float mov;
        mov = Input.GetAxisRaw("Horizontal");
        mov = touch ? firstTouchDir : mov;

        MovePlayer(mov);

        if (!jumping && (jump || Input.GetAxisRaw("Vertical") > 0f))
        {
            jumping = true;
            rb.velocity += world.up * 0.5f;
        }

        // Animate
        bool running = mov != 0f;
        animator.SetBool("Run", running);
        animator.SetBool("Stop", !running);
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumping = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder enter");
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.useGravity = false;
            climbing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder exit");
            rb.useGravity = true;
            climbing = false;
        }
    }

    /*********************************\
        Helper and utility functions
    \*********************************/ 

    Vector3 getDirection(bool local = false)
    {
        if (local)
        {
            switch (side)
            {
                case 0:
                    return -world.worldToLocalMatrix.MultiplyVector(world.right);
                case 1:
                    return world.worldToLocalMatrix.MultiplyVector(world.forward);
                case 2:
                    return world.worldToLocalMatrix.MultiplyVector(world.right);
                case 3:
                    return -world.worldToLocalMatrix.MultiplyVector(world.forward);
            }
        } else
        {
            switch (side)
            {
                case 0:
                    return -world.right;
                case 1:
                    return world.forward;
                case 2:
                    return world.right;
                case 3:
                    return -world.forward;
            }
        }
        
        return Vector3.zero;
    }

    void MovePlayer(float mov)
    {
        // Check boundaries and change side if needed
        CheckBounds();

        Vector3 dir = getDirection(true) * mov;
        transform.localPosition += dir * Time.deltaTime * speed;

        if (mov != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rb.MoveRotation(newRotation);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, angle, 0f); // Face the edge of the cube
        }

    }

    void CheckBounds()
    {
        if (transform.localPosition.x > xBoundsMax) // Change to side 1
        {
            side = 1;
            angle = 90;
            transform.localPosition = new Vector3(xBoundsMax, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.z < zBoundsMin) // Change to side 2
        {
            side = 2;
            angle = 180;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zBoundsMin);
        }
        else if (transform.localPosition.x < xBoundsMin) // Change to side 3
        {
            side = 3;
            angle = 270;
            transform.localPosition = new Vector3(xBoundsMin, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.z > zBoundsMax) // Change to side 0
        {
            side = 0;
            angle = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zBoundsMax);
        }
    }
}
