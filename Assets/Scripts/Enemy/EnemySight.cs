using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {
	[SerializeField] float FOVAngle = 110f;
	[SerializeField] LayerMask layersDetectedByRaycast;

	public bool playerInSight = false;
	public Vector3 currentPlayerPosition;

	private SphereCollider col;

	void Awake(){
		col = GetComponent<SphereCollider>();
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "Player" && !GameManager.instance.PlayerDied){
			playerInSight = false;
			Vector3 direction = other.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			
			//if the angle is less then half of the FOV angle then we should check if the player is behind a wall or any obstruction
			if(angle < FOVAngle*0.5f){
				RaycastHit hit;
				if(Physics.Raycast(transform.position+transform.up, direction.normalized, out hit, col.radius, layersDetectedByRaycast)){
					if(hit.collider.tag == "Player"){
                        playerInSight = true;
                        SetPlayerLastKnownPosition(other.transform.position);
						currentPlayerPosition = other.transform.position;
					}
				}
			}
		}
        else playerInSight = false;
    }


	void SetPlayerLastKnownPosition(Vector3 position){
		EnemyManager.instance.LastPlayerKnownPosition = position;
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Player")
			playerInSight = false;
	}
}
