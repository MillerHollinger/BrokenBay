using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spins a ship's radar around.

// TODO: Spin speed is affected by the spin speed of the attached ship. Should be same always.

public class RadarSpinner : MonoBehaviour {
public float spinSpeed;

	void Start() {
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
	}
}
