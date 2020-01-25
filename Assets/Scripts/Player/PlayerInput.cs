using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerInput : MonoBehaviour {

	[SerializeField] float speed = 3f;
	[SerializeField] float turningRate = 7.5f;
	bool isMoving = false;
    bool wasHit = false;
    public bool isActing = false;

    public GameObject actionButton;


    public bool IsMoving {
        get { return isMoving; }
    }

	Animator anim;

	void Awake(){
		anim = GetComponent<Animator>();
	}
	
	void Update () {
        anim.SetBool("IsMoving", isMoving);
        if(wasHit){
            isMoving = false;
            return;
        }

		float verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");
		float horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");

		//Setting animation params
		isMoving = Mathf.Abs(verticalAxis)+Mathf.Abs(horizontalAxis)>0.1f ? true : false;

		//Movement
		if(Mathf.Abs(horizontalAxis)>.1f || Mathf.Abs(verticalAxis)>.1f){
           
            Vector3 movementDirection = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;
            RaycastHit hit;

            //TODO Refactor this
            //Need this to get rid of bug that pushes through the wall
            Ray rayForward = new Ray(transform.position+transform.up, transform.forward);
            if (!Physics.Raycast(rayForward, out hit, .5f)) {
                MoveToDirection(movementDirection);
            }

            else if (hit.collider.isTrigger) {
                MoveToDirection(movementDirection);
            }
            RotateToDirection(movementDirection);
        }

        isActing = CrossPlatformInputManager.GetButtonDown("Submit");

        if (CrossPlatformInputManager.GetButtonDown("Cancel")) {
            GameManager.instance.TogglePause();
        }
    }


    void MoveToDirection(Vector3 dir){
		transform.Translate(dir*speed*Time.deltaTime, Space.World);
	}

	void RotateToDirection(Vector3 dir){
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turningRate*Time.deltaTime);
	}

	void PlayStepSound(){
		AudioManager.instance.PlayStepSound();
	}

    public IEnumerator Freeze(float amount){
        wasHit = true;
        yield return new WaitForSeconds(amount);
        wasHit = false;
    }
}
