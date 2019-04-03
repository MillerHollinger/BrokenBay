using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownTester : MonoBehaviour {

	// The player's location in row-column form.
	int pRow = 0;
	int pCol = 0;

	public List<GameObject> buttonsRaw;

	public Sprite enemy; // TODO: Support multiple enemy types
	public Sprite house; // TODO: Support multiple building types
	public Sprite player;
	public Sprite fight;
	public Sprite goal;
	public Sprite hidden;

	// The town. For this test, it will be randomly generated each run.
	Building[,] town = new Building[5,5];

	// The buttons.
	GameObject[,] buttons = new GameObject[5,5];

	// The chat handler.
	public GameObject chatHandler;

	// Use this for initialization
	void Start () {

		Debug.Log(town.Length);
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				GameObject first = buttonsRaw[0];
				//Debug.Log( i + " " + j);
				buttons[i,j] = first;
				buttonsRaw.RemoveAt(0);
			}

		}

		generateTown();
		buttons[pRow,pCol].GetComponent<Image>().sprite = player;
		town[2,2].addEnemy(new Enemy("Grunt"));
		town[3,3].addEnemy(new Enemy("Grunt"));
		town[0,0].discover();
		updateMap();
	}

	// Moves all enemies
	public void moveEnemies()
	{
		// Move all enemies
		// TODO Support multiple enemy types.
		for (int i = 0; i < 5; i++){
			for (int j = 0; j < 5; j++){
				if (town[i,j].enemiesHere.Count > 0){
					for (int townEnemy = 0; townEnemy < town[i,j].enemiesHere.Count; townEnemy++){
						if (!town[i,j].enemiesHere[townEnemy].movedRecently)	{
							int direction = Random.Range(0,4);
							town[i,j].enemiesHere[townEnemy].movedRecently = true;
							// 0 = up
							// 1 = left
							// 2 = right
							// 3 = down
							switch (direction){
								case 0:
									if (i-1 >= 0){
										town[i-1,j].addEnemy(town[i,j].enemiesHere[townEnemy]);
										town[i,j].enemiesHere.RemoveAt(townEnemy);
										townEnemy--;
									}
									break;
								case 1:
									if (j-1 >= 0){
										town[i,j-1].addEnemy(town[i,j].enemiesHere[townEnemy]);
										town[i,j].enemiesHere.RemoveAt(townEnemy);
										townEnemy--;
									}
									break;
								case 2:
									if (j+1 <= 4){
										town[i,j+1].addEnemy(town[i,j].enemiesHere[townEnemy]);
										town[i,j].enemiesHere.RemoveAt(townEnemy);
										townEnemy--;
									}
									break;
								case 3:
									if (i+1 <= 4){
										town[i+1,j].addEnemy(town[i,j].enemiesHere[townEnemy]);
										town[i,j].enemiesHere.RemoveAt(townEnemy);
										townEnemy--;
									}
									break;
							}
						}
					}
				}
			}
		}

		// Reset all enemies so they can move next round.
		for (int i = 0; i < 5; i++){
			for (int j = 0; j < 5; j++){
				if (town[i,j].enemiesHere.Count > 0){
					foreach (Enemy e in town[i,j].enemiesHere){
						e.movedRecently = false;
					}
				}
			}
		}
	}

	// Updates the map with most recent info
	public void updateMap()
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				Debug.Log("Mapping " + i + "  " + j + " : D E " +  town[i,j].discovered + " " + town[i,j].enemiesHere.Count);
				if (town[i,j].discovered)
				{
					if (i == pRow && j == pCol)
					{
						if (town[i,j].enemiesHere.Count > 0) // If explored, player, enemy
						{
							buttons[i,j].GetComponent<Image>().sprite = fight;
						}
						else	// If explored, player, no enemy
						{
							buttons[i,j].GetComponent<Image>().sprite = player;
							if (town[i,j].disp == "Goal")
							{
								Debug.Log("trying to start convo");
								chatHandler.GetComponent<ChatHandler>().testConvo();
							}
						}
					}
					else
					{
						if (town[i,j].enemiesHere.Count > 0) // If explored, no player, enemy
						{
							buttons[i,j].GetComponent<Image>().sprite = enemy;

						}
						else// If explored, no player, no enemy: disp
						{
							if (town[i,j].disp == "Goal")
								buttons[i,j].GetComponent<Image>().sprite = goal;
							else
								buttons[i,j].GetComponent<Image>().sprite = house;
						}
					}
				}
				else
				{
					if (town[i,j].enemiesHere.Count > 0) // If not explored, enemy: X
					{
						buttons[i,j].GetComponent<Image>().sprite = enemy;
					}
					else // If not explored, no enemy: ?
					{
						buttons[i,j].GetComponent<Image>().sprite = hidden;
					}
				}
			}
		}
	}

	// Ran by the town buttons. Attempts to move the player to the selected location.
	// TODO Investigate if it's possible to use more than one variable in a button onPress() function.
	public void attemptMove(int rowCol)
	{
		int row = rowCol / 10; // tens
		int col = rowCol % 10;
		Debug.Log("trying to move to " +row + " " + col);
		// Check that this is one space adjacent movement -- no diagonals alllowed
		if (Mathf.Abs(pRow - row) == 1 && pCol == col || Mathf.Abs(pCol - col) == 1 && pRow == row)
		{
			buttons[pRow,pCol].GetComponent<Image>().sprite = house;
			// Valid move, player relocates
			pRow = row;
			pCol = col;
			town[pRow,pCol].discover();
			// enemies move
			moveEnemies();
			// update map
			updateMap();
		}


	}

	// Generates a random town.
	void generateTown()
	{
		// For this test, each town will have 24 empties and 1 goal

		// Set all buildings to normal houses
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				town[i,j] = new Building("House");
				buttons[i,j].GetComponent<Image>().sprite = hidden;
			}
		}

		// Goal will always be in further half of town
		town[Random.Range(2,5),Random.Range(2,5)] = new Building("Goal");
	}

	public class Enemy
	{
		// If this enemy has moved since all enemies were last moved, to prevent double-moves.
		public bool movedRecently;
		// This enemy's type.
		public string type;

		public Enemy(string t)
		{
			type = t;
			movedRecently = false;
		}
	}

	public class Building
	{
		// How this building is displayed
		public string disp;
		// If the player has been in here yet
		public bool discovered;
		// Any enemies that are here, as strings
	 	public List<Enemy> enemiesHere;

		public Building(string d)
		{
			disp = d;
			discovered = false;
			enemiesHere = new List<Enemy>();
		}
		public Building(string d, bool b)
		{
			disp = d;
			discovered = b;
			enemiesHere = new List<Enemy>();
		}
		public Building(string d, bool b, List<Enemy> e)
		{
			disp = d;
			discovered = b;
			enemiesHere = e;
		}

		// Sets disc to true.
		public void discover()
		{
			discovered = true;
		}

		// Adds an enemy here. TODO: Support more complex enemy types.
		public void addEnemy(Enemy e)
		{
			enemiesHere.Add(e);
		}
	}
}
