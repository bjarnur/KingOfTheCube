using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmniscientController : MonoBehaviour  {

    private static OmniscientController instance = null;


    public static OmniscientController GetInstance()
    {
        if (instance == null)
        {
            instance = new OmniscientController();
        }

        return instance;
    }


    public Transform worldContainer; 


    public Transform getWorldContainer() {
        if (worldContainer != null) {
            return worldContainer;
        }
        else {
            Debug.Log("ERROR NO HAY TRANSFORM");
            return worldContainer;
        }
    } 
}
