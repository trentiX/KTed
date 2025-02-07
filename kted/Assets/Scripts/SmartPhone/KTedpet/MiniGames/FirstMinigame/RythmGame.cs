using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmGame : IPlayable
{
	// Serialization
	[SerializeField] private CanvasGroup[] gameObjects;	
	[SerializeField] private GameObject arrow;
	[SerializeField] private GameObject[] spawnPoints;
	[SerializeField] private Collider2D[] hitbox;
	/*
	0 = left
	1 = down
	2 = up
	3 = right
	*/
	
	
	// Variables
	private GameObject currArrow;
	private bool lastArrowKilled = false;
	
	private int spawnDelay = 4;
		
	// Code

	private void Update()
	{
		if (!KTedpet.instance.gameMode) return;

		if (Input.GetKey(KeyCode.LeftArrow))KillArrow(KeyCode.LeftArrow);
		if (Input.GetKey(KeyCode.DownArrow))KillArrow(KeyCode.DownArrow);
		if (Input.GetKey(KeyCode.UpArrow))KillArrow(KeyCode.UpArrow);
		if (Input.GetKey(KeyCode.RightArrow))KillArrow(KeyCode.RightArrow);
	}
	public override void StartGame()
	{
		foreach (CanvasGroup obj in gameObjects)
		{
			obj.alpha = 1;
		}	
		
		StartCoroutine(GameLoop());
	}
	
	private IEnumerator GameLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnDelay);
			SpawnArrow();
		}
	}
	
	public override void EndGame()
	{
		
	}
	
	private void SpawnArrow()
	{
		int i = Random.Range(0, spawnPoints.Length);
		GameObject newArrow = Instantiate(arrow, spawnPoints[i].transform.position, Quaternion.identity, gameObjects[0].transform);
		
		if (lastArrowKilled)
		{
			currArrow = newArrow;	
		}	
	}
	
	private void KillArrow(KeyCode keyCode)
	{
		
	}
}
