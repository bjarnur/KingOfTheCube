using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour {

    /**********************\
        Tunable fields
    \**********************/

    public float speed = 2f;
    public float jumpSpeed = 5f;


    /*********************\
        Private fields
    \*********************/

    Animator animator;
    Rigidbody rb;

    int side = 0;    
    float angle = 0;

    //TODO: Set these values appropriately with respect to level dimensions
    const float xBoundsMin = -4.5f;
    const float xBoundsMax = 4.5f;
    const float zBoundsMin = -4.5f;
    const float zBoundsMax = 4.5f;

    bool climbing = false;
    bool goingRight = false;
    bool grounded = false;
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
        ensureConsistentMovement();

        //Trigger animations based on user input
        triggerAnimations();

        //Update character position and rotation
        updateCharacterPosition();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder enter");
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.useGravity = false;
            climbing = true;
            if(jumping)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("Climb", true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder exit");
            rb.useGravity = true;
            climbing = false;
            animator.SetBool("Climb", false);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
            if (jumping)
            {
                animator.SetBool("Jump", false);
                jumping = false;
            }

        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = false;
        }
    }


    /*********************************\
        Helper and utility functions
    \*********************************/ 

    void ensureConsistentMovement()
    {
        switch (side)
        {
            case 0:
                transform.position = new Vector3(transform.position.x, transform.position.y, zBoundsMax);
                break;
            case 1:
                transform.position = new Vector3(xBoundsMax, transform.position.y, transform.position.z);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x, transform.position.y, zBoundsMin);
                break;
            case 3:
                transform.position = new Vector3(xBoundsMin, transform.position.y, transform.position.z);
                break;
        }
    }

    void updateCharacterPosition()
    {
        //Update character position, and rotation around the cube while left arrow button is held down
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > xBoundsMax && equals(transform.position.z, zBoundsMax, 0.001f))
            {
                crossAngle(xBoundsMax, zBoundsMax, true);
            }
            if (equals(transform.position.x, xBoundsMax, 0.001f) && transform.position.z < zBoundsMin)
            {
                crossAngle(xBoundsMax, zBoundsMin, true);
            }
            if (equals(transform.position.z, zBoundsMin, 0.001f) && transform.position.x < xBoundsMin)
            {
                crossAngle(xBoundsMin, zBoundsMin, true);
            }
            if (equals(transform.position.x, xBoundsMin, 0.001f) && transform.position.z > zBoundsMax)
            {
                crossAngle(zBoundsMin, zBoundsMax, true);
            }

            transform.position += transform.forward * Time.deltaTime * speed;
            Debug.Log("new position: " + transform.position);
        }

        //Update character position, and rotation around the cube while right arrow button is held down
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < xBoundsMin && equals(transform.position.z, zBoundsMax, 0.001f))
            {
                crossAngle(xBoundsMin, zBoundsMax, false);
            }
            if (equals(transform.position.x, xBoundsMin, 0.001f) && transform.position.z < zBoundsMin)
            {
                crossAngle(xBoundsMin, zBoundsMin, false);
            }
            if (equals(transform.position.z, zBoundsMin, 0.001f) && transform.position.x > xBoundsMax)
            {
                crossAngle(xBoundsMax, zBoundsMin, false);
            }
            if (equals(transform.position.x, xBoundsMax, 0.001f) && transform.position.z > zBoundsMax)
            {
                crossAngle(xBoundsMax, zBoundsMax, false);
            }

            transform.position += transform.forward * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (climbing)
            {
                transform.position += transform.up * Time.deltaTime * speed;
            }
            else if (grounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) && climbing)
        {
            transform.position -= transform.up * Time.deltaTime * speed;
        }
    }

    void triggerAnimations()
    {
        //Update animations and rotation the instance a button is pressed down
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("Run", true);
            animator.SetBool("Stop", false);
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                goingRight = true;
                angle -= 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
            }
            else
            {
                angle += 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
            }
        }

        //Trigger climbing animation the instance up/down arrows are pressed
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (climbing)
            {
                transform.localEulerAngles = new Vector3(0f, 180, 0f);
                animator.SetBool("Climb", true);
            }
            else if (grounded)
            {
                animator.SetBool("Jump", true);
                jumping = true;
            }
        }

        //Stop climbing animation the instance up/down arrows are released
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (climbing)
            {
                transform.localEulerAngles = new Vector3(0f, 180, 0f);
                animator.SetBool("Climb", false);
            }
        }

        //Update animations and rotation the instance a button is released
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("Run", false);
            animator.SetBool("Stop", true);
            if (goingRight)
            {
                goingRight = false;
                angle += 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
            }
            else
            {
                angle -= 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
            }
        }
    }
    

    private void crossAngle(float xPos, float zPos, bool fromRight)
    {
        if(fromRight) {
            side = (side + 1) % 4;
            angle += 90;            
        }
        else {
            side = (side - 1) % 4;
            angle -= 90;
        }
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }

    private bool equals(float a, float b, float err)
    {
        return a < b + err && a > b - err;
    }
}
