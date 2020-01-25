using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaser : Laser {

    [SerializeField] float timeBetweenMovement = 2f;
    [SerializeField] float movingSpeed = .5f;

    Vector3 startingPos, endingPos;

    void Start() {
        startingPos = gameObject.transform.position;
        endingPos = transform.GetChild(0).position;
        StartCoroutine("Move");    
    }

    IEnumerator Move() {
        while (true) {
            while (Vector3.Distance(transform.position, endingPos) > .5f) {
                transform.Translate(-transform.right*movingSpeed*Time.deltaTime, Space.World);
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenMovement);


            while (Vector3.Distance(transform.position, startingPos) > .5f) {
                transform.Translate(transform.right*movingSpeed*Time.deltaTime, Space.World);
                yield return null;
            }
            yield return new WaitForSeconds(timeBetweenMovement);
        }
    }

}
