using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Test of enemy movement.
// The ship will wander around randomly, changing directions every 10 seconds.

public class EnemyShipMovementTest : MonoBehaviour {

private float tillNextDirChange;
public float speed;
public float rotSpeed;
private Vector3 direction; // direction of movement as V3
private Quaternion directionQ; // direction of movement as Q

	// Use this for initialization
	void Start () {
		tillNextDirChange = 10;
		// Generate starting direction
		direction = genDir();
		Debug.Log("Starting dir is " + direction);
		directionQ = Quaternion.Euler(direction);
	}

	// Update is called once per frame
	void Update () {

		// Turn ship towards direction
		transform.rotation = Quaternion.Lerp(transform.rotation, directionQ, Time.deltaTime * rotSpeed);

		// Move in direction ship is facing
		transform.position += transform.forward * Time.deltaTime * speed;

		// Pick a new direction every five seconds
		if (tillNextDirChange <= 0)
		{
				Debug.Log("New dir is " + direction);
				tillNextDirChange = 10;
				// Generate new direction
				direction = genDir();
				directionQ = Quaternion.Euler(direction);
		}
		tillNextDirChange -= Time.deltaTime;

	}

	Vector3 genDir() {
		return new Vector3(0, Random.value  * 360, 0);
	}
}
