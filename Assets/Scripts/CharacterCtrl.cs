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
    const float xBounds = 4.5f;
    const float zBounds = 4.5f;

    bool climbing = false;
    bool moving = false;
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
        EnsureConsistentMovement();

        //Go around the corners when we get to them
        ChangeSides();

        //Update character position and rotation
        UpdateCharacterPosition();

        //Trigger animations based on user input
        TriggerAnimations();
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
            TriggerAnimations();
        }
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


    void EnsureConsistentMovement()
    {
        switch (side)
        {
            case 0:
                transform.position = new Vector3(transform.position.x, transform.position.y, zBounds);
                break;
            case 1:
                transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x, transform.position.y, -zBounds);
                break;
            case 3:
                transform.position = new Vector3(-xBounds, transform.position.y, transform.position.z);
                break;
        }
    }

    void ChangeSides()
    {
        if (transform.position.x > xBounds) // Change to side 1
        {
            side = 1;
            angle = 90;
            if(transform.position.z > 0) 
            {
                transform.position = new Vector3(xBounds, transform.position.y, zBounds - (transform.position.x - xBounds)); 
            } else 
            {
                transform.position = new Vector3(xBounds, transform.position.y, -zBounds + (transform.position.x - xBounds));
            }
        }
        else if (transform.position.z < -zBounds) // Change to side 2
        {
            side = 2;
            angle = 180;
            if (transform.position.x > 0) 
            {
                transform.position = new Vector3(xBounds + (transform.position.z + zBounds), transform.position.y, -zBounds);
            } else 
            {
                transform.position = new Vector3(-xBounds - (transform.position.z + zBounds), transform.position.y, -zBounds);
            }
        }
        else if (transform.position.x < -xBounds) // Change to side 3
        {
            side = 3;
            angle = 270;
            if(transform.position.z > 0) 
            {
                transform.position = new Vector3(-xBounds, transform.position.y, zBounds + (transform.position.x + xBounds));
            } else 
            {
                transform.position = new Vector3(-xBounds, transform.position.y, -zBounds - (transform.position.x + xBounds));
            }
        }
        else if (transform.position.z > zBounds) // Change to side 0
        {
            side = 0;
            angle = 0;
            if(transform.position.x > 0)
            {
                transform.position = new Vector3(xBounds - (transform.position.z - zBounds), transform.position.y, zBounds);
            } else 
            {
                transform.position = new Vector3(-xBounds + (transform.position.z - zBounds), transform.position.y, zBounds);
            }
        }
    }

    void UpdateCharacterPosition()
    {
        if(Input.GetKey(KeyCode.LeftArrow)) 
        {
            goingRight = false;
            moving = true;
            switch (side) 
            {
                case 0:
                    rb.velocity = new Vector3(speed, rb.velocity.y, 0);
                    break;
                case 1:
                    rb.velocity = new Vector3(0, rb.velocity.y, -speed);
                    break;
                case 2:
                    rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
                    break;
                case 3:
                    rb.velocity = new Vector3(0, rb.velocity.y, speed);
                    break;
            }
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            moving = true;
            goingRight = true;
            switch (side)
            {
                case 0:
                    rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
                    break;
                case 1:
                    rb.velocity = new Vector3(0, rb.velocity.y, speed);
                    break;
                case 2:
                    rb.velocity = new Vector3(speed, rb.velocity.y, 0);
                    break;
                case 3:
                    rb.velocity = new Vector3(0, rb.velocity.y, -speed);
                    break;
            }
        } else {
            moving = false;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
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
