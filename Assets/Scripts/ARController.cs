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
    public GameObject playerOne;
    public GameObject king;
    public Transform world;
    public Transform unitCube;

    private List<AugmentedImage> m_AugmentedImages = new List<AugmentedImage>();
    private AugmentedImage KOTCImage = null;
    private Anchor KOTCAnchor = null;

    private const float xBoundsMin = -1.55f;
    private const float xBoundsMax = 1.55f;
    private const float zBoundsMin = -1.55f;
    private const float zBoundsMax = 1.55f;

    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Update()
    {
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
            switch (image.DatabaseIndex)
            {
            /*case 0: // The old ugly and bad and simple KOTC Marker
                if (image.TrackingState == TrackingState.Tracking && KOTCImage == null)
                {
                    Debug.Log("Tracking OK");
                    KOTCImage = image;
                    KOTCAnchor = image.CreateAnchor(image.CenterPose);

                    world.SetParent(KOTCAnchor.transform, false);
                    Transform gardenObj = Instantiate(garden).transform;                    
                    gardenObj.SetParent(world.transform, false);

                    GetComponent<LevelInstatiator>().world = world;
                    GetComponent<LevelInstatiator>().buildLevel();

                    readyPlayerOne();
                    readyKing();
                }
                else if (image.TrackingState == TrackingState.Stopped)
                {
                    Debug.Log("Tracking Stopped");
                    world.SetParent(null, true);

                    KOTCImage = null;
                    GameObject.Destroy(KOTCAnchor);
                    KOTCAnchor = null;
                }
                break;*/
            case 0:
            case 1: // The new wonderful but which could be improved KOTC Marker (but in Black & White)
            case 2: // The amazing and beautiful (but which could still be improved) KOTC Marker
                if (image.TrackingState == TrackingState.Tracking && KOTCImage == null)
                {
                    Debug.Log("Tracking OK");
                    KOTCImage = image;
                    KOTCAnchor = image.CreateAnchor(image.CenterPose);

                    world.SetParent(KOTCAnchor.transform, false);
                    world.localPosition -= world.up * 0.5f;
                    Transform gardenObj = Instantiate(garden).transform;
                    gardenObj.SetParent(world.transform, false);

                    GetComponent<LevelInstatiator>().world = world;
                    GetComponent<LevelInstatiator>().buildLevel();

                    readyPlayerOne();
                    readyKing();
                }
                else if (image.TrackingState == TrackingState.Stopped)
                {
                    Debug.Log("Tracking Stopped");
                    world.SetParent(null, true);

                    KOTCImage = null;
                    GameObject.Destroy(KOTCAnchor);
                    KOTCAnchor = null;
                }
                break;
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

    public void ToggleWorldLock()
    {
        if(world.parent == null)
        {
            world.parent = KOTCAnchor.transform;
            world.localPosition = Vector3.zero;
            world.localRotation = Quaternion.identity;
            world.localScale = Vector3.one * 0.1f;
        } else
        {
            world.SetParent(null, true);
        }
    }

    GameObject playerInstance;
    
    void readyPlayerOne()
    {
        /*playerInstance = Instantiate(playerOne, world, false);
        CharacterCtrl c = playerInstance.GetComponent<CharacterCtrl>();
        c.world = world;
        c.winText = UIWinning;

        playerInstance.transform.localPosition = new Vector3(xBoundsMin + 0.1f, 0.1f, zBoundsMin);*/
    }

    public void ResetPlayer()
    {
        playerInstance.transform.localPosition = new Vector3(xBoundsMin + 0.1f, 0.1f, zBoundsMin);
        playerInstance.GetComponent<CharacterCtrl>().Reset();
    }

    void readyKing()
    {
        GameObject kingInstance = Instantiate(king, world, false);
        KingController_AR kCtrl = kingInstance.GetComponent<KingController_AR>();
        kCtrl.setPlayer(playerInstance);
        kingInstance.transform.localPosition = new Vector3(xBoundsMin - 0.5f, 3.1f, zBoundsMin);
    }
}
