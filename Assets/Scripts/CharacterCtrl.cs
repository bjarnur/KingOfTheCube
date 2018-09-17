using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour {

    /**********************\
        Tunable fields
    \**********************/

    public float speed = 2f;
    private Vector3 scale = new Vector3(1f, 1f, 1f);

    public void SetScale(Vector3 scale)
    {
        this.scale = scale;
    }

    public void SetWorld(Transform world)
    {
        this.world = world;
    }


    public Transform world;

    /*********************\
        Private fields
    \*********************/

    Animator animator;
    Rigidbody rb;

    int side = 0;    
    float angle = 0;

    //TODO: Set these values appropriately with respect to level dimensions
    private float xBoundsMin = -15.5f;
    private float xBoundsMax = 15.5f;
    private float zBoundsMin = -15.5f;
    private float zBoundsMax = 15.5f;

    bool climbing = false;
    bool goingRight = false;


    /*********************\
        Unity functions
    \*********************/

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        xBoundsMax *= scale.x;
        xBoundsMin *= scale.x;
        zBoundsMax *= scale.y;
        zBoundsMin *= scale.y;
	}	
	
	void Update () {
        
        //Ensure we only travel in the appropriate dimensions
        //ensureConsistentMovement();

        //Trigger animations based on user input
        //triggerAnimations();

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

    void ensureConsistentMovement()
    {
        switch (side)
        {
            case 0:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zBoundsMax);
                break;
            case 1:
                transform.localPosition = new Vector3(xBoundsMax, transform.localPosition.y, transform.localPosition.z);
                break;
            case 2:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zBoundsMin);
                break;
            case 3:
                transform.localPosition = new Vector3(xBoundsMin, transform.localPosition.y, transform.localPosition.z);
                break;
        }
    }

    bool touch = false;
    bool touchLeft = false;

    void updateCharacterPosition()
    {
        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    touch = true;
                    touchLeft = Input.GetTouch(0).position.y < Screen.height / 2;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    touch = false;
                    break;
            }
        }
        //Update character localPosition, and rotation around the cube while left arrow button is held down
        if (Input.GetKey(KeyCode.LeftArrow) || (touch && touchLeft))
        {
            /*if (transform.localPosition.x > xBoundsMax && equals(transform.localPosition.z, zBoundsMax, 0.001f))
            {
                crossAngle(xBoundsMax, zBoundsMax, true);
            }
            if (equals(transform.localPosition.x, xBoundsMax, 0.000001f) && transform.localPosition.z < zBoundsMin)
            {
                crossAngle(xBoundsMax, zBoundsMin, true);
            }
            if (equals(transform.localPosition.z, zBoundsMin, 0.000001f) && transform.localPosition.x < xBoundsMin)
            {
                crossAngle(xBoundsMin, zBoundsMin, true);
            }
            if (equals(transform.localPosition.x, xBoundsMin, 0.000001f) && transform.localPosition.z > zBoundsMax)
            {
                crossAngle(zBoundsMin, zBoundsMax, true);
            }*/

            transform.localPosition -= world.forward * Time.deltaTime * speed;
        }

        //Update character localPosition, and rotation around the cube while right arrow button is held down
        if (Input.GetKey(KeyCode.RightArrow) || (touch && !touchLeft))
        {
            /*if (transform.localPosition.x < xBoundsMin && equals(transform.localPosition.z, zBoundsMax, 0.001f))
            {
                crossAngle(xBoundsMin, zBoundsMax, false);
            }
            if (equals(transform.localPosition.x, xBoundsMin, 0.000001f) && transform.localPosition.z < zBoundsMin)
            {
                crossAngle(xBoundsMin, zBoundsMin, false);
            }
            if (equals(transform.localPosition.z, zBoundsMin, 0.000001f) && transform.localPosition.x > xBoundsMax)
            {
                crossAngle(xBoundsMax, zBoundsMin, false);
            }
            if (equals(transform.localPosition.x, xBoundsMax, 0.000001f) && transform.localPosition.z > zBoundsMax)
            {
                crossAngle(xBoundsMax, zBoundsMax, false);
            }*/

            transform.localPosition += world.forward * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow) && climbing)
        {
            transform.localPosition += transform.up * Time.deltaTime * speed * scale.y;
        }

        if (Input.GetKey(KeyCode.DownArrow) && climbing)
        {
            transform.localPosition -= transform.up * Time.deltaTime * speed * scale.y;
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
                animator.SetBool("StopClimbing", false);
            }
        }

        //Stop climbing animation the instance up/down arrows are released
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            //if (climbing)
            //{
            transform.localEulerAngles = new Vector3(0f, 180, 0f);
            animator.SetBool("Climb", false);
            animator.SetBool("StopClimbing", true);
            //}
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
        transform.localPosition = new Vector3(xPos, transform.localPosition.y, zPos);
    }

    private bool equals(float a, float b, float err)
    {
        return a < b + err && a > b - err;
    }
}
