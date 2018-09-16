//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class ARController : MonoBehaviour
{
    // 20 centimeters wide
    public static float RealCubeSize = 0.2f;
        
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

    private BoxCollider virtualCubeCollider;
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
                    KOTCImage = image;
                    KOTCAnchor = image.CreateAnchor(image.CenterPose);

                    world.parent = KOTCAnchor.transform;
                    world.localPosition = Vector3.zero;
                    world.localRotation = Quaternion.identity;
                    world.gameObject.SetActive(true);

                    GameObject gardenObj = Instantiate(garden, world.position, Quaternion.identity, world);
                    virtualCubeCollider = garden.GetComponent<BoxCollider>();
                    Vector3 colliderSize = virtualCubeCollider.size;
                    Vector3 scale = new Vector3(RealCubeSize / colliderSize.x, RealCubeSize / colliderSize.y, RealCubeSize / colliderSize.z) * 2f;
                    world.localScale = scale;
                    buildBasePlatform(scale);
                    readyPlayerOne(scale);

                    /*
                    for (int i = -15; i < 15; i += 4)
                    {
                        Transform plat = Instantiate(platformX, world.position, Quaternion.identity, world);
                        plat.position += new Vector3(i * scale.x, 0.5f * scale.y, 17.5f * scale.z);
                    }
                    */
                }

                else if (image.TrackingState == TrackingState.Stopped)
                {
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

    void readyPlayerOne(Vector3 scale)
    {
        GameObject player = Instantiate(playerOne, world.position, Quaternion.identity, world);
        CharacterCtrl c = player.GetComponent<CharacterCtrl>();
        c.SetScale(scale);
        //c.SetWorld(world.position);
        player.transform.position += new Vector3(15.5f * scale.x, 2.5f * scale.y, 15.5f * scale.z);
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
