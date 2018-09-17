using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_AI : MonoBehaviour {

    public Transform world;
    public Transform platform;
    public Transform corner1;
    public Transform corner2;
    public Transform corner3;
    public Transform corner4;
    public Transform platformX;
    public Transform platformZ;
    public Transform ladderX;
    public Transform ladderZ;
    public GameObject playerOne;

    const float xBoundsMin = -15.5f;
    const float xBoundsMax = 15.5f;
    const float zBoundsMin = -15.5f;
    const float zBoundsMax = 15.5f;

    // Use this for initialization
    void Start () {

        //Place objects in level
        constructLevel();

        //Instantiate player
        readyPlayerOne();
    }
	
	// Update is called once per frame
	void Update () {
	    	
	}

    /*********************************\
        Helper and utility functions
    \*********************************/

    void constructLevel()
    {
        //Builds a a solid platform around the base of the cube
        buildBasePlatform();

        Instantiate(corner1, new Vector3(15.5f, 5.5f, -15.5f), Quaternion.identity, world);
        Instantiate(corner2, new Vector3(-15.5f, 5.5f, 15.5f), Quaternion.identity, world);
        Instantiate(corner3, new Vector3(-15.5f, 15.5f, -15.5f), Quaternion.identity, world);
        Instantiate(corner4, new Vector3(15.5f, 15.5f, 15.5f), Quaternion.identity, world);

        Instantiate(corner4, new Vector3(15.5f, 8f, 15.5f), Quaternion.identity, world);
        Instantiate(platformX, new Vector3(11.5f, 8f, 15.5f), Quaternion.identity, world);
        Instantiate(ladderX, new Vector3(10.5f, 4f, 15.5f), Quaternion.identity, world);
    }

   
    void readyPlayerOne()
    {
        //GameObject player = Instantiate(playerOne, new Vector3(15.5f, 2.5f, 15.5f), Quaternion.identity, world);
        playerOne.transform.position = new Vector3(15.5f, 2.5f, 15.5f);
        playerOne.SetActive(true);
    }


    void buildBasePlatform() {
        float x = xBoundsMin;
        float y = 0.5f;
        float z = zBoundsMax;

        while (x < 15.5)
        {
            Transform obj = Instantiate(platform, new Vector3(x, y, z), Quaternion.identity, world);
            x += 1;
        }
        while (z > -15.5)
        {
            Transform obj = Instantiate(platform, new Vector3(x, y, z), Quaternion.identity, world);
            z -= 1;
        }
        while (x > -15.5)
        {
            Transform obj = Instantiate(platform, new Vector3(x, y, z), Quaternion.identity, world);
            x -= 1;
        }
        while (z < 15.5)
        {
            Transform obj = Instantiate(platform, new Vector3(x, y, z), Quaternion.identity, world);
            z += 1;
        }
    }
}
