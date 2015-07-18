using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
	public Sprite dmgSprite;
	public int hp = 3;

	private SpriteRenderer spriteRenderer;
	
	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void DamageWall(int loss) {
		//Set spriteRenderer to the damaged wall sprite.
		spriteRenderer.sprite = dmgSprite;
		
		//Subtract loss from hit point total.
		hp -= loss;
		
		//If hit points are less than or equal to zero:
		if(hp <= 0) {
			//Disable the gameObject.
			gameObject.SetActive (false);
		}
	}
}
