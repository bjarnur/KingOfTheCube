﻿using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

    public GameObject WinText;
    public GameObject LoseText;
    public bool isAR;

    const string VERSION = "0.0.1";
    private string roomName = "myRoom";
    [HideInInspector] public bool IsInactive = false;

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
        Debug.Log("Game Won!");
        WinText.SetActive(true);
        StartCoroutine(WaitForSecondsThenExit(4));
    }

    public void GameLost()
    {
        Debug.Log("Game Lost!");
        LoseText.SetActive(true);
        StartCoroutine(WaitForSecondsThenExit(4));
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
