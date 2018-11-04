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

    /********************\
     * Editor variables *
    \********************/ 

    public float scalingFactor = 0.1f;
    public Transform world;
    public Transform platformUnit;
    public Transform ladder;


    /*********************\
     * Private variables *
    \*********************/

    private float xBoundsMin = -15.7f;
    private float xBoundsMax = 15.7f;
    private float zBoundsMin = -15.7f;
    private float zBoundsMax = 15.7f;
    private float top = 30f;

    private Vector3 firstFaceVector;
    private Vector3 secondFaceVector;
    private Vector3 thirdFaceVector;
    private Vector3 fourthFaceVector;

    private Vector3 centerFirstFaceVector;
    private Vector3 centerSecondFaceVector;
    private Vector3 centerThirdFaceVector;
    private Vector3 centerFourthFaceVector;

    void Awake() {
                
        xBoundsMin *= scalingFactor;
        xBoundsMax *= scalingFactor;
        zBoundsMin *= scalingFactor;
        zBoundsMax *= scalingFactor;
        top *= scalingFactor;

        centerFirstFaceVector = new Vector3(0f, 0f, zBoundsMin);
        centerSecondFaceVector = new Vector3(xBoundsMax, 0f, 0f);
        centerThirdFaceVector = new Vector3(0f, 0f, zBoundsMax);
        centerFourthFaceVector = new Vector3(xBoundsMin, 0f, 0);

        firstFaceVector = new Vector3(xBoundsMax, 0f, zBoundsMin);
        secondFaceVector = new Vector3(xBoundsMax, 0f, zBoundsMax);
        thirdFaceVector = new Vector3(xBoundsMin, 0f, zBoundsMax);
        fourthFaceVector = new Vector3(xBoundsMax, 0f, zBoundsMin);
    }
    

    public void buildLevel() {
        /*
        //Can be used to easilly see what face you are looking at
        buildLadder(CubeFaces.firstFace, CubeLevel.first, 1, 1);
        buildLadder(CubeFaces.secondFace, CubeLevel.first, 1, 2);
        buildLadder(CubeFaces.thirdFace, CubeLevel.first, 1, 3);
        buildLadder(CubeFaces.fourthFace, CubeLevel.first, 1, 4);
        */
        //buildLadder(CubeFaces.secondFace, CubeLevel.first, 10, 25);        
        buildPlatform(CubeFaces.firstFace, CubeLevel.first, -1, 31);
        buildPlatform(CubeFaces.secondFace, CubeLevel.first, -1, 31);
        buildPlatform(CubeFaces.thirdFace, CubeLevel.first, -1, 31);
        buildPlatform(CubeFaces.fourthFace, CubeLevel.first, -1, 31);

        buildPlatform(CubeFaces.firstFace, CubeLevel.fourth, 20, 31);
        buildPlatform(CubeFaces.fourthFace, CubeLevel.fourth, -1, 10);
        //buildLadder(CubeFaces.fourthFace, CubeLevel.fourth, 3, 25);

        buildPlatform(CubeFaces.fourthFace, CubeLevel.fifth, 15, 31);
        buildPlatform(CubeFaces.thirdFace, CubeLevel.seventh, -1, 10);

        buildPlatform(CubeFaces.thirdFace, CubeLevel.seventh, 15, 20);
        buildPlatform(CubeFaces.thirdFace, CubeLevel.ninth, 20, 25);
        buildPlatform(CubeFaces.thirdFace, CubeLevel.eleventh, 25, 28);
        buildPlatform(CubeFaces.thirdFace, CubeLevel.thirteenth, 28, 31);
        buildPlatform(CubeFaces.secondFace, CubeLevel.thirteenth, -1, 9);

        buildPlatform(CubeFaces.secondFace, CubeLevel.eleventh, 15, 25);
        buildPlatform(CubeFaces.secondFace, CubeLevel.fourth, 15, 24);
        buildPlatform(CubeFaces.secondFace, CubeLevel.seventh, 18, 31);
        buildPlatform(CubeFaces.firstFace, CubeLevel.seventh, -1, 10);
        buildPlatform(CubeFaces.firstFace, CubeLevel.seventh, 15, 20);

        buildPlatform(CubeFaces.firstFace, CubeLevel.tenth, 20, 31);
        buildPlatform(CubeFaces.fourthFace, CubeLevel.tenth, -1, 10);
        buildPlatform(CubeFaces.fourthFace, CubeLevel.thirteenth, 15, 20);
        buildPlatform(CubeFaces.fourthFace, CubeLevel.thirteenth, 25, 31);
        buildPlatform(CubeFaces.fourthFace, CubeLevel.eleventh, 14, 20);
        buildPlatform(CubeFaces.thirdFace, CubeLevel.thirteenth, -1, 5);

        buildLadder(CubeFaces.firstFace, CubeLevel.first, 18, 3);
        buildLadder(CubeFaces.firstFace, CubeLevel.seventh, 18, 2);
        buildLadder(CubeFaces.secondFace, CubeLevel.eleventh, 20, 6);
        buildLadder(CubeFaces.thirdFace, CubeLevel.first, 13, 5);
        buildLadder(CubeFaces.thirdFace, CubeLevel.thirteenth, 4, 3);
        buildLadder(CubeFaces.fourthFace, CubeLevel.eleventh, 13, 1);
    }

    private void buildPlatform(CubeFaces inFace, float atLevel, int fromColumn, int toColumn)
    {
        for (int i = fromColumn; i <= toColumn; i++)
        {
            instantiateFacePlatform(inFace, atLevel, i, 0);
            instantiateFacePlatform(inFace, atLevel, i, 1);
        }
    }

    private void buildLadder(CubeFaces inFace, float atLevel, int column, float numOfLadders)
    {
        Transform t = Instantiate(ladder, world, false); 
        Quaternion rotation = Quaternion.identity;
        Vector3 copyVector = new Vector3(0, 0, 0);
        switch(inFace)
        { 
            case CubeFaces.firstFace:
                copyVector = firstFaceVector;
                copyVector.x = xBoundsMax - (column * scalingFactor);
                copyVector.z = zBoundsMin + (0.6f * scalingFactor);
                rotation = Quaternion.Euler(0, 90 + 180, 0);
                break;
            case CubeFaces.secondFace:
                copyVector = secondFaceVector;
                copyVector.z = zBoundsMax - (column * scalingFactor);
                copyVector.x = xBoundsMax - (0.6f * scalingFactor);
                rotation = Quaternion.Euler(0, 90 + 90, 0);
                break;
            case CubeFaces.thirdFace:
                copyVector = thirdFaceVector;
                copyVector.x = xBoundsMin + (column * scalingFactor);
                copyVector.z = zBoundsMax - (0.6f * scalingFactor);
                rotation = Quaternion.Euler(0, 90, 0);
                break;
            case CubeFaces.fourthFace:
                copyVector = fourthFaceVector;
                copyVector.z = zBoundsMin + (column * scalingFactor);                                
                copyVector.x = xBoundsMin + (0.6f * scalingFactor);
                rotation = Quaternion.Euler(0, 90 + 270, 0);
                break;
        }

        copyVector.y = atLevel * scalingFactor;
        t.localRotation = rotation;
        t.localPosition += copyVector;

        for (int i = 2; i <= numOfLadders; ++i) {
            var newLadder = Instantiate(ladder, world, false); 
            var newCopyVector = copyVector;
            newCopyVector.y += 2f  * scalingFactor * (i-1);
            newLadder.localPosition += newCopyVector;
            newLadder.localRotation = rotation;
        }
    }


    private void instantiateFacePlatform(CubeFaces inFace, float atLevel, float atColumn, float distanceFromCube)
    {
        var copyVector = new Vector3(0, 0, 0);
        switch (inFace)
        {
            case CubeFaces.firstFace:
                copyVector = firstFaceVector;
                copyVector.x = xBoundsMax - (atColumn  * scalingFactor);
                copyVector.z = zBoundsMin - (distanceFromCube * scalingFactor);
                break;
            case CubeFaces.secondFace:
                copyVector = secondFaceVector;
                copyVector.z = zBoundsMax - (atColumn * scalingFactor);
                copyVector.x = xBoundsMax + (distanceFromCube * scalingFactor);
                break;
            case CubeFaces.thirdFace:
                copyVector = thirdFaceVector;
                copyVector.x = xBoundsMin + (atColumn * scalingFactor);
                copyVector.z = zBoundsMax + (distanceFromCube * scalingFactor);
                break;
            case CubeFaces.fourthFace:
                copyVector = fourthFaceVector;
                copyVector.z = zBoundsMin + (atColumn * scalingFactor);
                copyVector.x = xBoundsMin - (distanceFromCube * scalingFactor);
                break;
        }
        copyVector.y = atLevel * scalingFactor;
        Transform t = Instantiate(platformUnit, world, false);
        t.localPosition += copyVector;
    }    

    public Vector3 instantiateSpawnPoint(int playerNumber)
    {
        switch(playerNumber)
        {
            case 0: //King
                return firstFaceVector + new Vector3(0, top, 0);
            case 1:
                return firstFaceVector + new Vector3(0, 1.5f * scalingFactor, 0);
            case 2:
                return secondFaceVector + new Vector3(0, 1.5f * scalingFactor, 0);
            case 3:
                return thirdFaceVector + new Vector3(0, 1.5f * scalingFactor, 0);
            case 4:
                return fourthFaceVector + new Vector3(0, 1.5f * scalingFactor, 0);
            default:
                Debug.LogError("Maximum four players allowed, you tried registering player " + playerNumber);
                return new Vector3();            
        }
    }

    public void PlantSmoke(GameObject smokePrefab, int faceNumber)
    {
        GameObject smoke = Instantiate(smokePrefab, world, false);        
        switch (faceNumber)
        {
            case 1:
                smoke.transform.localPosition = centerFirstFaceVector;
                break;
            case 2:
                smoke.transform.localPosition = centerSecondFaceVector;
                break;
            case 3:
                smoke.transform.localPosition = centerThirdFaceVector;
                break;
            case 4:
                smoke.transform.localPosition = centerFourthFaceVector;
                break;
            default:
                smoke.transform.localPosition = fourthFaceVector;
                break;
        }
    }
}
