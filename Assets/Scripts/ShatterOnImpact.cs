using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnImpact : MonoBehaviour {

    public GameObject shattered;

    public float timer = 3f;


    private void OnMouseDown( )
    {

        GameObject.Instantiate(shattered, transform.position, transform.rotation);

        Destroy(this.gameObject);
    }
}
