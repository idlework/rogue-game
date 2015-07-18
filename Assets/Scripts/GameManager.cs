using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public static readonly GameManager instance = new GameManager();

	private BoardManager boardManager;
	private int level = 3;

	//Awake is always called before any Start functions
	void Awake() {
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		
		//Get a component reference to the attached BoardManager script
		boardManager = GetComponent<BoardManager>();
		
		//Call the InitGame function to initialize the first level 
		InitGame();
	}
	
	//Initializes the game for each level.
	void InitGame() {
		//Call the SetupScene function of the BoardManager script, pass it current level number.
		boardManager.SetupScene(level);
	}
}
