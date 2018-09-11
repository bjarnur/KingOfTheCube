using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    float speed = 2f;
    int moveInX = 1;
    int moveInZ = 0;

    Animator animator;
    Rigidbody rb;

    Vector3 toFront;
    Vector3 toBack;
    Vector3 toRight;
    Vector3 toLeft;
    float angle = 0;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        toFront = Vector3.forward;
        toBack = Vector3.back;
        toRight = Vector3.right;
        toLeft = Vector3.left;
	}
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetBool("Run", true);
            animator.SetBool("Stop", false);
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                //transform.localEulerAngles = transform.right;//new Vector3(0f, angle, 0f);
            }
            else
            {
                //rb.rotation = Quaternion.LookRotation(toLeft);
            }
        }       

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetBool("Run", false);
            animator.SetBool("Stop", true);
            ////rb.rotation = Quaternion.LookRotation(toFront);
        }
       

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
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //rigidbody.AddForce(new Vector3(0, 0, force), ForceMode.VelocityChange);
            //rigidbody.rotation = Quaternion.LookRotation(Vector3.forward);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //rigidbody.AddForce(new Vector3(0, 0, -force), ForceMode.VelocityChange);
            //rigidbody.rotation = Quaternion.LookRotation(Vector3.back);
        }
       

        /*
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.AddForce(new Vector3(-force * moveInX, 0, -force * moveInZ), ForceMode.VelocityChange);
            rigidbody.rotation = Quaternion.LookRotation(Vector3.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.AddForce(new Vector3(force * moveInX, 0, force * moveInZ), ForceMode.VelocityChange);
            rigidbody.rotation = Quaternion.LookRotation(Vector3.right);
        }
        */
    }

    private bool equals(float a, float b, float err)
    {
        return a < b + err && a > b - err;
    }
}
