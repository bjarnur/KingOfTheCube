//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARController : MonoBehaviour
{
    public Transform ARCoreDevice;
    public GameObject UIScanning;
    public GameObject UIWinning;
    public GameObject garden;
    public GameObject king;
    public GameObject playerOne;
    public GameObject smoke;
    public GameObject Controls;
    public GameObject UpButton;
    public GameObject BombButton;
    public Transform world;
    public Transform unitCube;
    public bool isMultiplaer;
    public TutorialController tutorial;

    private List<AugmentedImage> m_AugmentedImages = new List<AugmentedImage>();
    private AugmentedImage KOTCImage = null;
    private Anchor KOTCAnchor = null;
    private GameObject playerInstance;
    private bool playingKing = false;

    private const float xBoundsMin = -1.6f;
    private const float xBoundsMax = 1.6f;
    private const float zBoundsMin = -1.6f;
    private const float zBoundsMax = 1.6f;

    public void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Controls.SetActive(false);
    }

    public void Update() {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        // Check that motion tracking is tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(m_AugmentedImages, TrackableQueryFilter.Updated);

        foreach (var image in m_AugmentedImages)
        {           
            if (image.TrackingState == TrackingState.Tracking && KOTCImage == null)
            {
                Debug.Log("Tracking OK");
                KOTCImage = image;
                KOTCAnchor = image.CreateAnchor(image.CenterPose);

                world.SetParent(KOTCAnchor.transform, false);
                world.localPosition -= world.up * 0.5f;
                Transform gardenObj = Instantiate(garden).transform;
                gardenObj.SetParent(world.transform, false);
                gardenObj.tag = GameConstants.GameObjectsTags.gardenObject;

                GetComponent<LevelInstatiator>().world = world;
                GetComponent<LevelInstatiator>().buildLevel();

                Controls.SetActive(true);

                if (!isMultiplaer)
                { 
                    readyPlayerOne();
                    readyKing();
                    //tutorial.InitiateForPlayer();
                }
                else
                {
                    int PlayerIndex = GameConstants.NetworkedPlayerID;
                    if (PlayerIndex == 0)
                    { 
                        spawnKing();
                        //tutorial.InitiateForKing();
                    }
                    else
                    { 
                        spawnPretender(PlayerIndex);
                        //tutorial.InitiateForPlayer();
                    }
                }
                //tutorial.Begin();
            }
            else if (image.TrackingState == TrackingState.Stopped) {
                Debug.Log("Tracking Stopped");
                world.SetParent(null, true);

                KOTCImage = null;
                GameObject.Destroy(KOTCAnchor);
                KOTCAnchor = null;
            }
        }

        UIScanning.SetActive(KOTCImage == null);
        if (world.parent == null)
        {
            world.localRotation = Quaternion.Euler(0, world.localRotation.eulerAngles.y, 0);
        } else
        {
            world.localRotation = Quaternion.Euler(-KOTCAnchor.transform.localEulerAngles.x, world.localEulerAngles.y, -KOTCAnchor.transform.localEulerAngles.z);
        }
    }

    public void ShowInstructions()
    {
        //tutorial.Begin();
    }

    public void ActivatePlayerLeft()
    {
        //Debug.Log("Activate Left");

        if (playerInstance == null)
        {
            //Debug.Log("Player instance null");
            return;
        }

        if (playingKing)
        {
            KingController_AR controller = playerInstance.GetComponent<KingController_AR>();
            controller.moveLeft = true;
        }
        else
        {
            CharacterCtrl controller = playerInstance.GetComponent<CharacterCtrl>();
            controller.moveLeft = true;
        }
    }
    public void DeActivatePlayerLeft()
    {
        //Debug.Log("Deactivating Light");

        if (playerInstance == null)
        {
            //Debug.Log("Player instance null");
            return;
        }

        if (playingKing)
        { 
            KingController_AR controller = playerInstance.GetComponent<KingController_AR>();
            controller.moveLeft = false;
        }
        else
        { 
            CharacterCtrl controller = playerInstance.GetComponent<CharacterCtrl>();
            controller.moveLeft = false;
        }
    }

    public void ActivatePlayerRight()
    {
        //Debug.Log("Activating Right");

        if (playerInstance == null)
        {
            //Debug.Log("Player instance null");
            return;
        }

        if (playingKing)
        {
            KingController_AR controller = playerInstance.GetComponent<KingController_AR>();
            controller.moveRight = true;
        }
        else
        {
            CharacterCtrl controller = playerInstance.GetComponent<CharacterCtrl>();
            controller.moveRight = true;
        }
    }

    public void DeActivatePlayerRight()
    {
        //Debug.Log("Deactivating Right");

        if (playerInstance == null)
        {
            //Debug.Log("Player instance null");
            return;
        }

        if (playingKing)
        {
            KingController_AR controller = playerInstance.GetComponent<KingController_AR>();
            controller.moveRight = false;
        }
        else
        {
            CharacterCtrl controller = playerInstance.GetComponent<CharacterCtrl>();
            controller.moveRight = false;
        }
    }

    public void ActivatePlayerUp()
    {
        //Debug.Log("Activating Up");

        if (playerInstance == null)
        {
            //Debug.Log("Player instance null");
            return;
        }

        if (playingKing)
        {
            KingController_AR controller = playerInstance.GetComponent<KingController_AR>();
            controller.dropBomb = true;
        }
        else
        {
            CharacterCtrl controller = playerInstance.GetComponent<CharacterCtrl>();
            controller.moveUp = true;
        }
    }

    public void DeactivatePlayerUp()
    {
        //Debug.Log("Activating Up");

        if (playerInstance == null)
        {
            //Debug.Log("Player instance null");
            return;
        }

        if (playingKing)
        {
            KingController_AR controller = playerInstance.GetComponent<KingController_AR>();
            controller.dropBomb = false;
        }
        else
        {
            CharacterCtrl controller = playerInstance.GetComponent<CharacterCtrl>();
            controller.moveUp = false;
        }
    }

    public void ToggleWorldLock() {
        if(world.parent == null) {
            world.parent = KOTCAnchor.transform;
            world.localPosition = Vector3.zero;
            world.localRotation = Quaternion.identity;
            world.localScale = Vector3.one * 0.1f;
        } 
        else {
            world.SetParent(null, true);
        }
    }    

    void readyPlayerOne()
    {
        playerInstance = Instantiate(playerOne, world, false);    
        CharacterCtrl controller = playerInstance.GetComponent<CharacterCtrl>();
        NetworkPlayer networkPlayer = playerInstance.GetComponent<NetworkPlayer>();
        Rigidbody playerRigidbody = playerInstance.GetComponent<Rigidbody>();

        networkPlayer.StopCoroutine(GameConstants.RPCTags.updateNetworked);
        networkPlayer.enabled = false;
        controller.enabled = true;
        controller.world = world;
        controller.winText = UIWinning;
        playerRigidbody.useGravity = true;

        ResetPlayer();
    }

    void readyKing()
    {
        GameObject kingInstance = Instantiate(king);
        kingInstance.transform.SetParent(GameObject.FindWithTag(GameConstants.GameObjectsTags.worldContainer).transform, false);

        KingController_AR kCtrl = kingInstance.GetComponent<KingController_AR>();
        kCtrl.isAI = true;
        kCtrl.isMultiplayer = false;
        kCtrl.enabled = true;        
        kCtrl.setPlayer(playerInstance);
        kingInstance.transform.localPosition = new Vector3(xBoundsMin - 0.5f, 3.1f, zBoundsMin);
    }

    void spawnKing()
    {
        object[] InstanceData = new object[1];
        InstanceData[0] = "AR";

        Vector3 spawn = GameObject.FindWithTag(GameConstants.GameObjectsTags.controller)
                            .GetComponent<LevelInstatiator>()
                            .instantiateSpawnPoint(0);
        
        GameObject newPlayer = PhotonNetwork.Instantiate(GameConstants.PunNames.arKing, Vector3.zero, Quaternion.identity, 0, InstanceData);
        newPlayer.transform.SetParent(GameObject.FindWithTag(GameConstants.GameObjectsTags.worldContainer).transform, false);
        newPlayer.transform.localPosition = spawn;
        playerInstance = newPlayer;
        playingKing = true;

        KingController_AR controller = newPlayer.GetComponent<KingController_AR>();
        NetworkKing networkPlayer = newPlayer.GetComponent<NetworkKing>();
        Rigidbody playerRigidbody = newPlayer.GetComponent<Rigidbody>();

        controller.enabled = true;
        controller.isAI = false;
        networkPlayer.enabled = true;
        playerRigidbody.useGravity = true;

        UpButton.SetActive(false);
    }

    void spawnPretender(int playerNumber)
    {        
        Vector3 spawn = GameObject.FindWithTag(GameConstants.GameObjectsTags.controller)
                        .GetComponent<LevelInstatiator>()
                        .instantiateSpawnPoint(playerNumber);
      
        GameObject newPlayer = null;
        switch (playerNumber)
        {
            case 1:
                newPlayer = PhotonNetwork.Instantiate("Player_One", Vector3.zero, Quaternion.identity, 0);
                break;
            case 2:
                newPlayer = PhotonNetwork.Instantiate("Player_Two", Vector3.zero, Quaternion.identity, 0);
                break;
            case 3:
                newPlayer = PhotonNetwork.Instantiate("Player_Three", Vector3.zero, Quaternion.identity, 0);
                break;
            case 4:
                newPlayer = PhotonNetwork.Instantiate("Player_Four", Vector3.zero, Quaternion.identity, 0);
                break;
        }

        CharacterCtrl controller = newPlayer.GetComponent<CharacterCtrl>();
        NetworkPlayer networkPlayer = newPlayer.GetComponent<NetworkPlayer>();
        Rigidbody playerRigidbody = newPlayer.GetComponent<Rigidbody>();
        playerInstance = newPlayer;

        networkPlayer.enabled = true;
        //networkPlayer.StopCoroutine("UpdateNetworked");
        controller.enabled = true;
        controller.winText = UIWinning;
        controller.world = world;
        controller.isMultiplayer = true;        
        playerRigidbody.useGravity = true;

        newPlayer.transform.SetParent(world, false);
        newPlayer.transform.localPosition = spawn;

        BombButton.SetActive(false);
    }

   public void ResetPlayer()
    {
        playerInstance.transform.localPosition = new Vector3(xBoundsMin + 0.13f, 0.3f, zBoundsMin);
        playerInstance.GetComponent<Rigidbody>().useGravity = true;
        playerInstance.GetComponent<CharacterCtrl>().Reset();        
    }
}
