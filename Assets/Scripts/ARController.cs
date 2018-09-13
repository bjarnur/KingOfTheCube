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

    public Transform WorldContainer;
    public BoxCollider VirtualCubeCollider;
    public GameObject UIScanning;

    private List<AugmentedImage> m_AugmentedImages = new List<AugmentedImage>();
    private AugmentedImage KOTCImage = null;
    private Anchor KOTCAnchor = null;

    public void Start()
    {
        Vector3 colliderSize = VirtualCubeCollider.size;
        Vector3 scale = new Vector3(RealCubeSize / colliderSize.x, RealCubeSize / colliderSize.y, RealCubeSize / colliderSize.z) * 2f;
        WorldContainer.localScale = scale;
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

                    WorldContainer.parent = KOTCAnchor.transform;
                    WorldContainer.localPosition = Vector3.zero;
                    WorldContainer.localRotation = Quaternion.identity;
                    WorldContainer.gameObject.SetActive(true);
                } else if (image.TrackingState == TrackingState.Stopped)
                {
                    WorldContainer.parent = null;
                    WorldContainer.gameObject.SetActive(false);

                    KOTCImage = null;
                    GameObject.Destroy(KOTCAnchor);
                    KOTCAnchor = null;
                }
            }
        }

        UIScanning.SetActive(KOTCImage == null);
    }
}
