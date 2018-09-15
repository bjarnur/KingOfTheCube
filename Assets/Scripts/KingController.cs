using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour {

    public float speed = 1f;
    public bool isAI;
    public GameObject rock;
    public GameObject hand;
    public GameObject player;
    //public GameObject cube;

    Vector3 movement;
    Animator anim;
    Rigidbody rb;

    float xBounds, zBounds;
    int side = 0;
    float angle = 0;
    bool throwing = false;

    int dir = 1;

	void Start () {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // TODO: Get automaticaly the limits of the current cube
        xBounds = 1.8f;
        zBounds = 1.8f;

        // Move king to initial position
        transform.position = new Vector3(0f, 0f, xBounds); 
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
    }
	
    private void FixedUpdate()
    {
        float mov; 

        // Move
        if(isAI)
        {
            mov = AutoMove(); 
        }
        else
        {
            // Don't move if it's throwing
            mov = throwing ? 0 : Input.GetAxisRaw("Horizontal");
            MoveKing(mov);
        }

        // Animate
        bool running = mov != 0f;
        anim.SetBool("IsRunning", running);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Throw");
        }
    }

    void MoveKing (float mov)
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

        if(mov != 0)
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

    float AutoMove()
    {
        // TODO: Get in which side the player is
        side = 0;

        // Move randomly in that side
        if(Random.Range(0.0f, 1.0f) < 0.01 || transform.position.x > xBounds || transform.position.x < -xBounds)
        {
            //Change direction
            dir = - dir;
        }
        // Don't move if it's throwing
        int dx = throwing ? 0 : dir;

        movement.Set(dx, 0.0f, 0.0f);
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        if (dir != 0)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(newRotation);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, angle, 0f); // Face the edge of the cube
        }

        // Throw randomly
        float th = Random.Range(0.0f, 1.0f);
        if (th < 0.01 && !throwing)
        {
            anim.SetTrigger("Throw");
        }

        return movement.x;
    }

    ////////////////////////////////////////////////////////////
    /// Events that are triggered during the throwing animation
    ////////////////////////////////////////////////////////////

    void StartThrowing()
    {
        throwing = true;
        Debug.Log("Start throwing");
    }

    void ThrowObject() 
    {
        rock.transform.position = hand.transform.position;
        rock.GetComponent<Rigidbody>().velocity = Vector3.zero;
        rock.SetActive(true);
        // TODO: Add a horizontal force to be more realistic
    }

    void EndThrowing()
    {
        throwing = false;
        Debug.Log("End throwing");
    }


}
