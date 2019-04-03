using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects the player.

// TODO: The current way to see what is being collided with is really bad. Change it to something that isn't just convienient.

public class RadarAlert : MonoBehaviour {
	public GameObject player;
	public GameObject alertIcon;

	void OnTriggerEnter(Collider touched)
	{
		if (touched.attachedRigidbody.mass == 0.1f) // TODO Make this not depend on mass
		{
			alertIcon.GetComponent<TextMesh>().text = "!"; // TODO: This is a placeholder for now. it should cause the enemy to chase/attack/call help, etc.
			player.GetComponent<SubMovement>().Detonate();
		}
	}
}
