using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    
    Animator animator;
    Rigidbody rb;

    float speed = 2f;
    float angle = 0;

    bool climbing = false;
    bool goingRight = false;

    
    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
	}
	
	
	void Update () {

        //Update animations and rotation the instance a button is pressed down
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("Run", true);
            animator.SetBool("Stop", false);
            if(Input.GetKeyDown(KeyCode.RightArrow))
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
            if(goingRight)
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
       
        //Update character position, and rotation around the cube while left arrow button is held down
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > 4.5f && equals(transform.position.z, 4.5f, 0.001f))
            {
                angle += 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
                transform.position = new Vector3(4.5f, 1f, 4.5f);
            }
            if (equals(transform.position.x, 4.5f, 0.001f) && transform.position.z < -4.5f)
            {
                transform.localEulerAngles = new Vector3(0f, 0, 0f);
                transform.position = new Vector3(4.5f, 1f, -4.5f);
                angle += 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);                
            }
            if (equals(transform.position.z, -4.5f, 0.001f) && transform.position.x < -4.5f)
            {                
                transform.localEulerAngles = new Vector3(0f, 0, 0f);
                transform.position = new Vector3(-4.5f, 1f, -4.5f);
                angle += 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);                
            }
            if (equals(transform.position.x, -4.5f, 0.001f) && transform.position.z > 4.5f)
            {
                transform.localEulerAngles = new Vector3(0f, 0, 0f);
                transform.position = new Vector3(-4.5f, 1f, 4.5f);
                angle += 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);                
            }

            transform.position += transform.forward * Time.deltaTime * speed;
        }

        //Update character position, and rotation around the cube while right arrow button is held down
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < -4.5f && equals(transform.position.z, 4.5f, 0.001f))
            {
                transform.localEulerAngles = new Vector3(0f, 0, 0f);
                transform.position = new Vector3(-4.5f, 1f, 4.5f);
                angle -= 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
            }
            if (equals(transform.position.x, -4.5f, 0.001f) && transform.position.z < -4.5f)
            {
                transform.localEulerAngles = new Vector3(0f, 0, 0f);
                transform.position = new Vector3(-4.5f, 1f, -4.5f);
                angle -= 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);                
            }            
            if (equals(transform.position.z, -4.5f, 0.001f) && transform.position.x > 4.5f)
            {
                transform.localEulerAngles = new Vector3(0f, 0, 0f);
                transform.position = new Vector3(4.5f, 1f, -4.5f);
                angle -= 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
            }
            if (equals(transform.position.x, 4.5f, 0.001f) && transform.position.z > 4.5f)
            {
                transform.localEulerAngles = new Vector3(0f, 0, 0f);
                transform.position = new Vector3(4.5f, 1f, 4.5f);
                angle -= 90;
                transform.localEulerAngles = new Vector3(0f, angle, 0f);
            }

            transform.position += transform.forward * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow) && climbing)
        {
            transform.position += transform.up * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.DownArrow) && climbing)
        {
            transform.position -= transform.up * Time.deltaTime * speed;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder enter");
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

    private bool equals(float a, float b, float err)
    {
        return a < b + err && a > b - err;
    }
}
