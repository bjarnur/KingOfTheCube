using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class KingNetwork : Photon.MonoBehaviour
{

    bool isAlive = true;
    public Vector3 position;
    public Quaternion rotation;
    public float larpSmoothing = 10f;

    // Use this for initialization
    void Start()
    {
        if (photonView.isMine)
        {
            gameObject.name = "Local King";
            //TODO Use appropriate controller depending on VR or AR (need a permanent solution for this)
            //GetComponent<PlayerController_AssemCube>().enabled = true;
            //GetComponent<CharacterCtrl>().enabled = true;
            //GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            gameObject.name = "Network King";
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
            var controller = GetComponent<KingController_AR>();
            //var controller = GetComponent<KingController_AssemCube>();
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
                animator.SetBool(GameConstants.AnimationNames.runAnimation, false);
                animator.SetBool(GameConstants.AnimationNames.stopAnimation, true);
                break;
            case GameConstants.AnimationTypes.running:
                animator.SetBool(GameConstants.AnimationNames.stopAnimation, false);
                animator.SetBool(GameConstants.AnimationNames.runAnimation, true);
                break;
            case GameConstants.AnimationTypes.throwing:
                animator.SetBool(GameConstants.AnimationNames.throwAnimation, true);
                animator.SetBool(GameConstants.AnimationNames.runAnimation, false);
                animator.SetBool(GameConstants.AnimationNames.stopAnimation, false);
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
