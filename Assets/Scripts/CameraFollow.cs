using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Locks the camera to the player's position.

public class CameraFollow : MonoBehaviour {
	public GameObject toFollow; // what is being followed by the camera
	public float distanceAbove; // how far above to follow
	public float distanceBehind; // how far behind to follow

	// Use this for initialization
	void Start () {
		// look at the follow object
		transform.LookAt(toFollow.GetComponent<Transform>());
	}

	// Update is called once per frame
	void Update () {
		transform.LookAt(toFollow.GetComponent<Transform>());
		transform.position = new Vector3(toFollow.transform.position.x, toFollow.transform.position.y + distanceAbove, toFollow.transform.position.z - distanceBehind);
	}
}
