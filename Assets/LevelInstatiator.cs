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

public static class CubeColumn {
    public const float first = 1;
    public const float second = 2;
    public const float third = 3;
    public const float fourth = 4;
    public const float fifth = 5;
    public const float sixth = 6;
    public const float seventh = 7;
    public const float eigth = 8;
    public const float ninth = 9;
    public const float tenth = 10;
    public const float eleventh = 11;
    public const float twelfth = 12;
    public const float thirteenth = 13;
    public const float fourtheenth = 14;
    public const float fifteenth = 15;
    public const float sixteenth = 16;
    public const float seventeenth = 17;
    public const float eighteenth = 18;
}

public class LevelInstatiator : MonoBehaviour{

    const float xBoundsMin = -15.64f; // Extra 0.84 for the corners
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
    public Transform platformUnit; 

    // Use this for initialization
    void Start() {
        buildLevel();
    }

    // Update is called once per frame
    void Update() {

    }

    private void buildLevel() {
        instantiateFaceCorner(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first);
        instantiateFaceCorner(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first);
        instantiateFaceCorner(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first);

        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventh);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventeenth);

        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventh);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixteenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventeenth);

        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventh);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixteenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventeenth);

        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventh);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.sixteenth);
        instantiateFacePlatform(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.first, atColumn: CubeColumn.seventeenth);


        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.first);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.sixth);
        //instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.seventh);
        //instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.sixteenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.seventeenth);
        instantiateFacePlatform(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.eighteenth);

        instantiateFaceCorner(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth);
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

    private void instantiateFacePlatform(CubeFaces inFace, float atLevel, float atColumn)
    {
        var copyVector = new Vector3(0, 0, 0);
        switch (inFace)
        {
            case CubeFaces.firstFace:
                copyVector = firstFaceVector;
                copyVector.x = xBoundsMax - 1.67f * atColumn;
                break;
            case CubeFaces.secondFace:
                copyVector = secondFaceVector;
                copyVector.z = zBoundsMax - 1.67f * atColumn;
                break;
            case CubeFaces.thirdFace:
                copyVector = thirdFaceVector;
                copyVector.x = xBoundsMin + 1.67f * atColumn;
                break;
            case CubeFaces.fourthFace:
                copyVector = fourthFaceVector;
                copyVector.z = zBoundsMin + 1.67f * atColumn;
                break;
        }
        copyVector.y = atLevel;

        Instantiate(platformUnit, copyVector, Quaternion.identity, world);
    }

}
