using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
	// Using Serializable allows us to embed a class with sub properties in the inspector.
	[Serializable]
	public class Count {
		public int minimum;
		public int maximum;

		public Count(int min, int max) {
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count(5, 9);
	public Count foodCount = new Count(1, 5);
	public GameObject exit;
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] foodTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;

	private int level;
	private Transform board;
	private List<Vector3> gameItemsGrid = new List<Vector3>();
	
	//SetupScene initializes our level and calls the previous functions to lay out the game board
	public void SetupScene(int level) {
		this.level = level;

		//Creates the outer walls and floor.
		BoardSetup();
		
		//Reset our list of gameItemsGridpositions.
		GameItemsSetup();
	}

	void BoardSetup() {
		board = new GameObject("Board").transform;

		TilesSetup();
		OuterWallSetup();
	}

	void TilesSetup() {
		GameObject toInstantiate;
		
		for(int x = 0; x < columns; x++) {
			for(int y = 0; y < rows; y++) {
				toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
				AddGameObject(toInstantiate, new Vector3 (x, y, 0f));
			}
		}
	}

	void OuterWallSetup() {
		GameObject toInstantiate;
		
		for(int x = -1; x < columns + 1; x++) {
			for(int y = -1; y < rows + 1; y++) {
				//Check if we current position is at board edge
				if(x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					AddGameObject(toInstantiate, new Vector3 (x, y, 0f));
				}
			}
		}
	}

	void GameItemsSetup() {
		GameItemsGridSetup();

		//Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
		LayoutGameItemAtRandomPosition(wallTiles, wallCount.minimum, wallCount.maximum);
		
		//Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
		LayoutGameItemAtRandomPosition(foodTiles, foodCount.minimum, foodCount.maximum);
		
		//Determine number of enemies based on current level number, based on a logarithmic progression
		int enemyCount = (int)Mathf.Log(level, 2f);
		
		//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
		LayoutGameItemAtRandomPosition(enemyTiles, enemyCount, enemyCount);
		
		//Instantiate the exit tile in the upper right hand corner of our game board
		Instantiate(exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}

	void GameItemsGridSetup() {
		gameItemsGrid.Clear();
		
		for(int x = 1; x < columns - 1; x++) {
			for(int y = 1; y < rows - 1; y++) {
				gameItemsGrid.Add (new Vector3(x, y, 0f));
			}
		}
	}
	
	//GetRandomGameItemsGridPosition returns a random position from our list gameItemsGrid.
	Vector3 GetRandomGameItemsGridPosition() {
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gameItemsGrid.
		int randomIndex = Random.Range(0, gameItemsGrid.Count);

		//Return the randomly selected Vector3 position.
		return GetGameItemsGridPosition(randomIndex);
	}
	
	Vector3 GetGameItemsGridPosition(int index) {
		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gameItemsGrid.
		Vector3 position = gameItemsGrid[index];
		
		//Remove the entry at randomIndex from the list so that it can't be re-used.
		gameItemsGrid.RemoveAt(index);
		
		//Return the randomly selected Vector3 position.
		return position;
	}

	//LayoutGameItemAtRandomgamePosition accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
	void LayoutGameItemAtRandomPosition(GameObject[] tileArray, int minimum, int maximum)
	{
		//Choose a random number of objects to instantiate within the minimum and maximum limits
		int objectCount = Random.Range(minimum, maximum + 1);
		
		//Instantiate objects until the randomly chosen limit objectCount is reached
		for(int i = 0; i < objectCount; i++) {
			//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gameItemsGridPosition
			Vector3 randomPosition = GetRandomGameItemsGridPosition();
			
			//Choose a random tile from tileArray and assign it to tileChoice
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			
			//Instantiate tileChoice at the position returned by GetRandomGameItemsGridPosition with no change in rotation
			AddGameObject(tileChoice, randomPosition);
		}
	}

	void LayoutGameItemAtPosition(GameObject[] tileArray, int minimum, int maximum)
	{
		//Choose a random number of objects to instantiate within the minimum and maximum limits
		int objectCount = Random.Range(minimum, maximum + 1);
		
		//Instantiate objects until the randomly chosen limit objectCount is reached
		for(int i = 0; i < objectCount; i++) {
			//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gameItemsGridPosition
			Vector3 randomPosition = GetRandomGameItemsGridPosition();
			
			//Choose a random tile from tileArray and assign it to tileChoice
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			
			//Instantiate tileChoice at the position returned by GetRandomGameItemsGridPosition with no change in rotation
			AddGameObject(tileChoice, randomPosition);
		}
	}

	void AddGameObject(GameObject toInstantiate, Vector3 position) {
		GameObject instance = Instantiate(toInstantiate, position, Quaternion.identity) as GameObject;
		instance.transform.SetParent(board);
	}
}
