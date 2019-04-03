using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownEnter : MonoBehaviour {

// This instance's related UI empty object
	public GameObject myTownUI;

	public GameObject player;

	void Start()
	{
		myTownUI.SetActive(false);
	}

// display this town
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("contact ");

		if (other.attachedRigidbody.gameObject.Equals(player))
			myTownUI.SetActive(true);
	}

// hide this town
	void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody.gameObject.Equals(player))
			myTownUI.SetActive(false);
	}
}
