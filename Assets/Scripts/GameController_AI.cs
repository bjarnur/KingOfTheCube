using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_AI : MonoBehaviour {

    public Transform world;
    public GameObject playerOne;

    const float xBoundsMin = -15.5f;
    const float xBoundsMax = 15.5f;
    const float zBoundsMin = -15.5f;
    const float zBoundsMax = 15.5f;

    // Use this for initialization
    void Start () {
        
        //Instantiate player
        readyPlayerOne();
        GetComponent<LevelInstatiator>().buildLevel();
    }

    /*********************************\
        Helper and utility functions
    \*********************************/    
   
    void readyPlayerOne()
    {
        //GameObject player = Instantiate(playerOne, new Vector3(15.5f, 2.5f, 15.5f), Quaternion.identity, world);
        playerOne.transform.position = new Vector3(15.5f, 2.5f, 15.5f);
        playerOne.SetActive(true);
    }  
}
