using UnityEngine;
using System.Collections;

public class Player : MovingObject {
	public float restartLevelDelay = 1f;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public int wallDamage = 1;

	private Animator animator;
	private int food;

	protected override void Start() {
		animator = GetComponent<Animator>();
		food = GameManager.instance.playerFoodPoints;

		base.Start ();
	}

	private void OnDisable() {
		GameManager.instance.playerFoodPoints = food;
	}

	private void Update() {
		//If it's not the player's turn, exit the function.
		if(!GameManager.instance.playersTurn) return;
		
		int horizontal = (int)(Input.GetAxisRaw("Horizontal"));
		int vertical = (int)(Input.GetAxisRaw("Vertical"));

		//To avoid going diagonal reset vertical.
		if(horizontal != 0) {
			vertical = 0;
		}
		
		//Check if we have a non-zero value for horizontal or vertical
		if(horizontal != 0 || vertical != 0) {
			AttemptMove<Wall>(horizontal, vertical);
		}
	}

	protected override void AttemptMove<T>(int xDir, int yDir) {
		food--;

		base.AttemptMove<T>(xDir, yDir);
		
		//Since the player has moved and lost food points, check if the game has ended.
		CheckIfGameOver();
		
		//Set the playersTurn boolean of GameManager to false now that players turn is over.
		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove<T>(T component) {
		//Set hitWall to equal the component passed in as a parameter.
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);

		animator.SetTrigger ("playerChop");
	}

	private void OnTriggerEnter2D(Collider2D other) {
		//Check if the tag of the trigger collided with is Exit.
		if(other.tag == "Exit") {
			//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
			Invoke ("Restart", restartLevelDelay);
			
			//Disable the player object since level is over.
			enabled = false;
		}
		
		//Check if the tag of the trigger collided with is Food.
		else if(other.tag == "Food") {
			//Add pointsPerFood to the players current food total.
			food += pointsPerFood;
			
			//Disable the food object the player collided with.
			other.gameObject.SetActive (false);
		}
		
		//Check if the tag of the trigger collided with is Soda.
		else if(other.tag == "Soda") {
			//Add pointsPerSoda to players food points total
			food += pointsPerSoda;

			//Disable the soda object the player collided with.
			other.gameObject.SetActive (false);
		}
	}

	private void Restart() {
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void LoseFood(int loss) {
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger("playerHit");

		food -= loss;

		CheckIfGameOver();
	}

	private void CheckIfGameOver() {
		if (food <= 0)  {
			GameManager.instance.GameOver();
		}
	}
}