using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static readonly GameManager instance = new GameManager();

	[HideInInspector] public bool playersTurn = true;

	public int playerFoodPoints = 100;

	private BoardManager boardManager;
	private int level = 10;
	
	void Awake() {
		boardManager = GetComponent<BoardManager>();

		InitGame();
	}

	void InitGame() {
		boardManager.SetupScene(level);
	}
	
	public void GameOver() {
		enabled = false;
	}
}
