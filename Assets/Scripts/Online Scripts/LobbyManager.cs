using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class LobbyManager : MonoBehaviour
{

    string[] names = new string[]
    {
        "King of the Cube",
        "King Arthur",
        "King Kong",
        "Kingsley shacklebolt",
        "King's Cross"
    };

    public bool IsAr = false;
    public Canvas LobbyCanvas;
    public GameObject JoinGamePanel;
    public GameObject LaunchGamePanel;
    public GameObject CountdownTimer;
    public GameObject ReadyMessage;
    public Button ReadyButton;

    [HideInInspector] public bool GameOver;

    const string VERSION = "0.0.1";
    private string RoomName = "PrivateRoom";
    private string PlayerName = "Player";
    private bool HasJoinedRoom = false;
    private bool CountdownTimerActive = false;
    private bool ReadyToStartGame = false;
    private float SecondsInactive = 0.0f;
    private float InactifityTimeThreshold = 25.0f;
    private byte MaxPlayerNumber = 5;
    private int LastKnownPlayerCount = -1;
    private float CountdowntimerValue = 5;
    private float CountdowntimerLength = 5;
    private long PlayerID;
   
  

    void Start ()
    {
        Debug.Log("STARTING UP");       

        ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
        PropertyTable.Add(GameConstants.NetworkedProperties.Ready, false);
        PropertyTable.Add(GameConstants.NetworkedProperties.Stamp, null);
        PropertyTable.Add(GameConstants.NetworkedProperties.Inactive, false);
        PropertyTable.Add(GameConstants.NetworkedProperties.InGame, false);

        PhotonNetwork.player.SetCustomProperties(PropertyTable);
        PhotonNetwork.autoJoinLobby = true;
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    

    void Update ()
    {
        if (!HasJoinedRoom) return;

        if (GameOver)
        { 
            //Game is over before player can join
            ReloadLobby();
            return;
        }

        //Get info an all players in room
        ListAndIndexPlayersInRoom();

        //Monitor current player for inactivity
        CheckForInactivity();       

        //Start countdown to start of game when all players are ready
        if (!CountdownTimerActive)
            CountdownTimerActive = InitiateGameCountdown();
        else
            CountdownTimerActive = IncrementGameCountdown();
    }

    //Called from button in lobby
    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = MaxPlayerNumber };
        bool success = PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
        if(!success)
        {
            Debug.Log("Failed to join room");

            //Hack to avoid lobby deadlock
            object check = PhotonNetwork.player.CustomProperties[GameConstants.NetworkedProperties.Ready];
            if (check != null)
            {
                Debug.Log("Reinitializing lobby scene");
                Start();
            }

            return;
        }

        Debug.Log("Player joining game");
        JoinGamePanel.SetActive(false);
        LaunchGamePanel.SetActive(true);

        ColorBlock cb = ReadyButton.colors;
        cb.normalColor = Color.red;
        cb.highlightedColor = Color.red;
        ReadyButton.colors = cb;
    }

    //Called from button in lobby
    public void PlayerReady()
    {
        if (LastKnownPlayerCount < 2) return;

        ReadyMessage.GetComponent<TextMeshProUGUI>().text = "Väntar på andra";

        ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
        PropertyTable.Add(GameConstants.NetworkedProperties.Ready, true);
        PhotonNetwork.player.SetCustomProperties(PropertyTable);

        ColorBlock cb = ReadyButton.colors;
        cb.normalColor = Color.green;
        cb.highlightedColor = Color.green;
        ReadyButton.colors = cb;

        ReadyToStartGame = true;
        SecondsInactive = 0.0f;
    }

    void LaunchGame()
    {
        ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
        PropertyTable.Add(GameConstants.NetworkedProperties.InGame, true);
        PhotonNetwork.player.SetCustomProperties(PropertyTable);

        if (IsAr)
            PhotonNetwork.LoadLevel(GameConstants.SceneNames.OnlineAR);
        else
            PhotonNetwork.LoadLevel(GameConstants.SceneNames.OnlineVR);
    }

    void CheckForInactivity()
    {
        if (!ReadyToStartGame)
            SecondsInactive += Time.deltaTime;

        if (SecondsInactive > InactifityTimeThreshold)
            ReloadLobby();
    }

    //Returns true if conditions are met so the game can start
    bool InitiateGameCountdown()
    {        
        if (AreAllPlayersReady())
        {
            ReadyMessage.GetComponent<TextMeshProUGUI>().text = "Spelet börjar snart";

            CountdowntimerValue = 5;
            CountdownTimer.GetComponent<TextMeshProUGUI>().text = "Börjar om " + ((int)Math.Round(CountdowntimerValue)).ToString();
            return true;
        }
      
        return false;
    }

    //Ticks towards start of game, returns true while countdown is valid
    bool IncrementGameCountdown()
    {
        //Too many players have left, cancel countdown
        if (LastKnownPlayerCount < 2 || !AreAllPlayersReady())
        {
            CountdowntimerValue = CountdowntimerLength;
            CountdownTimer.GetComponent<TextMeshProUGUI>().text = "";
            ReadyMessage.GetComponent<TextMeshProUGUI>().text = "";

            ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
            PropertyTable.Add(GameConstants.NetworkedProperties.Ready, false);
            PhotonNetwork.player.SetCustomProperties(PropertyTable);

            ColorBlock cb = ReadyButton.colors;
            cb.normalColor = Color.red;
            cb.highlightedColor = Color.red;
            ReadyButton.colors = cb;

            ReadyToStartGame = false;

            return false;
        }

        CountdowntimerValue -= Time.deltaTime;
        CountdownTimer.GetComponent<TextMeshProUGUI>().text = "Börjar om " + ((int)Math.Round(CountdowntimerValue)).ToString();
        if (CountdowntimerValue < 0.0f)
            LaunchGame();

        return true;
    }

    bool AreAllPlayersReady()
    {
        bool AllPlayersReady = true;
        foreach (PhotonPlayer Player in PhotonNetwork.playerList)
        {
            //For newly joined players this property can be null
            object check = Player.CustomProperties[GameConstants.NetworkedProperties.Ready];
            if (check == null) return false;

            bool PlayerReady = (bool)Player.CustomProperties[GameConstants.NetworkedProperties.Ready];
            AllPlayersReady = AllPlayersReady && PlayerReady;
        }
        return AllPlayersReady;
    }

    public void ReloadLobby()
    {
        //Have to wait until all players have exited game
        foreach (PhotonPlayer Player in PhotonNetwork.otherPlayers)
        {
            //For newly joined players this property can be null
            object check = Player.CustomProperties[GameConstants.NetworkedProperties.InGame];
            if (check == null) return;

            bool PlayerInGame = (bool)Player.CustomProperties[GameConstants.NetworkedProperties.InGame];
            if (PlayerInGame) return;
        }

        HasJoinedRoom = false;
        Debug.Log("Leaving room " + PhotonNetwork.countOfPlayers);
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
        PhotonNetwork.LeaveRoom();
    }

    Dictionary<long, PhotonPlayer> GetPlayerTimestampMap()
    {
        Dictionary<long, PhotonPlayer> res = new Dictionary<long, PhotonPlayer>();
        foreach (PhotonPlayer Player in PhotonNetwork.playerList)
        {
            //Will be null if other player hasn't had time to start up
            object check = Player.CustomProperties[GameConstants.NetworkedProperties.Stamp];
            if (check == null) return null;

            long Stamp = (long)Player.CustomProperties[GameConstants.NetworkedProperties.Stamp];
            res.Add(Stamp, Player);
        }
        return res;
    }

    void ListAndIndexPlayersInRoom()
    {
        int NumberOfPlayers = PhotonNetwork.room.PlayerCount;

        if (NumberOfPlayers != LastKnownPlayerCount)
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
                String PlayerVisibleId = PlayerIndex.ToString();
                if (Player.ID == PhotonNetwork.player.ID)
                {
                    //Static ID used to instantiate character in game scene, only set for local player
                    GameConstants.NetworkedPlayerID = PlayerIndex;
                    PlayerVisibleId += "  <- Här är du";
                }

                PlayerNames += names[Convert.ToInt32(PlayerIndex)] + " " + PlayerVisibleId;
                PlayerNames += "\n";
                PlayerIndex++;
            }

            GameObject.Find(GameConstants.GameObjectsTags.playerListText).GetComponent<TextMeshProUGUI>().text = PlayerNames;
            //GameObject.FindGameObjectWithTag(GameConstants.GameObjectsTags.playerNumText).GetComponent<TextMeshProUGUI>().text = NumberOfPlayers.ToString();
            LastKnownPlayerCount = NumberOfPlayers;
        }
    }

    //Photon callback
    void OnJoinedLobby()
    {
        Debug.Log("JOINED LOBBY");
    }
    
    //Photon callback
	void OnJoinedRoom()
    {
        Debug.Log("JOINED ROOM");

        HasJoinedRoom = true;
        PlayerID = DateTime.Now.Ticks;
        string RoomName = PhotonNetwork.room.Name;
        int NumberOfPlayers = PhotonNetwork.room.PlayerCount;
        
        ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
        PropertyTable.Add(GameConstants.NetworkedProperties.Stamp, PlayerID);
        PhotonNetwork.player.SetCustomProperties(PropertyTable);

        //GameObject.FindGameObjectWithTag(GameConstants.GameObjectsTags.roomNameText).GetComponent<TextMeshProUGUI>().text = RoomName;
        //GameObject.FindGameObjectWithTag(GameConstants.GameObjectsTags.playerNumText).GetComponent<TextMeshProUGUI>().text = NumberOfPlayers.ToString();
    }

    //Photon callback
    public void OnLeftRoom()
    {
        Debug.Log("Loading lobby scene");
        PhotonNetwork.LoadLevel(GameConstants.SceneNames.Lobby);
    }
}
