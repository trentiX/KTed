using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTreatsGame : Games
{
	public override void PlayGame()
	{
		base.PlayGame();
		pet.AddComponent<Rigidbody2D>();
		pet.AddComponent<BoxCollider2D>();
		
		Rigidbody2D rb = pet.GetComponent<Rigidbody2D>();
		rb.gravityScale = 3f;
		
		pet.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
	}
	
	private void Update()
	{
		//Move
	}
}
