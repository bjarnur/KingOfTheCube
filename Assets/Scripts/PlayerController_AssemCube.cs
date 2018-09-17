using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_AssemCube : MonoBehaviour {

    public float speed = 1f;

    [HideInInspector]
    public int side = 0;

    Vector3 movement;
    Animator animator;
    Rigidbody rb;

    float xBounds, zBounds;
    float angle = 0;

    bool climbing = false;
    bool grounded = false;
    bool jumping = false;
    bool dead = false;


    void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // TODO: Get automaticaly the limits of the current cube
        xBounds = 15.5f;
        zBounds = 15.5f;

        // Move player to initial position
        transform.position = new Vector3(15.5f, 2.5f, xBounds);
        transform.localEulerAngles = new Vector3(0f, angle, 0f);

    }

    private void FixedUpdate()
    {
        float mov;
        //Don't move if it's dead
        mov = dead ? 0 : Input.GetAxisRaw("Horizontal");

        MovePlayer(mov);

        // Animate
        bool running = mov != 0f;
        animator.SetBool("Run", running);

    }

    void MovePlayer(float mov)
    {
        // Check boundaries and change side if needed
        CheckBounds();

        switch (side)
        {
            case 0:
                movement.Set(-mov, 0.0f, 0.0f);
                break;
            case 1:
                movement.Set(0.0f, 0.0f, mov);
                break;
            case 2:
                movement.Set(mov, 0.0f, 0.0f);
                break;
            case 3:
                movement.Set(0.0f, 0.0f, -mov);
                break;
        }
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        if (mov != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(newRotation);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, angle, 0f); // Face the edge of the cube
        }

    }

    void CheckBounds()
    {
        if (transform.position.x > xBounds) // Change to side 1
        {
            side = 1;
            angle = 90;
            transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
        }
        else if (transform.position.z < -zBounds) // Change to side 2
        {
            side = 2;
            angle = 180;
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBounds);
        }
        else if (transform.position.x < -xBounds) // Change to side 3
        {
            side = 3;
            angle = 270;
            transform.position = new Vector3(-xBounds, transform.position.y, transform.position.z);
        }
        else if (transform.position.z > zBounds) // Change to side 0
        {
            side = 0;
            angle = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, zBounds);
        }
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

        if (collision.gameObject.name == "Rock" && !dead)
        {
            animator.SetTrigger("Die");
            dead = true;
            StartCoroutine(Dying());
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

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(3); // Length of dying animatin
        dead = false;
    }


    void TriggerAnimations()
    {
        if (grounded && !jumping)
        {
            //animator.SetBool("Fall", false);
            animator.SetBool("Climb", false);
            animator.SetBool("Jump", false);
            //if (moving)
            //{
            //    animator.SetBool("Run", true);
            //}
            //else
            //{
            //    animator.SetBool("Run", false);
            //}
        }
        else if (climbing)
        {
            animator.SetBool("Climb", true);
            animator.SetBool("Jump", false);
        }
        else if (jumping)
        {
            animator.SetBool("Jump", true);
        }
        else if (!climbing)
        {
            //animator.SetBool("Fall", true);
            animator.SetBool("Run", true);
            animator.SetBool("Climb", false);
        }
    }

}
