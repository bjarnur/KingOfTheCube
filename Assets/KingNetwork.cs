using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class KingNetwork : Photon.MonoBehaviour
{

    bool isAlive = true;
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Quaternion rotation = new Quaternion(0,0,0,0);
    public float larpSmoothing = 10f;

    // Use this for initialization
    void Start()
    {
        if (photonView.isMine)
        {
            gameObject.name = "Local King";
            //TODO Use appropriate controller depending on VR or AR (need a permanent solution for this)
            GetComponent<PlayerController_AssemCube>().enabled = true;
            GetComponent<CharacterCtrl>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            gameObject.name = "NetworkKing";
            StartCoroutine("UpdateNetworked");
            //this.transform.SetParent(OmniscientController.GetInstance().worldContainer);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //GetComponent<CharacterCtrl>().enabled = true;        
        if (stream.isWriting)
        {
            //TODO Use appropriate controller depending on VR or AR (need a permanent solution for this)
            //var controller = GetComponent<PlayerController_AssemCube>();
            //var controller = GetComponent<KingController_AR>();
            var controller = GetComponent<KingController_AssemCube>();
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
            stream.SendNext(controller.currentAnimation);
        }
        else
        {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
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
}
