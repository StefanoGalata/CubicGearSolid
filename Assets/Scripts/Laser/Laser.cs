using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Laser : MonoBehaviour {

    private BoxCollider col;

    void Awake() {
        col = GetComponent<BoxCollider>();    
    }

    protected bool shouldPlaySound = true;

	protected virtual void OnTriggerEnter(Collider other){
		if(other.tag == "Player" && other.GetComponent<PlayerInput>().IsMoving){
			StartCoroutine(GameManager.instance.LaserTriggered(other.transform.position));
			shouldPlaySound = false;
		}
	}


    protected virtual void OnTriggerExit(Collider other){
		if(other.tag == "Player" && other.GetComponent<PlayerInput>().IsMoving){
			StartCoroutine(GameManager.instance.LaserTriggered(other.transform.position, shouldPlaySound));
			shouldPlaySound = true;
		}
	}

    public void Disable() {
        col.enabled = false;
        transform.GetChild(3).gameObject.SetActive(false);
    }
}
