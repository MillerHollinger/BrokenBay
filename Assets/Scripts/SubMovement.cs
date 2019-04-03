using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Player movement. Allows right-click to dive.

// TODO: Destroying the player should be moved elsewhere.

public class SubMovement : MonoBehaviour {
public float speed; // How fast the ship accelerates.
public float maxSpeed; // How fast the ship can go.

public GameObject textDisp; // The "diving" text disp.

private Vector3 lastRotation = Vector3.zero; // Which way the ship was last rotated. Used for movement.

public ParticleSystem explosion; // The explosion.
private bool exploding = false; // if the ship is about to explode.
public List<GameObject> parts; // the parts of the ship. All are exploded.

	// Update is called once per frame
	void Update () {
		if (!exploding)
		{
			float y = Input.GetAxis("Vertical") * Time.deltaTime * speed;
			float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
			// Stop diagonals from being faster
			if (x != 0 && y != 0)
			{
				x /= Mathf.Sqrt(2);
				y /= Mathf.Sqrt(2);
			}
			Vector3 movement = new Vector3(x,0,y);
			if (movement != lastRotation && movement != Vector3.zero)
				transform.rotation = Quaternion.LookRotation(movement);
			GetComponent<Rigidbody>().AddForce(movement);


			if(GetComponent<Rigidbody>().velocity.magnitude > maxSpeed)
	    {
	    	GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxSpeed;
	  	}

			if (Input.GetButton("Fire2"))
			{
				if (GetComponent<Transform>().position.y < -1)
					GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, -1, GetComponent<Transform>().position.z);
				else
					GetComponent<Rigidbody>().AddForce(new Vector3(0,-15 * Time.deltaTime,0));

				textDisp = GameObject.Find("DepthDisplay");
				textDisp.GetComponent<Text>().text = "Status: Diving";
				textDisp.GetComponent<Text>().color = Color.white;
			}
			else
			{
				if (GetComponent<Transform>().position.y > 0)
					GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 0, GetComponent<Transform>().position.z);
				else
					GetComponent<Rigidbody>().AddForce(new Vector3(0,15 * Time.deltaTime,0));
				textDisp = GameObject.Find("DepthDisplay");
				textDisp.GetComponent<Text>().text = "Status: Surfacing";
				textDisp.GetComponent<Text>().color = Color.black;
			}
		}
	}

	public void Detonate() {
		exploding = true;
		explosion.Play();
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].GetComponent<MeshRenderer>().enabled = false;
			Destroy(parts[i], explosion.duration);
		}
	}
}
