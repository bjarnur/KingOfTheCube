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

    public bool IsAr = false;
    public Canvas LobbyCanvas;
    public GameObject JoinGamePanel;
    public GameObject LaunchGamePanel;
    public GameObject CountdownTimer;
    public GameObject ReadyMessage;

    const string VERSION = "0.0.1";
    private string RoomName = "PrivateRoom";
    private string PlayerName = "Player";
    private bool HasJoinedRoom = false;
    private bool CountdownTimerActive = false;
    private byte MaxPlayerNumber = 5;
    private int LastKnownPlayerCount = -1;
    private float CountdowntimerValue = 10;
    private long PlayerID;

    void Start ()
    {
        Debug.Log("STARTING UP");       

        ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
        PropertyTable.Add("Ready", false);
        PropertyTable.Add("Stamp", null);

        PhotonNetwork.player.SetCustomProperties(PropertyTable);
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings(VERSION);

    }

    void Update ()
    {
        if (!HasJoinedRoom) return;

        AssignPlayerIndices();

        if (!CountdownTimerActive)
        { 
            bool AllPlayersReady = true;
            foreach (PhotonPlayer Player in PhotonNetwork.playerList)
            {
                object check = Player.CustomProperties["Ready"];
                if (check == null) return;

                bool PlayerReady = (bool) Player.CustomProperties["Ready"];
                AllPlayersReady = AllPlayersReady && PlayerReady;
            }
            if(AllPlayersReady)
            {
                CountdowntimerValue = 10;
                CountdownTimerActive = true;
                CountdownTimer.GetComponent<Text>().text = "Starting in " + ((int)Math.Round(CountdowntimerValue)).ToString();
            }
        }
        else
        {
            CountdowntimerValue -= Time.deltaTime;
            CountdownTimer.GetComponent<Text>().text = "Starting in " + ((int)Math.Round(CountdowntimerValue)).ToString();
            if (CountdowntimerValue < 0.0f)
                LaunchGame();
        }
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = MaxPlayerNumber };
        bool success = PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
        if(!success)
        {
            Debug.Log("Failed to join room");
            return;
        }

        Debug.Log("Player joining game");
        JoinGamePanel.SetActive(false);
        LaunchGamePanel.SetActive(true);
    }

    public void PlayerReady()
    {
        if (LastKnownPlayerCount < 2) return;

        ReadyMessage.GetComponent<Text>().text = "Ready, waiting for other players";
        ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
        PropertyTable.Add("Ready", true);
        PropertyTable.Add("Stamp", PlayerID);
        PhotonNetwork.player.SetCustomProperties(PropertyTable);
    }

    void LaunchGame()
    {
        if(IsAr)
            PhotonNetwork.LoadLevel("AR_OnlineScene");
        else
            PhotonNetwork.LoadLevel("AssembleCube_AI_test");
    }

    Dictionary<long, PhotonPlayer> GetPlayerTimestampMap()
    {
        Dictionary<long, PhotonPlayer> res = new Dictionary<long, PhotonPlayer>();
        foreach (PhotonPlayer Player in PhotonNetwork.playerList)
        {
            //Will be null if other player hasn't had time to start up
            object check = Player.CustomProperties["Stamp"];
            if (check == null) return null;

            long Stamp = (long)Player.CustomProperties["Stamp"];
            res.Add(Stamp, Player);
        }
        return res;
    }

    void AssignPlayerIndices()
    {
        int NumberOfPlayers = PhotonNetwork.room.PlayerCount;

        if (NumberOfPlayers > LastKnownPlayerCount)
        {
            Dictionary<long, PhotonPlayer> PlayerTimestampMap = GetPlayerTimestampMap();
            if (PlayerTimestampMap == null) return;

            List<long> MapKeys = new List<long>(PlayerTimestampMap.Keys);
            MapKeys.Sort();

            int PlayerIndex = 0;
            string PlayerNames = "";
            foreach (long Key in MapKeys)
            {
                PhotonPlayer Player = PlayerTimestampMap[Key];
                if (Player.ID == PhotonNetwork.player.ID)
                {
                    //Static ID used to instantiate character in game scene
                    GameConstants.NetworkedPlayerID = PlayerIndex;
                }

                PlayerNames += names[Convert.ToInt32(PlayerIndex)] + PlayerIndex;
                PlayerNames += "\n";
                PlayerIndex++;
            }

            GameObject.Find("PlayerList").GetComponent<Text>().text = PlayerNames;
            GameObject.FindGameObjectWithTag("NumberOfPlayersText").GetComponent<Text>().text = NumberOfPlayers.ToString();
            LastKnownPlayerCount = NumberOfPlayers;
        }
    }

    void OnJoinedLobby()
    {
        Debug.Log("JOINED LOBBY");
    }
    
	void OnJoinedRoom()
    {
        Debug.Log("JOINING ROOM");

        HasJoinedRoom = true;
        PlayerID = DateTime.Now.Ticks;
        string RoomName = PhotonNetwork.room.Name;
        int NumberOfPlayers = PhotonNetwork.room.PlayerCount;
        
        ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
        PropertyTable.Add("Ready", false);
        PropertyTable.Add("Stamp", PlayerID);   
        PhotonNetwork.player.SetCustomProperties(PropertyTable);

        GameObject.FindGameObjectWithTag("RoomNameText").GetComponent<Text>().text = RoomName;
        GameObject.FindGameObjectWithTag("NumberOfPlayersText").GetComponent<Text>().text = NumberOfPlayers.ToString();
    }   
}
