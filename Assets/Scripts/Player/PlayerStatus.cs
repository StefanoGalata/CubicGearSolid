using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
	public delegate void Status();
	public static event Status currentStatus;

	[SerializeField] private float invisibleTime;

	public static float invisibilityTime=1;

	void Start(){
		invisibilityTime = invisibleTime;
	}

	void Update(){
		if(currentStatus != null)
			currentStatus();
	}

}
