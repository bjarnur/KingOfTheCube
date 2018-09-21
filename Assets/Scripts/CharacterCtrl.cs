using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour {

    /**********************\
        Tunable fields
    \**********************/

    public float speed = 0.1f;
    public float jumpSpeed = 0.5f;

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
    float angle = 180;

    //TODO: Set these values appropriately with respect to level dimensions
    const float xBounds = 1.55f;
    const float zBounds = 1.55f;

    bool climbing = false;
    bool moving = false;
    bool goingRight = false;
    bool grounded = true;
    bool jumping = false;


    /*********************\
        Unity functions
    \*********************/

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
	}	
	
	void Update () {
        //Ensure we only travel in the appropriate dimensions
        EnsureConsistentMovement();

        //Go around the corners when we get to them
        ChangeSides();

        //Update character position and rotation
        UpdateCharacterPosition();

        //Trigger animations based on user input
        TriggerAnimations();
    }

    public void Reset()
    {
        climbing = false;
        moving = false;
        goingRight = false;
        grounded = true;
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
            jumping = false;
        }
        TriggerAnimations();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder exit");
            rb.useGravity = true;
            climbing = false;
        }
        TriggerAnimations();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
            if (jumping)
            {
                jumping = false;
            }
        }
        TriggerAnimations();
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = false;
        }
        TriggerAnimations();
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

    void EnsureConsistentMovement()
    {
        switch (side)
        {
            case 0:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zBounds);
                break;
            case 1:
                transform.localPosition = new Vector3(xBounds, transform.localPosition.y, transform.localPosition.z);
                break;
            case 2:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -zBounds);
                break;
            case 3:
                transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, transform.localPosition.z);
                break;
        }
    }

    void ChangeSides()
    {
        if (transform.localPosition.x > xBounds) // Change to side 1
        {
            side = 1;
            angle = 90;
            if(transform.localPosition.z > 0) 
            {
                transform.localPosition = new Vector3(xBounds, transform.localPosition.y, zBounds - (transform.localPosition.x - xBounds)); 
            } else 
            {
                transform.localPosition = new Vector3(xBounds, transform.localPosition.y, -zBounds + (transform.localPosition.x - xBounds));
            }
        }
        else if (transform.localPosition.z < -zBounds) // Change to side 2
        {
            side = 2;
            angle = 180;
            if (transform.localPosition.x > 0) 
            {
                transform.localPosition = new Vector3(xBounds + (transform.localPosition.z + zBounds), transform.localPosition.y, -zBounds);
            } else 
            {
                transform.localPosition = new Vector3(-xBounds - (transform.localPosition.z + zBounds), transform.localPosition.y, -zBounds);
            }
        }
        else if (transform.localPosition.x < -xBounds) // Change to side 3
        {
            side = 3;
            angle = 270;
            if(transform.localPosition.z > 0) 
            {
                transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, zBounds + (transform.localPosition.x + xBounds));
            } else 
            {
                transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, -zBounds - (transform.localPosition.x + xBounds));
            }
        }
        else if (transform.localPosition.z > zBounds) // Change to side 0
        {
            side = 0;
            angle = 0;
            if(transform.localPosition.x > 0)
            {
                transform.localPosition = new Vector3(xBounds - (transform.localPosition.z - zBounds), transform.localPosition.y, zBounds);
            } else 
            {
                transform.localPosition = new Vector3(-xBounds + (transform.localPosition.z - zBounds), transform.localPosition.y, zBounds);
            }
        }
    }

    void UpdateCharacterPosition()
    {
        bool touch = false;
        float firstTouchDir = 0f;
        bool bothTouch = false;

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
                switch (Input.GetTouch(1).phase)
                {
                    case TouchPhase.Began:
                    case TouchPhase.Stationary:
                    case TouchPhase.Moved:
                        float secondTouchDir = Input.GetTouch(1).position.x < Screen.width / 2 ? -1f : 1f;
                        bothTouch = secondTouchDir != firstTouchDir;
                        break;
                }
            }
        }

        Vector3 yVelocity = world.up * rb.velocity.y;
        float hAxis = Input.GetAxisRaw("Horizontal");
        hAxis = touch ? firstTouchDir : hAxis;

        if (hAxis != 0f)
        {
            moving = true;
            goingRight = hAxis == 1f;
            transform.localPosition += hAxis * getDirection(true) * Time.deltaTime * speed;
        } else
        {
            moving = false;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow) || bothTouch)
        {
            if (climbing)
            {
                transform.position += transform.up * Time.deltaTime * speed;
            }
            else if (grounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
                jumping = true;
            }
        }
    }

    void TriggerAnimations()
    {
        if (goingRight) 
        {
            transform.localEulerAngles = new Vector3(0f, angle - 90, 0f);
        }
        else 
        {
            transform.localEulerAngles = new Vector3(0f, angle + 90, 0f);
        }

        if (grounded && !jumping)
        {
            animator.SetBool("Fall", false);
            animator.SetBool("Climb", false);
            animator.SetBool("Jump", false);
            if (moving)
            {
                animator.SetBool("Run", true);
                animator.SetBool("Stop", false);
            } else {
                animator.SetBool("Run", false);
                animator.SetBool("Stop", true);
            }
        } else if (climbing)
        {
            animator.SetBool("Climb", true);
            animator.SetBool("Jump", false);
        }
        else if (jumping)
        {
            animator.SetBool("Jump", true);
        } else if (!grounded) 
        {
            animator.SetBool("Fall", true);
        }
    }
}
