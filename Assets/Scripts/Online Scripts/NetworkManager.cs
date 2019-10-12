using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class NetworkManager : MonoBehaviour {

    public GameObject WinText;
    public GameObject LoseText;
    public GameObject GameTimer;
    public bool gameOver = false;
    public bool isAR;

    const string VERSION = "0.0.1";
    private string roomName = "myRoom";
    [HideInInspector] public bool IsInactive = false;
    [HideInInspector] public float SecondsInactive = 0.0f;

    private float timeAlone = 0.0f;

    void Start()
    {        
        if (!isAR)
        {
            int PlayerIndex = GameConstants.NetworkedPlayerID;
            Debug.Log("Registering player " + PlayerIndex);
            if (PlayerIndex == 0)
                spawnKing();
            else
                spawnPretender(PlayerIndex);
        }        
    }

    void Update()
    {
        SecondsInactive += Time.deltaTime;
        if(SecondsInactive > 30.0f)
        {
            IsInactive = true;
            ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
            PropertyTable.Add(GameConstants.NetworkedProperties.Inactive, true);
            PhotonNetwork.player.SetCustomProperties(PropertyTable);
        }
        else
        {
            IsInactive = false;
            ExitGames.Client.Photon.Hashtable PropertyTable = new ExitGames.Client.Photon.Hashtable();
            PropertyTable.Add(GameConstants.NetworkedProperties.Inactive, false);
            PhotonNetwork.player.SetCustomProperties(PropertyTable);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToLobby();
        }

        if (IsInactive)
        {
            bool AllPlayersInactive = true;
            foreach (PhotonPlayer Player in PhotonNetwork.playerList)
            {
                bool PlayerReady = (bool)Player.CustomProperties[GameConstants.NetworkedProperties.Inactive];
                AllPlayersInactive = AllPlayersInactive && PlayerReady;
            }
            if (AllPlayersInactive) ExitToLobby();
        }

        int ActivePlayers = PhotonNetwork.playerList.Length;
        if(ActivePlayers == 1)
        {
            timeAlone += Time.deltaTime;
            if (timeAlone > 5.0f) ExitToLobby();
        }
        else
        {
            timeAlone = 0.0f;
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
        NetworkKing networkPlayer = newPlayer.GetComponent<NetworkKing>();
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

    public void GameWon()
    {
        if (gameOver) return;

        Debug.Log("Game Won!");
        gameOver = true;
        WinText.SetActive(true);
        StartCoroutine(WaitForSecondsThenExit(4));
    }

    public void GameLost()
    {
        if (gameOver) return;

        Debug.Log("Game Lost!");
        gameOver = true;
        LoseText.SetActive(true);
        StartCoroutine(WaitForSecondsThenExit(4));
    }

    public void UpdateTimer(int seconds)
    {
        if (gameOver) return;

        int m = (seconds / 60);
        int s = (seconds % 60);

        string mins;
        string secs;
        if (m < 10)
            mins = "0" + m.ToString();
        else
            mins = m.ToString();
        if (s < 10)
            secs = "0" + s.ToString();
        else
            secs = s.ToString();

        GameTimer.GetComponent<TextMeshProUGUI>().SetText(mins + ":" + secs);
    }

    IEnumerator WaitForSecondsThenExit(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        ExitToLobby();
    }

    void ExitToLobby()
    {
        Debug.Log("Leaving room " + PhotonNetwork.countOfPlayers);
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
        PhotonNetwork.LeaveRoom();
    }

    public void OnLeftRoom()
    {
        Debug.Log("Loading lobby scene");
        PhotonNetwork.LoadLevel(GameConstants.SceneNames.Lobby);
    }
}
