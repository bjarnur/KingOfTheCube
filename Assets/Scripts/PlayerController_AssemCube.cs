using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController_AssemCube : MonoBehaviour {

    public float speed = 1f;
    public float jumpSpeed = 5f;
    public GameObject winText;
    public GameConstants.AnimationTypes currentAnimation;
    public bool isMultiplayer = true;

    [HideInInspector]
    public int side = 0;
    [HideInInspector]
    public bool win = false;
    [HideInInspector]
    public bool dead = false;

    Vector3 movement;
    Animator animator;
    Rigidbody rb;

    float xBounds, zBounds, topCube;
    float angle = 0;

    bool climbing = false;
    bool grounded = false;
    bool jumping = false;
   
    bool moving = false;
    bool goingRight = false;
    
    void Awake()
    {
        if (!isMultiplayer) return;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "AssembleCube_AI_test")
        {
            transform.SetParent(GameObject.Find("Wrapper").transform);
        }
        else
        {
            SceneManager.sceneLoaded += OnSceneChangedCallback;
        }
    }

    void Start ()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // TODO: Get automaticaly the limits of the current cube
        xBounds = 16f;
        zBounds = 16f;
        topCube = 30f;

        // Move player to initial position
        //transform.position = new Vector3(16f, 2.5f, zBounds);
        //transform.localEulerAngles = new Vector3(0f, angle, 0f);

    }

    private void FixedUpdate()
    {
        float mov;
        //Don't move if it's dead
        mov = dead ? 0 : Input.GetAxisRaw("Horizontal");

        MovePlayer(mov);

        // Animate
        bool running = mov != 0f;
        moving = running;

        //animator.SetBool("Run", running);
        TriggerAnimations();
    }

    void MovePlayer(float mov)
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (climbing)
            {
                transform.position += transform.up * Time.deltaTime * speed;
                transform.localEulerAngles = new Vector3(0f, angle + 180, 0f); // Face the edge of the cube

                if (transform.position.y > topCube)
                {
                    //WIN!!
                    transform.position = new Vector3(transform.position.x, topCube, transform.position.z);
                    animator.SetBool("Climb", false);
                    //animator.SetTrigger("Win");
                    win = true;
                    winText.SetActive(true);
                }
            }
            else if (grounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
                jumping = true;
                grounded = false;
            }
        }

        else {
            
            

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
            else if(climbing)
            {
                transform.localEulerAngles = new Vector3(0f, angle + 180, 0f); // Face the the cube
            }
            else
            {
                transform.localEulerAngles = new Vector3(0f, angle, 0f); // Face the edge of the cube
            }
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
        if (!isActiveAndEnabled) return;
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
        if (!isActiveAndEnabled) return;
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
        if (!isActiveAndEnabled) return;
        if (collision.gameObject.CompareTag("Floor"))
        {
            grounded = true;
            if (jumping)
            {
                jumping = false;
            }
            TriggerAnimations();
        }

        if (collision.gameObject.tag == "Rock" && !dead)
        {
            animator.SetTrigger("Die");
            dead = true;
            //Play hurt sound effect
            GetComponent<AudioSource>().Play();
            StartCoroutine(Dying());
            if(isMultiplayer)          
                GetComponent<PhotonView>().RPC("die", PhotonTargets.AllBuffered);
        }
        
        if(collision.gameObject.tag == GameConstants.UNITYPLAYERTAG)
        {
            var colliderOther = collision.gameObject.GetComponent<Collider>();
            var colliderThis = GetComponent<Collider>();
            Physics.IgnoreCollision(colliderOther, colliderThis, true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (!isActiveAndEnabled) return;        
        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Exiting floor");
            grounded = false;
        }
        TriggerAnimations();        
    }

    public IEnumerator Dying()
    {
        yield return new WaitForSeconds(3); // Length of dying animation
        // Move player to initial position
        transform.position = new Vector3(16f, 2.5f, zBounds);
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
        side = 0;
        dead = false;
    }


    //void TriggerAnimations()
    //{
    //    if (grounded && !jumping)
    //    {
    //        //animator.SetBool("Fall", false);
    //        animator.SetBool("Climb", false);
    //        animator.SetBool("Jump", false);
    //        //if (moving)
    //        //{
    //        //    animator.SetBool("Run", true);
    //        //}
    //        //else
    //        //{
    //        //    animator.SetBool("Run", false);
    //        //}
    //    }
    //    else if (climbing)
    //    {
    //        animator.SetBool("Climb", true);
    //        animator.SetBool("Jump", false);
    //    }
    //    else if (jumping)
    //    {
    //        animator.SetBool("Jump", true);
    //    }
    //    else if (!climbing)
    //    {
    //        //animator.SetBool("Fall", true);
    //        animator.SetBool("Run", true);
    //        animator.SetBool("Climb", false);
    //    }
    //}

    void TriggerAnimations()
    {
        if (!isActiveAndEnabled) return;
        if (goingRight)
        {
            //transform.localEulerAngles = new Vector3(0f, angle - 90, 0f);
        }
        else
        {
            //transform.localEulerAngles = new Vector3(0f, angle + 90, 0f);
        }

        if (grounded && !jumping)
        {
            animator.SetBool("Fall", false);
            animator.SetBool("Climb", false);
            animator.SetBool("Jump", false);
            if (moving)
            {                
                animator.SetBool("Run", true);
                currentAnimation = GameConstants.AnimationTypes.running;                
            }
            else
            {
                animator.SetBool("Run", false);
                currentAnimation = GameConstants.AnimationTypes.stopped;
            }
        }
        else if (climbing)
        {
            animator.SetBool("Climb", true);
            animator.SetBool("Jump", false);
            currentAnimation = GameConstants.AnimationTypes.climbing;
        }
        else if (jumping)
        {
            animator.SetBool("Jump", true);
            currentAnimation = GameConstants.AnimationTypes.jumping;
        }
        else if (!grounded)
        {
            animator.SetBool("Fall", true);
            currentAnimation = GameConstants.AnimationTypes.falling;
        }
    }

    void OnSceneChangedCallback(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "AssembleCube_AI_test")
        {
            Debug.Log("Setting character parent");
            transform.SetParent(GameObject.Find("Wrapper").transform);
        }
    }
}
