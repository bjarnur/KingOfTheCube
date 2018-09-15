using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CubeFaces {
    firstFace = 1,
    secondFace = 2,
    thirdFace = 3,
    fourthFace = 4
}

public class LevelInstatiator : MonoBehaviour{

    const float xBoundsMin = -15.5f; // Extra 0.5 for the corners
    const float xBoundsMax = 15.5f;
    const float zBoundsMin = -15.5f;
    const float zBoundsMax = 15.5f;

    Vector3 firstFaceVector = new Vector3(15.5f, 0f, -15.5f);
    Vector3 secondFaceVector = new Vector3(15.5f, 0f, 15.5f);
    Vector3 thirdFaceVector = new Vector3(-15.5f, 0f, 15.5f);
    Vector3 fourthFaceVector = new Vector3(-15.5f, 0f, -15.5f);


    public Transform world;
    public Transform firstFaceCorner;
    public Transform secondFaceCorner;
    public Transform thirdFaceCorner;
    public Transform fourthFaceCorner;

    // Use this for initialization
    void Start() {
        instantiateFaceCorner(inFace: CubeFaces.firstFace, atHeight: 5f);
        instantiateFaceCorner(inFace: CubeFaces.secondFace, atHeight: 5f);
        instantiateFaceCorner(inFace: CubeFaces.thirdFace, atHeight: 5f);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atHeight: 5f);
    }

    // Update is called once per frame
    void Update() {

    }

    /* This method instantiates a corner in the indicated face and height. The face's corner is always the rightmost one*/
    private void instantiateFaceCorner(CubeFaces inFace, float atHeight) { // Faces can be 1,2,3 or 4
        var copyVector = new Vector3(0,0,0);
        var copyCorner = firstFaceCorner;
        switch (inFace) {
            case CubeFaces.firstFace:
                copyVector = firstFaceVector;
                break;
            case CubeFaces.secondFace:
                copyVector = secondFaceVector;
                copyCorner = secondFaceCorner;
                break;
            case CubeFaces.thirdFace:
                copyVector = thirdFaceVector;
                copyCorner = thirdFaceCorner;
                break;
            case CubeFaces.fourthFace:
                copyVector = fourthFaceVector;
                copyCorner = fourthFaceCorner;
                break;
        } 
        copyVector.y = atHeight;
        Instantiate(copyCorner, copyVector, Quaternion.identity, world);
    }

}
