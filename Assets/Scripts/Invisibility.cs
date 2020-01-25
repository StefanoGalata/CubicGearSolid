using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : MonoBehaviour {
	private float timer =0;

	private GameObject player = null;
	private int playerStartingLayer=0;
	private Renderer playerRenderer;
	private Shader currentShader;
	private Color currentColor;
	
	[SerializeField]
	private Shader transparentShader;

	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			AudioManager.instance.PlayPickUp();
			player = other.gameObject;
			playerStartingLayer = player.layer;
			player.layer = 10;
			playerRenderer = player.GetComponentInChildren<SkinnedMeshRenderer>();
			currentShader = playerRenderer.material.shader;
			currentColor = playerRenderer.material.color;
			
			playerRenderer.material.shader = transparentShader;
			Debug.Log(playerRenderer.material.shader.name);
			playerRenderer.material.color = new Color(1,1,1,.2f);

			PlayerStatus.currentStatus+=UpdateStatus;
			Destroy(this.gameObject);
		}

	}

	void UpdateStatus(){
		timer+=Time.deltaTime;
		if(timer>PlayerStatus.invisibilityTime && player != null){
			player.layer=playerStartingLayer;
			player.GetComponentInChildren<Renderer>().material.shader = currentShader;
			playerRenderer.material.color = currentColor;
			Debug.Log("Time done");
			PlayerStatus.currentStatus-=UpdateStatus;
		}

	}
}
