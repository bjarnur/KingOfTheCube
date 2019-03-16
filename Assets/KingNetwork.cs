using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class KingNetwork : Photon.MonoBehaviour
{    
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Quaternion rotation = new Quaternion(0,0,0,0);

    public float larpSmoothing = 10f;

    private bool isAR = true;

    // Use this for initialization
    void Start()
    {
        if (photonView.isMine)
        {
            gameObject.name = "Local King";

            object[] InstanceData = gameObject.GetPhotonView().instantiationData;
            if((string) InstanceData[0] == "VR")
            {
                isAR = false;
                GetComponent<CharacterCtrl>().enabled = true;
                GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else
        {
            gameObject.name = "NetworkKing";
            StartCoroutine("UpdateNetworked");
        }
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
                stream.SendNext(controller.currentAnimation);
            }
            else
            { 
                var controller = GetComponent<KingController_AssemCube>();
                stream.SendNext(transform.localPosition);
                stream.SendNext(transform.localRotation);
                stream.SendNext(controller.currentAnimation);
            }
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
