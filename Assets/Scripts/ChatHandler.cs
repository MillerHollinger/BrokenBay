using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles all chatting functions.

public class ChatHandler : MonoBehaviour {
	// The chat bubbles.
	public Sprite chatBubble;
	public Sprite radioBubble;

	// The text display box.
	public GameObject textBox;

	// The text display, with the Text component.
	public GameObject textActual;

	// Who is speaking.
	public GameObject speaker;

	// When ready to advance to the next text option.
	private bool next;
	private bool speaking;

	// The test messages
	private string[] messages = {"Hi!", "This is the test conversation.", "Now, you can hear me talk...", "You probably also noticed there's more enemies.", "Cool stuff, yeah?", "Bye."};
	private int messageVal = 0;

	// Use this for initialization
	void Start () {
		setChatVisibility(false);
		next = false;
		speaking = false;
	}

	// Starts the test conversation.
	// TODO: Read converstaions from files using an ID.
	public void Update()
	{
		if (speaking)
		{
			if (next)
			{
				next = false;
				messageVal++;
			}
			if (messageVal != 6)
			{
				say(messages[messageVal]);
			}
			else
			{
				setChatVisibility(false);
			}
		}

	}

	public void testConvo()
	{
		setChatVisibility(true);
		speaking = true;


		//setChatVisibility(false);
	}

	public IEnumerator waitFor(float x)
	{
		yield return new WaitForSeconds(x);
	}

	// Sets next to true, advancing the converstaion.
	public void nextLine()
	{
		next = true;
	}

	// Shows or hides all chat blocks.
	private void setChatVisibility(bool visi)
	{
		textBox.GetComponent<Image>().enabled = visi;
		textActual.GetComponent<Text>().enabled = visi;
		speaker.GetComponent<Image>().enabled = visi;
	}

	// Sets the chat box to something.
	private void say(string what)
	{
		textActual.GetComponent<Text>().text = what;
	}
}
