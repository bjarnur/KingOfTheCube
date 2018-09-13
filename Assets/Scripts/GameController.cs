using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform world;
    public Transform platform;    

    // Use this for initialization
    void Start () {

        //TODO Generate our world

        //Experiment in progress below, remove if not wanted
        float x = 10f;
        float y = 22f;
        float z = -5f;

        while (x > -20f)
        {
            Transform obj = Instantiate(platform, new Vector3(x, y, z), new Quaternion(1, 0, 0, 1), world);
            x -= 4f;          
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
