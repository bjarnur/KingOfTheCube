using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

    string[] names = new string[]
    {
        "Bjarni",
        "Henrique",
        "Sonia",
        "Rafa",
        "Julien"
    };

    public Canvas LobbyCanvas;
   
    const string VERSION = "0.0.1";
    private string RoomName = "myRoom";
    private string PlayerName = "Player";
    private int LastKnownPlayerCount = 0;

    void Start ()
    {
        Debug.Log("STARTING UP");
        
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings(VERSION);

    }

    void Update ()
    {
        int NumberOfPlayers = PhotonNetwork.room.PlayerCount;
        if(NumberOfPlayers > LastKnownPlayerCount)
        {
            string PlayerNames = "";
            foreach (PhotonPlayer Player in PhotonNetwork.playerList)
            {
                PlayerNames += names[Convert.ToInt32(Player.NickName)] + Player.NickName;
                PlayerNames += "\n";
            }
            GameObject.Find("PlayerList").GetComponent<Text>().text = PlayerNames;
            GameObject.FindGameObjectWithTag("NumberOfPlayersText").GetComponent<Text>().text = NumberOfPlayers.ToString();
        }
                

        /*
        int NumberOfRooms = PhotonNetwork.countOfRooms;
        if(NumberOfRooms > 1)
        {            
            long IDthis = Convert.ToInt64(roomName);
            long IDmin = IDthis;

            var AllRooms = PhotonNetwork.GetRoomList();
            foreach(var Room in AllRooms)
            {                
                long IDother = Convert.ToInt64(Room.Name);
                if(IDother < IDmin)
                {
                    IDmin = IDother;
                }
            }

            if(IDmin < IDthis)
            {
                Debug.Log("Joiningn other room");
                PhotonNetwork.JoinRoom(IDmin.ToString());
            }
        } */
    }

    [PunRPC]
    public void LaunchGame()
    {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.LoadLevel("AssembleCube_AI_test");
        else
            Debug.Log("You have now power here");
    }

    void OnJoinedLobby()
    {
        Debug.Log("JOINED LOBBY");

        long Ticks = DateTime.Now.Ticks;
        RoomName = Ticks.ToString();

        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 5 };
        PhotonNetwork.JoinOrCreateRoom("FUN ROOM", roomOptions, TypedLobby.Default);
    }
	
	void OnJoinedRoom()
    {
        Debug.Log("JOINING ROOM");

        string RoomName = PhotonNetwork.room.Name;
        int NumberOfPlayers = PhotonNetwork.room.PlayerCount;
        int PlayerIndex = NumberOfPlayers - 1;

        PhotonNetwork.player.NickName = PlayerIndex.ToString();
        GameObject.FindGameObjectWithTag("RoomNameText").GetComponent<Text>().text = RoomName;
        GameObject.FindGameObjectWithTag("NumberOfPlayersText").GetComponent<Text>().text = NumberOfPlayers.ToString();
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
