using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CubeFaces {
    firstFace = 1,
    secondFace = 2,
    thirdFace = 3,
    fourthFace = 4
}
public static class CubeLevel {
    public const float first = 0.5f;
    public const float second = 2.5f;
    public const float third = 4.5f;
    public const float fourth = 6.5f;
    public const float fifth = 8.5f;
    public const float sixth = 10.5f;
    public const float seventh = 12.5f;
    public const float eigth = 14.5f;
    public const float ninth = 16.5f;
    public const float tenth = 18.5f;
    public const float eleventh = 20.5f;
    public const float twelfth = 22.5f;
    public const float thirteenth = 24.5f;
    public const float fourtheenth = 26.5f;
    public const float fifteenth = 28.5f;
}

public class LevelInstatiator : MonoBehaviour{

    const float xBoundsMin = -15.64f; // Extra 0.5 for the corners
    const float xBoundsMax = 16.04f;
    const float zBoundsMin = -15.84f;
    const float zBoundsMax = 15.84f;

    static Vector3 firstFaceVector = new Vector3(xBoundsMax, 0f, zBoundsMin);
    static Vector3 secondFaceVector = new Vector3(xBoundsMax, 0f, zBoundsMax);
    static Vector3 thirdFaceVector = new Vector3(xBoundsMin, 0f, zBoundsMax);
    static Vector3 fourthFaceVector = new Vector3(xBoundsMin, 0f, zBoundsMin);

    /* Vars from Editor */
    public Transform world;
    public Transform firstFaceCorner;
    public Transform secondFaceCorner;
    public Transform thirdFaceCorner;
    public Transform fourthFaceCorner;

    // Use this for initialization
    void Start() {
        instantiateFaceCorner(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first);
        instantiateFaceCorner(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first);
        instantiateFaceCorner(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first);



        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.fourth);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.seventh);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.tenth);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.thirteenth);
    }

    // Update is called once per frame
    void Update() {

    }

    /* This method instantiates a corner in the indicated face and height. The face's corner is always the rightmost one*/
    private void instantiateFaceCorner(CubeFaces inFace, float atLevel) { 
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
        copyVector.y = atLevel;
        Instantiate(copyCorner, copyVector, Quaternion.identity, world);
    }

}
