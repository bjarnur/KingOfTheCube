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

    const float scalingFactor = 0.1f;

    
    const float xBoundsMin = -15.5f * scalingFactor; // Extra 0.84 for the corners
    const float xBoundsMax = 15.5f * scalingFactor;
    const float zBoundsMin = -15.5f * scalingFactor;
    const float zBoundsMax = 15.5f * scalingFactor;
    
    /*
    const float xBoundsMin = -1.55f * scalingFactor;
    const float xBoundsMax = 1.55f * scalingFactor;
    const float zBoundsMin = -1.55f * scalingFactor;
    const float zBoundsMax = 1.55f * scalingFactor;
    */
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
        //buildLevel();
    }

    // Update is called once per frame
    void Update() {

    }

    public void buildLevel() {

        instantiateFaceCorner(inFace: CubeFaces.firstFace,  atLevel: CubeLevel.first * scalingFactor);
        instantiateFaceCorner(inFace: CubeFaces.secondFace, atLevel: CubeLevel.fifth * scalingFactor);
        instantiateFaceCorner(inFace: CubeFaces.thirdFace,  atLevel: CubeLevel.tenth * scalingFactor);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.tenth * scalingFactor);


        instantiateFaceCorner(inFace: CubeFaces.firstFace, atLevel: CubeLevel.third * scalingFactor);
        instantiateFaceCorner(inFace: CubeFaces.secondFace, atLevel: CubeLevel.third * scalingFactor);
        for (int i = 1; i <= 30; i++)
        {
            instantiateFacePlatform(inFace: CubeFaces.secondFace, 
                                    atLevel: CubeLevel.third * scalingFactor, 
                                    atColumn: i);
        }

        instantiateFaceCorner(inFace: CubeFaces.thirdFace, atLevel: CubeLevel.sixth * scalingFactor);
        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.sixth * scalingFactor);
        for (int i = 1; i <= 30; i++)
        {
            instantiateFacePlatform(inFace: CubeFaces.fourthFace,
                                    atLevel: CubeLevel.sixth * scalingFactor,
                                    atColumn: i);
        }

        instantiateFaceCorner(inFace: CubeFaces.fourthFace, atLevel: CubeLevel.ninth * scalingFactor);
        instantiateFaceCorner(inFace: CubeFaces.firstFace, atLevel: CubeLevel.ninth * scalingFactor);
        for (int i = 1; i <= 30; i++)
        {
            instantiateFacePlatform(inFace: CubeFaces.firstFace,
                                    atLevel: CubeLevel.ninth * scalingFactor,
                                    atColumn: i);
        }

        /*
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.sixth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.seventh);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.sixteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.first * scalingFactor, atColumn: CubeColumn.seventeenth);

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

        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.first);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.sixth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.seventh);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.sixteenth);
        instantiateFacePlatform(inFace: CubeFaces.secondFace, atLevel: CubeLevel.sixth, atColumn: CubeColumn.seventeenth);

        instantiateFaceCorner(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth);

        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.first);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.third);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.second);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.fourth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.fifth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.sixth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.seventh);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.tenth, atColumn: CubeColumn.eigth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.ninth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.tenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.eleventh);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.twelfth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.thirteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.fourtheenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.fifteenth);
        instantiateFacePlatform(inFace: CubeFaces.firstFace, atLevel: CubeLevel.fourtheenth, atColumn: CubeColumn.sixteenth);
        */
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
        Transform t = Instantiate(copyCorner, world, false);
        t.localPosition += copyVector;
    }
    
    private void instantiateFacePlatform(CubeFaces inFace, float atLevel, float atColumn)
    {
        var copyVector = new Vector3(0, 0, 0);
        switch (inFace)
        {
            case CubeFaces.firstFace:
                copyVector = firstFaceVector;
                copyVector.x = xBoundsMax - (atColumn  * scalingFactor);
                copyVector.z = zBoundsMin;
                break;
            case CubeFaces.secondFace:
                copyVector = secondFaceVector;
                copyVector.z = zBoundsMax - (atColumn * scalingFactor);
                copyVector.x = xBoundsMax;
                break;
            case CubeFaces.thirdFace:
                copyVector = thirdFaceVector;
                copyVector.x = xBoundsMin + (atColumn * scalingFactor);
                copyVector.z = zBoundsMax;
                break;
            case CubeFaces.fourthFace:
                copyVector = fourthFaceVector;
                copyVector.z = zBoundsMin + (atColumn * scalingFactor);
                copyVector.x = xBoundsMin;
                break;
        }
        copyVector.y = atLevel;
        Transform t = Instantiate(platformUnit, world, false);
        t.localPosition += copyVector;
    }
}
