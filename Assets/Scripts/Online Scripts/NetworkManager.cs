using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

    private int playerNumber = 1;
    const string VERSION = "0.0.1";
    public string roomName = "myRoom";
    public GameObject player;
    public GameObject garden;
    public Transform spawn;
    //public string playerPrefabName = "character";

	// Use this for initialization
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

        //Debug.Log("player count " + PhotonNetwork.playerList.Length);
        //Transform loc = garden.transform.GetChild(PhotonNetwork.playerList.Length);
        PhotonNetwork.Instantiate("Player", spawn.position, Quaternion.identity, 0);        
    }
}
