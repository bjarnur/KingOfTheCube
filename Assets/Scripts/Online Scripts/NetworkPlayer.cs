using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkPlayer : Photon.MonoBehaviour {

    bool isAlive = true;
    public Vector3 position;
    public Quaternion rotation;
    public float larpSmoothing = 10f;
    public GameConstants.AnimationTypes currentAnimation;

    // Use this for initialization
    void Start () {
        if(photonView.isMine)
        {
            gameObject.name = "Local Player";
            //GetComponent<CharacterCtrl>().enabled = true;
            GetComponent<PlayerController_AssemCube>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            gameObject.name = "Network Player";
            StartCoroutine("Alive");
            //this.transform.SetParent(OmniscientController.GetInstance().worldContainer);
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        //GetComponent<CharacterCtrl>().enabled = true;        
        if (stream.isWriting){
            var controller = GetComponent<PlayerController_AssemCube>();
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(controller.currentAnimation);
        }
        else {
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            currentAnimation = (GameConstants.AnimationTypes)stream.ReceiveNext();
            SetCharacterAnimation(GetComponent<Animator>(), currentAnimation);
        }
    }

    void SetCharacterAnimation(Animator animator, GameConstants.AnimationTypes animation)
    {
        Debug.Log(animation);
        switch (animation)
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
    IEnumerator Alive()
    {
        while(isAlive)
        {
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * larpSmoothing);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * larpSmoothing);

            yield return null;
        }
    }
}
