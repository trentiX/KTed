using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RythmGame : IPlayable
{
	// Serialization
	[SerializeField] private CanvasGroup[] gameObjects;	
	[SerializeField] private CanvasGroup[] MenuObjects;
	[SerializeField] private GameObject arrow;
	[SerializeField] private GameObject hitFlash;
	[SerializeField] private GameObject[] spawnPoints;
	[SerializeField] private GameObject[] killPoints;
	[SerializeField] private GameObject scoreText;
	[SerializeField] private GameObject comboText;
	[SerializeField] private GameObject timeText;
	
	/*
	0 = left
	1 = down
	2 = up
	3 = right
	*/
	
	
	// Variables
	private List<GameObject> arrows = new List<GameObject>();
	private bool lastArrowKilled = false;
	private float spawnDelay = 2;
	private int combo;
	private int score;
	
	// Code

	private void Update()
	{
		if (!KTedpet.instance.gameMode) return;

		if (Input.GetKeyDown(KeyCode.LeftArrow))TryKillArrow(0);
		if (Input.GetKeyDown(KeyCode.DownArrow))TryKillArrow(1);
		if (Input.GetKeyDown(KeyCode.UpArrow))TryKillArrow(2);
		if (Input.GetKeyDown(KeyCode.RightArrow))TryKillArrow(3);
		
		DestroyArrow();
		
		TextRainbowAnimation();
	}
	public override void StartGame()
	{
		foreach (CanvasGroup obj in MenuObjects)
		{
			obj.alpha = 1;
		}
	}
	
	private void LaunchGameLoop()
	{
		foreach (CanvasGroup obj in gameObjects)
		{
			obj.alpha = 1;
		}	
		
		foreach (CanvasGroup obj in MenuObjects)
		{
			obj.alpha = 0;
			obj.blocksRaycasts = false;
			obj.interactable = false;
		}

		StartCoroutine(GameLoop());
	}

	private IEnumerator GameLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnDelay);
			SpawnArrow();

			if (spawnDelay > 0.6f)
			{
			 	spawnDelay = spawnDelay - 0.05f;
				Debug.Log(spawnDelay);   
			}
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
		newArrow.GetComponent<Arrow>().killPos = killPoints[i];
		
		arrows.Add(newArrow);
		
		switch(i)
		{
			case 0:
				newArrow.transform.Rotate(0, 0, 180);
				break;
			case 1:
				newArrow.transform.Rotate(0, 0, 270);
				break;
			case 2:
				newArrow.transform.Rotate(0, 0, 90);
				break;
			case 3:
				break;
			default:
				break;
		}
	}
	
	private void TryKillArrow(int arrowDirection)
	{
		if (arrows.Count == 0) return;

		if (arrows[0].GetComponent<Arrow>().direction == arrowDirection && CheckArrowPosition())
		{
			SuccessfullyKillArrow();
		}
		else
		{
			UnsuccessfullyKillArrow();
		}
	}
	
	private bool CheckArrowPosition()
	{
		if (arrows[0].GetComponent<Arrow>().killPos.transform.position.y - arrows[0].transform.position.y <= 70f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	private void DestroyArrow()
	{
		if (arrows.Count == 0) return;

		GameObject arrowToDestroy = arrows[0];

		if (arrowToDestroy.transform.position.y >= arrowToDestroy.GetComponent<Arrow>().killPos.transform.position.y + 80f)
		{
			StartCoroutine(DestroyWithDelay(arrowToDestroy));
		}
	}

	private IEnumerator DestroyWithDelay(GameObject arrowToDestroy)
	{
		yield return new WaitForSeconds(0.2f); // Задержка перед удалением
		if (arrows.Count > 0 && arrows[0] == arrowToDestroy) 
		{
			UnsuccessfullyKillArrow();
		}
	}

	
	private void SuccessfullyKillArrow()
	{
		if (arrows.Count == 0) return;

		Debug.Log("Killed");

		GameObject arrowToRemove = arrows[0];
		arrows.RemoveAt(0); // Удаляем из списка сразу, но объект пока остается

		float distance = Mathf.Abs(arrowToRemove.GetComponent<Arrow>().killPos.transform.position.y - arrowToRemove.transform.position.y);

		arrowToRemove.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			Destroy(arrowToRemove);
		});

		combo++;

		// Базовое количество очков за попадание
		int baseScore = 10 * combo;

		// Дополнительные бонусы за точность
		int accuracyBonus = 0;
		
		if (distance <= 5f) // Идеальное попадание
		{
			accuracyBonus = 50;
			PerferctShot();
		}
		else if (distance <= 15f) // Хорошее попадание
		{
			accuracyBonus = 30;
		}
		else if (distance <= 30f) // Среднее попадание
		{
			accuracyBonus = 10;
		}

		score += baseScore + accuracyBonus;

		AudioManager.Instance.SFXQuestBitCompletion();

		FlashEffect(false);
		UpdateUI();
	}

	
	private void UnsuccessfullyKillArrow()
	{
		Debug.Log("Missed");

		if (arrows.Count == 0) return;

		GameObject arrowToRemove = arrows[0];
		arrows.RemoveAt(0); // Удаляем из списка сразу, но объект пока остается

		arrowToRemove.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
		{
			Destroy(arrowToRemove);
		});

		combo = 0;
		
		AudioManager.Instance.SFXFailedSound();
		
		FlashEffect(true);
		UpdateUI();
	}

	
	private void UpdateUI()
	{
		comboText.GetComponentInChildren<TextMeshProUGUI>(0).text = combo.ToString() + "X";
		scoreText.GetComponentInChildren<TextMeshProUGUI>(0).text = score.ToString();
	}
	
	private void PerferctShot()
	{
	

	}
	
	private void ComboPopUpAnimation()
	{
		
	}
	
	private void FlashEffect(bool miss)
	{
		if (miss)
		{
			hitFlash.GetComponent<Image>().color = Color.red;
		}
		else
		{
			hitFlash.GetComponent<Image>().color = Color.green;
		}
		
		hitFlash.GetComponent<CanvasGroup>().DOFade(0.1f, 0.1f) // Быстро увеличиваем прозрачность до 50%
			.OnComplete(() => hitFlash.GetComponent<CanvasGroup>().DOFade(0, 0.1f)); // Затем плавно исчезаем
	}

	private void TextRainbowAnimation()
	{
		float hue = Mathf.Repeat(Time.time * 0.2f, 1f); // 0.2f - скорость изменения цвета
		Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f); // Полностью насыщенный и яркий цвет

		foreach (var text in scoreText.GetComponentsInChildren<TextMeshProUGUI>())
		{
			text.color = rainbowColor;
		}
		foreach (var text in comboText.GetComponentsInChildren<TextMeshProUGUI>())
		{
			text.color = rainbowColor;
		}
		foreach (var text in timeText.GetComponentsInChildren<TextMeshProUGUI>())
		{
			text.color = rainbowColor;
		}
	}
}
