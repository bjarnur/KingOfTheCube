using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour {

    public float speed = 1f;
    public GameObject cube;
    public GameObject rock;
    public GameObject hand;

    Vector3 movement;
    Animator anim;
    Rigidbody rb;

    float xBounds, zBounds;
    int side = 0;
    float angle = 0;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // TODO: Get the limits of the current cube
        xBounds = 2.3f;
        zBounds = 2.3f;

        // Move king to initial position
        transform.position = new Vector3(0f, 0f, xBounds); 
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
    }
	
    private void FixedUpdate()
    {
        float mov = Input.GetAxisRaw("Horizontal");

        // Move
        MoveKing(mov);

        // Animate
        bool running = mov != 0f;
        anim.SetBool("IsRunning", running);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Throw");
        }
        // TODO: Don't move the king while he is throwing something
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

    void ThrowObject() // Event that is called during the throwing animation
    {
        rock.transform.position = hand.transform.position;
        rock.GetComponent<Rigidbody>().velocity = Vector3.zero;
        rock.SetActive(true);
    }

    
}
