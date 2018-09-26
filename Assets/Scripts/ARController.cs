//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class ARController : MonoBehaviour
{
    public Transform ARCoreDevice;
    public GameObject UIScanning;
    public GameObject garden;
    public GameObject playerOne;
    public Transform world;
    public Transform unitCube;

    private List<AugmentedImage> m_AugmentedImages = new List<AugmentedImage>();
    private AugmentedImage KOTCImage = null;
    private Anchor KOTCAnchor = null;

    private const float xBoundsMin = -1.55f;
    private const float xBoundsMax = 1.55f;
    private const float zBoundsMin = -1.55f;
    private const float zBoundsMax = 1.55f;
    

    public void Update()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
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
            // The KOTC Marker
            if(image.DatabaseIndex == 0)
            {
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
                }
                else if (image.TrackingState == TrackingState.Stopped)
                {
                    Debug.Log("Tracking Stopped");
                    world.SetParent(null, true);

                    KOTCImage = null;
                    GameObject.Destroy(KOTCAnchor);
                    KOTCAnchor = null;
                }
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

        playerInstance.transform.localPosition = new Vector3(xBoundsMin + 0.1f, 0.1f, zBoundsMin);*/
    }

    public void ResetPlayer()
    {
        playerInstance.transform.localPosition = new Vector3(xBoundsMin + 0.1f, 0.1f, zBoundsMin);
        playerInstance.GetComponent<CharacterCtrl>().Reset();
    }
}
