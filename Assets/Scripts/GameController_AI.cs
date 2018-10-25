using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_AI : MonoBehaviour {

    public Transform world;
    public GameObject playerOne;
    public GameObject smokePrefab;
    public bool isMultiplayer;

    const float xBoundsMin = -15.5f;
    const float xBoundsMax = 15.5f;
    const float zBoundsMin = -15.5f;
    const float zBoundsMax = 15.5f;

    // Use this for initialization
    void Start ()
    {        
        GetComponent<LevelInstatiator>().buildLevel();

        /*
        GetComponent<LevelInstatiator>().PlantSmoke(smokePrefab, 1);
        GetComponent<LevelInstatiator>().PlantSmoke(smokePrefab, 2);
        GetComponent<LevelInstatiator>().PlantSmoke(smokePrefab, 3);
        GetComponent<LevelInstatiator>().PlantSmoke(smokePrefab, 4);
        */

        if (!isMultiplayer)
        {
            readyPlayerOne();
        }
    }

    /*********************************\
        Helper and utility functions
    \*********************************/    
   
    void readyPlayerOne()
    {
        playerOne.GetComponent<NetworkPlayer>().StopCoroutine("UpdateNetworked");
        playerOne.transform.position = new Vector3(16f, 2.5f, 16f);
        //GameObject player = Instantiate(playerOne, new Vector3(15.5f, 2.5f, 15.5f), Quaternion.identity, world);
        //playerOne.transform.position = new Vector3(15.5f, 2.5f, 15.5f);
        //playerOne.SetActive(true);
    }  
}
