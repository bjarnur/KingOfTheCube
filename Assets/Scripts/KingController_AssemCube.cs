using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KingController_AssemCube : MonoBehaviour {

    public float speed = 1f;
    public bool isAI;
    public GameObject rock;
    public GameObject hand;
    public GameObject garden;
    public GameObject player;
    public bool isMultiplayer = true;
    public GameConstants.AnimationTypes currentAnimation;

    Vector3 movement;
    Animator anim;
    Rigidbody rb;

    float xBounds, zBounds;
    int side = 0;
    float angle = 0;
    bool throwing = false;
    bool dead = false;

    int dir = 1;

    private float SecondsInactive = 0.0f;

    private NetworkManager networkManager;

    void Awake()
    {
        if (!isMultiplayer) return;

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == GameConstants.SceneNames.OnlineVR)
        {
            transform.SetParent(GameObject.Find(GameConstants.ObjecNames.Wrapper).transform);
        }
        else
        {
            SceneManager.sceneLoaded += OnSceneChangedCallback;
        }
    }

	void Start () {

        networkManager = GameObject.Find(GameConstants.ObjecNames.NetworkManager).GetComponent<NetworkManager>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // TODO: Get automaticaly the limits of the current cube
        xBounds = 13.0f;
        zBounds = 13.0f;

        // Move king to initial position
        transform.position = new Vector3(0f, 30f, xBounds); 
        transform.localEulerAngles = new Vector3(0f, angle, 0f);
    }

    void Update()
    {
        if (SecondsInactive > 30)
        {
            networkManager.IsInactive = true;
            ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
            PropertyTable.Add(GameConstants.NetworkedProperties.Inactive, true);
            PhotonNetwork.player.SetCustomProperties(PropertyTable);
        }
        else
        {
            networkManager.IsInactive = false;
            ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
            PropertyTable.Add(GameConstants.NetworkedProperties.Inactive, false);
            PhotonNetwork.player.SetCustomProperties(PropertyTable);
        }
    }
	
    private void FixedUpdate()
    {

        //if(player.GetComponent<PlayerController_AssemCube>().win)
        if(false)
        {
            //Something happens... 
            anim.SetBool("IsRunning", false);
            if(!dead)
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
            if(running)
            { 
                currentAnimation = GameConstants.AnimationTypes.running;
                SecondsInactive = 0.0f;
            }
            else
            { 
                currentAnimation = GameConstants.AnimationTypes.stopped;
                SecondsInactive += Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("Throw");
                currentAnimation = GameConstants.AnimationTypes.throwing;
            }
        } 
    }

    float AutoMove()
    {
        // Get in which side the player is
        int playerSide = player.GetComponent<PlayerController_AssemCube>().side;

        // Follow the player, move and throw randomly when in same side
        dir = FollowPlayer(playerSide);

        // Don't move if it's throwing
        float dx = throwing ? 0 : dir;
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
        rb.MovePosition(transform.position + movement);

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

    ////////////////////////////////////////////////////////////
    /// Events that are triggered during the throwing animation
    ////////////////////////////////////////////////////////////

    void StartThrowing()
    {
        throwing = true;
    }

    void ThrowObject() 
    {
        if(isMultiplayer)
        {
            GameObject rockInstance = PhotonNetwork.Instantiate("UnityRock", Vector3.zero, Quaternion.identity, 0);
            rockInstance.GetComponent<BombController>().enabled = true;
            rockInstance.transform.position = hand.transform.position;
            rockInstance.transform.localScale = new Vector3(1, 1, 1);
            rockInstance.transform.position = hand.transform.position;
            rockInstance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            rockInstance.SetActive(true);
        }
        else
        {
            GameObject rockInstance = Instantiate(rock);
            rockInstance.GetComponent<BombController>().enabled = true;
            rockInstance.transform.position = hand.transform.position;
            rockInstance.transform.localScale = new Vector3(1, 1, 1);
            rockInstance.transform.position = hand.transform.position;
            rockInstance.GetComponent<Rigidbody>().velocity = Vector3.zero;
            rockInstance.SetActive(true);
        }
    }

    void EndThrowing()
    {
        throwing = false;
    }

    void OnSceneChangedCallback(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == GameConstants.SceneNames.OnlineVR)
        {
            Debug.Log("Setting king parent");
            transform.SetParent(GameObject.Find(GameConstants.ObjecNames.Wrapper).transform);
        }
    }
}
