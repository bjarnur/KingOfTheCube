using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class KingNetwork : Photon.MonoBehaviour {

    bool isAlive = true;
    public Vector3 position;
    public Quaternion rotation;
    public float larpSmoothing = 10f;
    public GameConstants.AnimationTypes currentAnimation;

	// Use this for initialization
	void Start () {
        if (photonView.isMine) {
            gameObject.name = "Local King";
            //TODO Use appropriate controller depending on VR or AR (need a permanent solution for this)
            //GetComponent<PlayerController_AssemCube>().enabled = true;
            GetComponent<CharacterCtrl>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
        }
        else {
            gameObject.name = "Network King";
            StartCoroutine("Alive");
            //this.transform.SetParent(OmniscientController.GetInstance().worldContainer);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        //GetComponent<CharacterCtrl>().enabled = true;        
        if (stream.isWriting) {
            //TODO Use appropriate controller depending on VR or AR (need a permanent solution for this)
            //var controller = GetComponent<PlayerController_AssemCube>();
            var controller = GetComponent<CharacterCtrl>();
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
            stream.SendNext(controller.currentAnimation);
        }
        else
        {
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
}
