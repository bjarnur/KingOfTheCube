using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;

public class NetworkManager : MonoBehaviour {
        
    public bool isAR;

    const string VERSION = "0.0.1";
    private string roomName = "myRoom";
    
    void Start()
    {
        if (!isAR)
        {
            int PlayerIndex = Convert.ToInt32(PhotonNetwork.player.NickName);
            if(PlayerIndex == 0)
                spawnKing();
            else
                spawnPretender(PlayerIndex);
        }
    }

    void spawnKing()
    {
        object[] InstanceData = new object[1];
        InstanceData[0] = "VR";

        Vector3 spawn = GameObject.FindWithTag("Cube")
                            .GetComponent<LevelInstatiator>()
                            .instantiateSpawnPoint(0);

        GameObject newPlayer = PhotonNetwork.Instantiate("UnityKing", Vector3.zero, Quaternion.identity, 0, InstanceData);
        newPlayer.transform.SetParent(GameObject.Find("Wrapper").transform, false);
        newPlayer.transform.localPosition = spawn;

        KingController_AssemCube controller = newPlayer.GetComponent<KingController_AssemCube>();
        KingNetwork networkPlayer = newPlayer.GetComponent<KingNetwork>();
        Rigidbody playerRigidbody = newPlayer.GetComponent<Rigidbody>();

        controller.enabled = true;
        controller.isMultiplayer = true;
        controller.isAI = false;
        networkPlayer.enabled = true;
        playerRigidbody.useGravity = true;
    }

    void spawnPretender(int playerNumber)
    {
        Vector3 spawn = GameObject.FindWithTag("Cube")
                            .GetComponent<LevelInstatiator>()
                            .instantiateSpawnPoint(playerNumber);

        GameObject newPlayer = PhotonNetwork.Instantiate("UnityPlayer", Vector3.zero, Quaternion.identity, 0);
        newPlayer.transform.SetParent(GameObject.Find("Wrapper").transform, false);
        newPlayer.transform.localPosition = spawn;

        PlayerController_AssemCube controller = newPlayer.GetComponent<PlayerController_AssemCube>();
        NetworkPlayer networkPlayer = newPlayer.GetComponent<NetworkPlayer>();
        Rigidbody playerRigidbody = newPlayer.GetComponent<Rigidbody>();

        controller.enabled = true;
        controller.isMultiplayer = true;
        networkPlayer.enabled = true;
        playerRigidbody.useGravity = true;
    }

}
