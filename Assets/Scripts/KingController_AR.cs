using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController_AR : MonoBehaviour {

    public float speed = 1f;
    public bool isAI;
    public GameObject rock;
    public GameObject hand;
    //public GameObject AR_Controller;

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
        xBounds = 1.4f;
        zBounds = 1.4f;

        // Move king to initial position
        transform.localPosition = new Vector3(0f, 3f, xBounds); 
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

    float AutoMove()
    {
        //GameObject player = AR_Controller.GetComponent<ARController>().playerOne;

        // Get in which side the player is
        //int playerSide = player.GetComponent<CharacterCtrl>().side;
        int playerSide = 2;

        // Follow the player, move and throw randomly when in same side
        dir = FollowPlayer(playerSide);

        // Don't move if it's throwing
        float dx = throwing ? 0 : dir;
        Debug.Log(dx);
        MoveKing(dx);
        
        return dx;
    }

    int FollowPlayer(int playerSide)
    {
        if (side == playerSide)
        {
            // Throw randomly if it's in the same side as the player
            float th = Random.Range(0.0f, 1.0f);
            if (th < 0.01 && !throwing)
            {
                anim.SetTrigger("Throw");
            }
            //Random direction
            if (Random.Range(0.0f, 1.0f) < 0.01)
            {
                //Change direction
                dir = -dir;
            }
        }
        else if (side == (playerSide + 1) % 4) //Go right
        {
            dir = 1;
        }
        else //Go left
        {
            dir = -1;
        }
        return dir;
    }

    void MoveKing(float mov)
    {
        // Check boundaries and change side if needed
        CheckBounds();

        // Set movement into correct axis
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
        //rb.MovePosition(transform.position + movement);
        transform.localPosition += movement;

        // Rotation
        if (mov != 0) // Follow the direction of motion
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(newRotation);
        }
        else // Face the edge of the cube
        {
            transform.localEulerAngles = new Vector3(0f, angle, 0f);
        }
    }

    void CheckBounds()
    {
        if (transform.localPosition.x > xBounds) // Change to side 1
        {
            side = 1;
            angle = 90;
            transform.localPosition = new Vector3(xBounds, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.z < -zBounds) // Change to side 2
        {
            side = 2;
            angle = 180;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -zBounds);
        }
        else if (transform.localPosition.x < -xBounds) // Change to side 3
        {
            side = 3;
            angle = 270;
            transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.z > zBounds) // Change to side 0
        {
            side = 0;
            angle = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zBounds);
        }
    }

    ////////////////////////////////////////////////////////////
    /// Events that are triggered during the throwing animation
    ////////////////////////////////////////////////////////////

    void StartThrowing()
    {
        throwing = true;
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
    }
}
