using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			other.tag = "Untagged";
			other.GetComponent<Animator>().SetBool("IsMoving", false);
			other.GetComponent<PlayerInput>().enabled = false;
			AudioManager.instance.StopAllCoroutines();
			AudioManager.instance.themeSource.Stop();
			EnemyManager.instance.LastPlayerKnownPosition = EnemyManager.instance.ResetPosition;
			GameManager.instance.LevelComplete();
			AudioManager.instance.PlayVictory();
		}
	}
}
