using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;

public class NetworkKing : Photon.MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Quaternion rotation = new Quaternion(0,0,0,0);

    public float gameTimer = 60;
    public float larpSmoothing = 10f;

    private bool gameOver = false;
    private bool isLocal = false;
    private bool isAR = true;
    private bool bufferedGameLost = false;
    private NetworkManager networkManager;
    private LobbyManager lobbyManager;

    // Use this for initialization
    void Start()
    {
        //Ensure all king instances will have access to network manager
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == GameConstants.SceneNames.OnlineAR || scene.name == GameConstants.SceneNames.OnlineVR)
            networkManager = GameObject.Find(GameConstants.ObjecNames.NetworkManager).GetComponent<NetworkManager>();
        else
            SceneManager.sceneLoaded += SetNetworkManagerOnSceneLoad;

        if (photonView.isMine)
        {
            isLocal = true;
            gameObject.name = "Local King";

            //Special configuration for running on PC
            object[] InstanceData = gameObject.GetPhotonView().instantiationData;
            if((string) InstanceData[0] == "VR")
            {
                isAR = false;
                GetComponent<KingController_AssemCube>().enabled = true;
                GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else
        {
            gameObject.name = "NetworkKing";
            StartCoroutine("UpdateNetworked");
            DontDestroyOnLoad(this.gameObject);

            Destroy(gameObject.transform.Find("Indicator").gameObject);
        }
    }

    void Update()
    {
        if (gameOver) return;

        if(isLocal)
        { 
            gameTimer -= Time.deltaTime;

            if (gameTimer < 0.0)
            {
                GetComponent<PhotonView>().RPC("GameOver", PhotonTargets.OthersBuffered);
                networkManager.GameWon();
                gameOver = true;
                return;
            }
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == GameConstants.SceneNames.OnlineAR || scene.name == GameConstants.SceneNames.OnlineVR)
            networkManager.UpdateTimer((int) gameTimer);

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (isAR)
            { 
                var controller = GetComponent<KingController_AR>();
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
                stream.SendNext(gameTimer);
                stream.SendNext(controller.currentAnimation);
            }
            else
            { 
                var controller = GetComponent<KingController_AssemCube>();
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
                stream.SendNext(gameTimer);
                stream.SendNext(controller.currentAnimation);
            }
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            gameTimer = (float)stream.ReceiveNext();
            var currentAnimation = (GameConstants.AnimationTypes)stream.ReceiveNext();
            SetCharacterAnimation(GetComponent<Animator>(), currentAnimation);
        }
    }

    void SetCharacterAnimation(Animator animator, GameConstants.AnimationTypes animationState)
    {
        Debug.Log("Networked king animation: " + animationState);
        switch (animationState)
        {
            case GameConstants.AnimationTypes.stopped:
                animator.SetBool(GameConstants.KingAnimationNames.throwAnimation, false);
                animator.SetBool(GameConstants.KingAnimationNames.runAnimation, false);

                break;
            case GameConstants.AnimationTypes.running:
                animator.SetBool(GameConstants.KingAnimationNames.throwAnimation, false);
                animator.SetBool(GameConstants.KingAnimationNames.runAnimation, true);
                break;
            case GameConstants.AnimationTypes.throwing:
                animator.SetBool(GameConstants.KingAnimationNames.throwAnimation, true);
                animator.SetBool(GameConstants.KingAnimationNames.runAnimation, false);

                break;
        }
    }

    /*
     For smooth transistion of networked players */
    IEnumerator UpdateNetworked()
    {
        while (true)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * larpSmoothing);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * larpSmoothing);
            yield return null;
        }
    }

    [PunRPC]
    void GameOver()
    {
        Debug.Log("game over rpc");
        gameOver = true;

        if (isAR)
        {
            var Players = GameObject.FindGameObjectsWithTag(GameConstants.ARPLAYERTAG);
            foreach (var Player in Players)
            {
                Player.GetComponent<CharacterCtrl>().gameOver = true;
            }
        }

        else
        {
            var Players = GameObject.FindGameObjectsWithTag(GameConstants.UNITYPLAYERTAG);
            foreach (var Player in Players)
            {
                Player.GetComponent<PlayerController_AssemCube>().gameOver = true;
            }
        }

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == GameConstants.SceneNames.OnlineAR || scene.name == GameConstants.SceneNames.OnlineVR)
        {
            //Winning player instantiates the RPC, hence all players that run this have lost
            networkManager.GameLost();
        }
        else if (scene.name == GameConstants.SceneNames.Lobby)
        {
            lobbyManager = GameObject.Find(GameConstants.ObjecNames.LobbyManager).GetComponent<LobbyManager>();
            lobbyManager.GameOver = true;
        }
        else
            bufferedGameLost = true;
    }

    void SetNetworkManagerOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Network player scene callback");
        if (scene.name == GameConstants.SceneNames.OnlineAR || scene.name == GameConstants.SceneNames.OnlineVR)
        {
            networkManager = GameObject.Find(GameConstants.ObjecNames.NetworkManager).GetComponent<NetworkManager>();
            SceneManager.sceneLoaded -= SetNetworkManagerOnSceneLoad;

            if (bufferedGameLost)
                networkManager.GameLost();
        }
    }
}
