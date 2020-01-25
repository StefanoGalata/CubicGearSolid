using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other) {
        if(other is CapsuleCollider){
             if(other.tag == "Player" || (other.tag == "Patrol")) {
                anim.SetBool("ShouldOpen", true);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if(other is CapsuleCollider){
            if(other.tag == "Player" || (other.tag == "Patrol")) {
                anim.SetBool("ShouldOpen", false);
            } 
        }   
    }
}
