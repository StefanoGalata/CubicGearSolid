using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Vector2 velocity;

	[SerializeField]
	private float smoothTimeX = 0f;
	[SerializeField]
	private float smoothTimeY = 0f;


	public bool bounds = true;

	public Vector3 minCamPos;
	public Vector3 maxCamPos;

	public GameObject player;

	float nextTimeToSearch;

	private Vector3 offset;


	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		offset = player.transform.position - transform.position;
	}

	void LateUpdate(){
		if (player != null) {
			float posX = Mathf.SmoothDamp (transform.position.x, player.transform.position.x-offset.x, ref velocity.x, smoothTimeX);
			float posZ = Mathf.SmoothDamp (transform.position.z, player.transform.position.z-offset.z, ref velocity.y, smoothTimeY);

			transform.position = new Vector3 (posX, transform.position.y, posZ);
			if (bounds) {
				transform.position = new Vector3 (
					Mathf.Clamp (transform.position.x, minCamPos.x, maxCamPos.x),
					Mathf.Clamp (transform.position.y, minCamPos.y, maxCamPos.y),
					Mathf.Clamp (transform.position.z, minCamPos.z, maxCamPos.z)
				);
			}
		}
		else {
			FindPlayer ();
		}
	}

	void FindPlayer(){
		if (nextTimeToSearch <= Time.time) {
			GameObject searchResult=GameObject.FindGameObjectWithTag ("Player");
			if (searchResult != null)
				player = searchResult;
			nextTimeToSearch = Time.time + 0.5f;
		}

	}
}
