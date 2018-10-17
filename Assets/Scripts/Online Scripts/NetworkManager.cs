using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {
        
    public bool isAR;

    const string VERSION = "0.0.1";
    private string roomName = "myRoom";

    void Start () {
        Debug.Log("STARTING UP");
        PhotonNetwork.ConnectUsingSettings(VERSION);
        PhotonNetwork.autoJoinLobby = true;
	}

    void OnJoinedLobby()
    {
        Debug.Log("JOINED LOBBY");
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 5 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
	
	void OnJoinedRoom()
    {
        Debug.Log("JOINING ROOM");
        if(!isAR)
        {
            int numberOfPlayers = PhotonNetwork.countOfPlayers;
            if (numberOfPlayers == 1)
                spawnKing();
            else
                spawnPretender(numberOfPlayers);
        }
    }

    void spawnKing()
    {
        Vector3 spawn = GameObject.FindWithTag("Cube")
                            .GetComponent<LevelInstatiator>()
                            .instantiateSpawnPoint(0);

        GameObject newPlayer = PhotonNetwork.Instantiate("UnityKing", Vector3.zero, Quaternion.identity, 0);
        newPlayer.transform.SetParent(GameObject.Find("Wrapper").transform, false);
        newPlayer.transform.localPosition = spawn;

        KingController_AssemCube controller = newPlayer.GetComponent<KingController_AssemCube>();
        KingNetwork networkPlayer = newPlayer.GetComponent<KingNetwork>();
        Rigidbody playerRigidbody = newPlayer.GetComponent<Rigidbody>();

        controller.enabled = true;
        //controller.isMultiplayer = true;
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
