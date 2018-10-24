using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {
    public static float cloudsSpeed = 1f;

    float bounds;

    public void Init(float bounds)
    {
        this.bounds = bounds;
        transform.localPosition = new Vector3(-bounds, Random.Range(-bounds, bounds) / 2f + 2f, Random.Range(-bounds, bounds));
        transform.localScale = new Vector3(Random.Range(0.5f, 2f), 0.2f, Random.Range(0.5f, 2f));
    }

    void Update()
    {
        transform.localPosition += Vector3.right * cloudsSpeed * Time.deltaTime;
        if (transform.localPosition.x > bounds)
        {
            Destroy(gameObject);
        }
    }
}
