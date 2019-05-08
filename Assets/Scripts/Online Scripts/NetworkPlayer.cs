using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;

public class NetworkPlayer : Photon.MonoBehaviour {
    
    bool isAlive = true;
    public Vector3 position;
    public Quaternion rotation;
    public float larpSmoothing = 10f;
    public GameConstants.AnimationTypes currentAnimation;
    public bool isAR = false;

    private bool BufferedGameLost = false;
    private NetworkManager networkManager;
    private LobbyManager lobbyManager;

    void Awake () {

        if (photonView.isMine)
        {            
            gameObject.name = "Local Player";
        }
        else
        {
            gameObject.name = "Network Player";
            StartCoroutine("UpdateNetworked");
            DontDestroyOnLoad(this.gameObject);

            Destroy(gameObject.transform.Find("Indicator").gameObject);
        }
	}

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == GameConstants.SceneNames.OnlineAR || scene.name == GameConstants.SceneNames.OnlineVR)
        {
            networkManager = GameObject.Find(GameConstants.ObjecNames.NetworkManager).GetComponent<NetworkManager>();
        }
        else
        {
            SceneManager.sceneLoaded += SetNetworkManagerOnSceneLoad;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {     
        if (stream.isWriting){
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
            if(isAR)
            {
                stream.SendNext(GetComponent<CharacterCtrl>().currentAnimation);
            }            
            else
            {
                stream.SendNext(GetComponent<PlayerController_AssemCube>().currentAnimation);
            }
        }
        else {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            currentAnimation = (GameConstants.AnimationTypes)stream.ReceiveNext();
            SetCharacterAnimation(GetComponent<Animator>(), currentAnimation);
        }
    }

    void SetCharacterAnimation(Animator animator, GameConstants.AnimationTypes animationType) {
        Debug.Log(animationType);
        switch (animationType)
        {
            case GameConstants.AnimationTypes.stopped:
                animator.SetBool("Fall", false);
                animator.SetBool("Climb", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Run", false);
                animator.SetBool("Stop", true);
                break;
            case GameConstants.AnimationTypes.running:
                animator.SetBool("Fall", false);
                animator.SetBool("Climb", false);
                animator.SetBool("Jump", false);
                animator.SetBool("Stop", false);
                animator.SetBool("Run", true);                
                break;
            case GameConstants.AnimationTypes.jumping:
                animator.SetBool("Run", false);
                animator.SetBool("Stop", false);
                animator.SetBool("Run", false);
                animator.SetBool("Jump", true);
                break;
            case GameConstants.AnimationTypes.climbing:
                animator.SetBool("Run", false);
                animator.SetBool("Stop", false);
                animator.SetBool("Climb", true);
                animator.SetBool("Jump", false);
                break;
            case GameConstants.AnimationTypes.falling:
                animator.SetBool("Run", false);
                animator.SetBool("Stop", false);
                animator.SetBool("Fall", true);
                break;
        }
    }

    /*
     For smooth transistion of networked players */
    IEnumerator UpdateNetworked()
    {
        while(isAlive)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * larpSmoothing);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * larpSmoothing);
            yield return null;
        }
    }

    [PunRPC]
    void die()
    {
        if (isAR)
        {
            GetComponent<Animator>().SetTrigger("Die");
            var controller = GetComponent<CharacterCtrl>();
            controller.dead = true;
            StartCoroutine(controller.Dying());

        }
        else
        {
            GetComponent<Animator>().SetTrigger("Die");
            var controller = GetComponent<PlayerController_AssemCube>();            
            controller.dead = true;
            StartCoroutine(controller.Dying());
            
            //Use this if we don't want players to respawn
            //isAlive = false;
        }

    }

    [PunRPC]
    void GameOver()
    {
        Debug.Log("game over rpc");

        if (isAR)
        {
            var Players = GameObject.FindGameObjectsWithTag(GameConstants.ARPLAYERTAG);
            foreach(var Player in Players)
            {
                Player.GetComponent<CharacterCtrl>().gameOver = true;
            }
        }
            
        else
        {
            var Players = GameObject.FindGameObjectsWithTag(GameConstants.UNITYPLAYERTAG);
            foreach(var Player in  Players)
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
        else if(scene.name == GameConstants.SceneNames.Lobby)
        {
            lobbyManager = GameObject.Find(GameConstants.ObjecNames.LobbyManager).GetComponent<LobbyManager>();
            lobbyManager.GameOver = true;
        }
        else
            BufferedGameLost = true;
    }

    void SetNetworkManagerOnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Network player scene callback");
        if (scene.name == GameConstants.SceneNames.OnlineAR || scene.name == GameConstants.SceneNames.OnlineVR)
        {
            networkManager = GameObject.Find(GameConstants.ObjecNames.NetworkManager).GetComponent<NetworkManager>();
            SceneManager.sceneLoaded -= SetNetworkManagerOnSceneLoad;

            if (BufferedGameLost)
                networkManager.GameLost();
        }        
    }
}
