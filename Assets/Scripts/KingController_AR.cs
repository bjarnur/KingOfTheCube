using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController_AR : MonoBehaviour {

    public float speed = 1f;
    public bool isAI;
    public GameObject rockPrefab;
    public GameObject hand;
    [HideInInspector]
    public Transform world;

    GameObject player;
    GameObject rockInstance;

    Vector3 movement;
    Animator anim;
    Rigidbody rb;

    float xBounds, zBounds;
    int side = 2;
    float angle = 180;
    bool throwing = false;
    bool dead = false;

    int dir = 1;

	void Start () {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // TODO: Get automaticaly the limits of the current cube
        xBounds = 1.3f;
        zBounds = 1.3f;

        // Move king to initial position
        transform.localPosition = new Vector3(zBounds / 2f, 3f, xBounds); 
        transform.localEulerAngles = new Vector3(0f, angle, 0f);

        rockInstance = Instantiate<GameObject>(rockPrefab, transform.parent);
    }
	
    public void setPlayer(GameObject player)
    {
        this.player = player;
    }

    private void FixedUpdate()
    {
        if (player.GetComponent<CharacterCtrl>().win)
        {
            //Something happens... 
            anim.SetBool("IsRunning", false);
            if (!dead)
            {
                anim.SetTrigger("Die");
                dead = true;
            }
        }
        else
        {
            float mov;

            // Move
            if (isAI)
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
    }

    float AutoMove()
    {
        // Get in which side the player is
        int playerSide = player.GetComponent<CharacterCtrl>().side;

        // Follow the player, move and throw randomly when in same side
        dir = FollowPlayer(playerSide);

        // Don't move if it's throwing
        float dx = throwing ? 0 : dir;
        MoveKing(dx);
        
        return dx;
    }

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
        }
        else
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
        // Set movement into correct axis
        movement = (mov * getDirection(true)).normalized * speed * Time.deltaTime;
        //rb.MovePosition(transform.position + movement);
        transform.localPosition += movement;

        // Check boundaries and change side if needed
        CheckBounds();

        // Rotation
        if (mov != 0) // Follow the direction of motion
        {
            /*Quaternion newRotation = Quaternion.LookRotation(getDirection(true));
            rb.MoveRotation(newRotation);*/
            if (mov == 1f)
            {
                transform.localEulerAngles = new Vector3(0f, angle - 90, 0f);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0f, angle + 90, 0f);
            }
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
        rockInstance.transform.position = hand.transform.position;
        rockInstance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        rockInstance.SetActive(true);
        // TODO: Add a horizontal force to be more realistic
    }

    void EndThrowing()
    {
        throwing = false;
    }
}
