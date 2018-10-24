using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {
    public GameObject cloudPrefab;

    float bounds = 10f;
    List<Transform> clouds;

    // Use this for initialization
    void Start()
    {
        clouds = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn Logic
        if (Random.value < 0.03f)
        {
            SpawnCloud();
        }
    }

    void SpawnCloud()
    {
        GameObject cloud = Instantiate<GameObject>(cloudPrefab, transform);
        clouds.Add(cloud.transform);
        cloud.GetComponent<CloudController>().Init(bounds);
    }
}
