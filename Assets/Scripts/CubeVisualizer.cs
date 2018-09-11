 using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;

public class CubeVisualizer : MonoBehaviour
{
    public AugmentedImage Image;
    public GameObject Cube;
    
    public void Update()
    {
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            Cube.SetActive(false);
            return;
        }

        float halfWidth = Image.ExtentX / 2;
        float halfHeight = Image.ExtentZ / 2;
        Cube.transform.localPosition = Vector3.zero;
        //Cube.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.back);
        /*
        FrameLowerRight.transform.localPosition = (halfWidth * Vector3.right) + (halfHeight * Vector3.back);
        FrameUpperLeft.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.forward);
        FrameUpperRight.transform.localPosition = (halfWidth * Vector3.right) + (halfHeight * Vector3.forward);
        */

        Cube.SetActive(true);
        /*
        FrameLowerRight.SetActive(true);
        FrameUpperLeft.SetActive(true);
        FrameUpperRight.SetActive(true);
        */
    }
}