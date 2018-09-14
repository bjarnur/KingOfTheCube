using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform world;
    public Transform platform;
    public GameObject playerOne; 
    //public CharacterCtrl;

    // Use this for initialization
    void Start () {

        //TODO Generate our world

        float x = -15.5f;
        float y = 0.5f;
        float z = 15.5f;

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
        while(x > -15.5)
        {
            Transform obj = Instantiate(platform, new Vector3(x, y, z), Quaternion.identity, world);
            x -= 1;
        }
        while(z < 15.5)
        {
            Transform obj = Instantiate(platform, new Vector3(x, y, z), Quaternion.identity, world);
            z += 1;
        }

        GameObject player = Instantiate(playerOne, new Vector3(15.5f, (y + 2f), 15.5f), Quaternion.identity, world);
        player.AddComponent<CharacterCtrl>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
