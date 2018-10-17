using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour {

    /**********************\
        Tunable fields
    \**********************/

    public float speed = 0.17f;
    public float jumpSpeed = 0.9f;
    public Transform world;
    public GameObject winText;
    public GameConstants.AnimationTypes currentAnimation; 

    public float xBounds = 1.6f;
    public float zBounds = 1.6f;
    public float topCube = 3.0f;    

    [HideInInspector]
    public int side = 2;
    [HideInInspector]
    public bool win = false;
    [HideInInspector]
    public bool isMultiplayer = false;
    [HideInInspector]
    public bool dead = false;

    /*********************\
        Private fields
    \*********************/

    Animator animator;
    Rigidbody rb;

    // 0 & 2 = Moving along X
    // 1 & 3 = Moving along Z
    float angle = 180;

    bool climbing = false;
    bool moving = false;
    bool goingRight = false;
    bool grounded = true;
    bool jumping = false;    

    float timeBetweenJumps = 0.3f;
    float groundedTime = 0.0f;
    bool oneFingerReleased = false;

    /*********************\
        Unity functions
    \*********************/

    void Awake()
    {
        transform.SetParent(GameObject.Find("WorldContainer").transform, false);
    }

    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
	}	
	
	void Update () {

        if (win)
            return;

        //Ensure we only travel in the appropriate dimensions
        EnsureConsistentMovement();

        //Go around the corners when we get to them
        ChangeSides();

        //Update character position and rotation
        UpdateCharacterPosition();

        //Trigger animations based on user input
        TriggerAnimations();
    }

    public void Reset()
    {
        climbing = false;
        moving = false;
        goingRight = false;
        grounded = true;
        jumping = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActiveAndEnabled) return;
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder enter");
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.useGravity = false;
            climbing = true;
            jumping = false;            
            grounded = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isActiveAndEnabled) return;
        if (other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Ladder exit");
            rb.useGravity = true;
            climbing = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isActiveAndEnabled) return;
        if (collision.gameObject.tag == "Rock" && !dead)
        {
            if (isMultiplayer) {
                PhotonNetwork.Destroy(collision.gameObject);
                GetComponent<PhotonView>().RPC("die", PhotonTargets.AllBuffered);
            }
                
            Debug.Log("Killing character");
            animator.SetTrigger("Die");
            dead = true;
            StartCoroutine(Dying());            
        }

        if (collision.gameObject.tag == GameConstants.ARPLAYERTAG)
        {
            var colliderOther = collision.gameObject.GetComponent<Collider>();
            var colliderThis = GetComponent<Collider>();
            Physics.IgnoreCollision(colliderOther, colliderThis, true);
        }
    }

    bool IsGrounded()
    {
        float distToGround = GetComponent<BoxCollider>().bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }

    /*********************************\
        Helper and utility functions
    \*********************************/ 

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
        } else
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

    void EnsureConsistentMovement()
    {
        switch (side)
        {
            case 0:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zBounds);
                break;
            case 1:
                transform.localPosition = new Vector3(xBounds, transform.localPosition.y, transform.localPosition.z);
                break;
            case 2:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -zBounds);
                break;
            case 3:
                transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, transform.localPosition.z);
                break;
        }
    }

    void ChangeSides()
    {
        if (transform.localPosition.x > xBounds) // Change to side 1
        {
            side = 1;
            angle = 90;
            if(transform.localPosition.z > 0) 
            {
                transform.localPosition = new Vector3(xBounds, transform.localPosition.y, zBounds - (transform.localPosition.x - xBounds)); 
            } else 
            {
                transform.localPosition = new Vector3(xBounds, transform.localPosition.y, -zBounds + (transform.localPosition.x - xBounds));
            }
        }
        else if (transform.localPosition.z < -zBounds) // Change to side 2
        {
            side = 2;
            angle = 180;
            if (transform.localPosition.x > 0) 
            {
                transform.localPosition = new Vector3(xBounds + (transform.localPosition.z + zBounds), transform.localPosition.y, -zBounds);
            } else 
            {
                transform.localPosition = new Vector3(-xBounds - (transform.localPosition.z + zBounds), transform.localPosition.y, -zBounds);
            }
        }
        else if (transform.localPosition.x < -xBounds) // Change to side 3
        {
            side = 3;
            angle = 270;
            if(transform.localPosition.z > 0) 
            {
                transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, zBounds + (transform.localPosition.x + xBounds));
            } else 
            {
                transform.localPosition = new Vector3(-xBounds, transform.localPosition.y, -zBounds - (transform.localPosition.x + xBounds));
            }
        }
        else if (transform.localPosition.z > zBounds) // Change to side 0
        {
            side = 0;
            angle = 0;
            if(transform.localPosition.x > 0)
            {
                transform.localPosition = new Vector3(xBounds - (transform.localPosition.z - zBounds), transform.localPosition.y, zBounds);
            } else 
            {
                transform.localPosition = new Vector3(-xBounds + (transform.localPosition.z - zBounds), transform.localPosition.y, zBounds);
            }
        }
    }

    int firstTouchFingerID = -1;
    float touchDir = 0f;
    bool ignoreBothTouch = false;

    void UpdateCharacterPosition()
    {
        bool bothTouch = false;
        bool movingVertically = Math.Abs(rb.velocity.y) > 0.001f;

        if(dead)
        {
            // Don't update the position if the player has died
            return;
        }

        if (IsGrounded())
        {
            groundedTime += Time.deltaTime;
            jumping = false;
        }            

        // Only one touch, we go in that direction
        if (Input.touchCount == 1)
        {
            touchDir = Input.GetTouch(0).position.x < Screen.width / 2 ? -1f : 1f;
            firstTouchFingerID = Input.GetTouch(0).fingerId;
            oneFingerReleased = true;
        }
        // Two touches, we keep the same direction but test if we have one touch on each side (for jumping/climbing)
        else if (Input.touchCount == 2)
        {
            foreach (Touch t in Input.touches)
            {
                if (t.fingerId != firstTouchFingerID && !ignoreBothTouch)
                {
                    float floatScreenWidth = (float)Screen.width;
                    bothTouch = (t.position.x < floatScreenWidth / 2f ? -1f : 1f) != touchDir;
                }
            }
        }
        // Anything else = Reset and do nothing
        else
        {
            firstTouchFingerID = -1;
            touchDir = 0f;
        }

        Vector3 yVelocity = world.up * rb.velocity.y;
        float hAxis = Input.GetAxisRaw("Horizontal");
        // Ignore xAxis if we touched the screen on mobile
        hAxis = firstTouchFingerID != -1 ? touchDir : hAxis;
       
        if (Input.GetKey(KeyCode.UpArrow) || bothTouch)
        {
            if (climbing) {
                transform.position += transform.up * Time.deltaTime * speed;
                if (transform.localPosition.y > topCube)
                {
                    //WIN!!
                    transform.localPosition = new Vector3(transform.localPosition.x, topCube, transform.localPosition.z);
                    animator.SetBool("Climb", false);
                    //animator.SetTrigger("Win");
                    win = true;
                    winText.SetActive(true);
                    return;
                }
            }

            else if ((IsGrounded() && groundedTime > timeBetweenJumps) || (oneFingerReleased && !movingVertically && IsGrounded())) {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
                jumping = true;
                grounded = false;
                groundedTime = 0.0f;
                oneFingerReleased = false;
            }
        }

        if (hAxis != 0f && (!climbing || !bothTouch))
        {
            moving = true;
            goingRight = hAxis == 1f;
            transform.localPosition += hAxis * getDirection(true) * Time.deltaTime * speed;
        }
        else
        {
            moving = false;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void TriggerAnimations()
    {
        if (!isActiveAndEnabled) return;
        if (goingRight) {
            transform.localEulerAngles = new Vector3(0f, angle - 90, 0f);
        }
        else 
        {
            transform.localEulerAngles = new Vector3(0f, angle + 90, 0f);
        }

        if (IsGrounded()){
            animator.SetBool("Fall", false);
            animator.SetBool("Climb", false);
            animator.SetBool("Jump", false);
            if (moving)
            {
                animator.SetBool("Run", true);
                animator.SetBool("Stop", false);
                currentAnimation = GameConstants.AnimationTypes.running;
            }
            else
            {
                animator.SetBool("Run", false);
                animator.SetBool("Stop", true);
                currentAnimation = GameConstants.AnimationTypes.stopped;
            }

        }
        else {
            animator.SetBool("Run", false);
            animator.SetBool("Stop", false);
            if (climbing){
                animator.SetBool("Climb", true);
                animator.SetBool("Jump", false);
                currentAnimation = GameConstants.AnimationTypes.climbing;
            }
            else if (jumping)
            {
                animator.SetBool("Run", false);
                animator.SetBool("Jump", true);
                currentAnimation = GameConstants.AnimationTypes.jumping;
            }
            else {
                animator.SetBool("Fall", true);
                currentAnimation = GameConstants.AnimationTypes.falling;
            }
        }
    }

    public IEnumerator Dying()
    {
        yield return new WaitForSeconds(3); // Length of dying animation
        // Move player to initial position
        //transform.position = new Vector3(16f, 2.5f, zBounds);
        angle = 180;
        transform.localPosition = new Vector3(-xBounds + 0.1f, 0.1f, -zBounds);        
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
        side = 2;
        dead = false;
    }
}
