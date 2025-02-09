using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RythmGame : IPlayable
{
	// Serialization
	[SerializeField] private CanvasGroup[] gameObjects;	
	[SerializeField] private GameObject arrow;
	[SerializeField] private GameObject[] spawnPoints;
	[SerializeField] private Collider2D[] hitbox;
	[SerializeField] private GameObject scoreText;
	[SerializeField] private GameObject comboText;
	
	/*
	0 = left
	1 = down
	2 = up
	3 = right
	*/
	
	
	// Variables
	private GameObject currArrow;
	private bool lastArrowKilled = false;
	private float spawnDelay = 4;
	private int combo;
	private int score;
	
	private Vector3 scoreTextStartPos;
	private Vector3 comboTextStartPos;
	
	// Code

	private void Start()
	{
		scoreTextStartPos = scoreText.transform.localPosition;
		comboTextStartPos = comboText.transform.localPosition;
	}
	private void Update()
	{
		if (!KTedpet.instance.gameMode) return;

		if (Input.GetKey(KeyCode.LeftArrow))TryKillArrow(0);
		if (Input.GetKey(KeyCode.DownArrow))TryKillArrow(1);
		if (Input.GetKey(KeyCode.UpArrow))TryKillArrow(2);
		if (Input.GetKey(KeyCode.RightArrow))TryKillArrow(3);
		
		TextRainbowSwingAnimation();
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
			spawnDelay = spawnDelay - 0.05f;
		}
	}
	
	public override void EndGame()
	{
		
	}
	
	private void SpawnArrow()
	{
		int i = Random.Range(0, spawnPoints.Length);
		GameObject newArrow = Instantiate(arrow, spawnPoints[i].transform.position, Quaternion.identity, gameObjects[0].transform);
		
		newArrow.GetComponent<Arrow>().direction = i;
		
		if (lastArrowKilled)
		{
			currArrow = newArrow;	
		}	
	}
	
	private void TryKillArrow(int arrowDirection)
	{
		if (currArrow.GetComponent<Arrow>().direction == arrowDirection)
		{
			SuccessfullyKillArrow();
		}
		else
		{
			UnsuccessfullyKillArrow();
		}
	}
	
	private void SuccessfullyKillArrow()
	{
		
	}
	
	private void UnsuccessfullyKillArrow()
	{
		
	}
	
	private void TextRainbowSwingAnimation()
	{
		float hue = Mathf.Repeat(Time.time * 0.2f, 1f); // Меняет оттенок по кругу
		Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f); // Создает насыщенный цвет

		// Применяем цвет ко всем дочерним текстовым объектам
		foreach (var text in scoreText.GetComponentsInChildren<TextMeshProUGUI>())
		{
			text.color = rainbowColor;
		}
		
		foreach (var text in comboText.GetComponentsInChildren<TextMeshProUGUI>())
		{
			text.color = rainbowColor;
		}

		// Создаем качание относительно стартовой позиции
		float swingOffset = Mathf.Sin(Time.time * 2f) * 10f; // 10 - амплитуда качания
		scoreText.transform.localPosition = scoreTextStartPos + new Vector3(swingOffset, 0, 0);
		comboText.transform.localPosition = comboTextStartPos + new Vector3(swingOffset, 0, 0);
	}

}
