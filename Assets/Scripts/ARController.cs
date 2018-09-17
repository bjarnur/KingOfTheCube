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
    public Transform corner1;
    public Transform corner2;
    public Transform corner3;
    public Transform corner4;
    public Transform platformX;
    public Transform platformZ;
    public Transform ladderX;
    public Transform ladderZ;
    public Transform unitCube;

    private List<AugmentedImage> m_AugmentedImages = new List<AugmentedImage>();
    private AugmentedImage KOTCImage = null;
    private Anchor KOTCAnchor = null;

    private const float xBoundsMin = -15.5f;
    private const float xBoundsMax = 15.5f;
    private const float zBoundsMin = -15.5f;
    private const float zBoundsMax = 15.5f;

    public void Start()
    {
    }

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

                    world.parent = KOTCAnchor.transform;
                    world.localPosition = Vector3.zero;
                    world.localRotation = Quaternion.identity;
                    world.gameObject.SetActive(true);

                    Transform gardenObj = Instantiate(garden).transform;
                    gardenObj.parent = world;
                    gardenObj.localPosition = Vector3.zero;
                    gardenObj.localRotation = Quaternion.identity;

                    //buildBasePlatform(scale);
                    readyPlayerOne();
                    /*for (int i = -15; i < 15; i += 4)
                    {
                        Transform plat = Instantiate(platformX, world.position, Quaternion.identity, world);
                        plat.position += new Vector3(i * scale.x, 0.5f * scale.y, 17.5f * scale.z);
                    }*/
                }
                else if (image.TrackingState == TrackingState.Stopped)
                {
                    Debug.Log("Tracking Stopped");
                    world.parent = null;
                    world.gameObject.SetActive(false);

                    KOTCImage = null;
                    GameObject.Destroy(KOTCAnchor);
                    KOTCAnchor = null;
                }
            }
        }

        UIScanning.SetActive(KOTCImage == null);
    }

    public void ToggleWorldLock()
    {
        if(world.parent == null)
        {
            world.parent = KOTCAnchor.transform;
        } else
        {
            world.parent = null;
        }
    }

    void readyPlayerOne()
    {
        GameObject player = Instantiate(playerOne, world.position, Quaternion.identity, world);
        CharacterCtrl c = player.GetComponent<CharacterCtrl>();
        c.SetWorld(world);

        player.transform.position += new Vector3(0f, 3.5f, 0f);
    }

    void buildBasePlatform(Vector3 scale)
    {
        float x = xBoundsMin;
        float y = 0.5f;
        float z = zBoundsMax;

        while (x < 15.5)
        {
            Transform plat = Instantiate(unitCube, world.position, Quaternion.identity, world);
            plat.position += new Vector3(x * scale.x, 0.5f * scale.y, zBoundsMax * scale.z);
            x += 1;
        }
        while (z > -15.5)
        {
            Transform plat = Instantiate(unitCube, world.position, Quaternion.identity, world);
            plat.position += new Vector3(xBoundsMax * scale.x, 0.5f * scale.y, z * scale.z);
            z -= 1;
        }
        while (x > -15.5)
        {
            Transform plat = Instantiate(unitCube, world.position, Quaternion.identity, world);
            plat.position += new Vector3(x * scale.x, 0.5f * scale.y, zBoundsMin * scale.z);
            x -= 1;
        }
        while (z < 15.5)
        {
            Transform plat = Instantiate(unitCube, world.position, Quaternion.identity, world);
            plat.position += new Vector3(xBoundsMin * scale.x, 0.5f * scale.y, z * scale.z);
            z += 1;
        }
    }
}
