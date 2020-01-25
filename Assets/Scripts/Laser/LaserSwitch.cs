using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LaserSwitch : MonoBehaviour {
    [SerializeField] Laser[] lasers;

    bool used = false;

    GameObject buttonToShow;

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !used) {
#if MOBILE_INPUT
            buttonToShow = other.GetComponent<PlayerInput>().actionButton;
            buttonToShow.SetActive(true);
#endif
        }    
    }

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player" && !used){
            if (other.GetComponent<PlayerInput>().isActing) {
                foreach(Laser laser in lasers) {
                    laser.Disable();
                }
                AudioManager.instance.PlayLaserSwitch();
                used = true;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            if (buttonToShow)
                buttonToShow.SetActive(false);
        }
    }

}
