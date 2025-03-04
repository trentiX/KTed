using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RythmGame : IPlayable, IDataPersistence
{
	// Serialization

	// CanvasGroups
	[Header("CanvasGroups")]
	[SerializeField] private CanvasGroup[] gameObjects;	
	[SerializeField] private CanvasGroup[] menuObjects;
	[SerializeField] private CanvasGroup[] afterGameMenuObjects;
	[SerializeField] private CanvasGroup[] songs;

	// Gameobjects
	[Header("Gameobjects")]
	[SerializeField] private GameObject arrow;
	[SerializeField] private GameObject hitFlash;
	[SerializeField] private GameObject[] spawnPoints;
	[SerializeField] private GameObject[] killPoints;
	[SerializeField] private GameObject[] hearts;
	[SerializeField] private GameObject scoreText;
	[SerializeField] private GameObject comboText;
	[SerializeField] private GameObject timeText;

	// Texts
	[Header("Texts")]
	[SerializeField] private TextMeshProUGUI yourCombo;
	[SerializeField] private TextMeshProUGUI yourScore;
	[SerializeField] private TextMeshProUGUI winOrLoseText;
	[SerializeField] private TextMeshProUGUI maxCombo;
	[SerializeField] private TextMeshProUGUI maxScore;
	[SerializeField] private TextMeshProUGUI pregameDelayText;
	
	// Other
	[Header("Other")]
	[SerializeField] private Toggle easyModeToggle;
	
	/*
	0 = left
	1 = down
	2 = up
	3 = right
	*/
	
	
	// Variables
	public static RythmGame instance {get; private set;}
	private List<GameObject> arrows = new List<GameObject>();
	private ChooseSong chooseSong;
	private bool gameIsGoing;
	private int combo;
	private int maxMatchCombo;
	private int score;
	private int health;
	private int ComboRecord;
	private int ScoreRecord;
	private AudioSource lastAudioSource;

    // Code

    void Awake()
    {
		if (instance == null)
		{
			instance = this;   
		}
        chooseSong = GetComponent<ChooseSong>();
    }

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

	private void Win(int reward)
	{
		gameIsGoing = false;
		Ktedwork.instance.AccBalanceUIUpdate(Ktedwork.instance._accBalanceInt + reward);
		yourCombo.text = combo.ToString() + "X";
		yourScore.text = score.ToString();
		winOrLoseText.text = "Вы выйграли!";

		SetNewRecord();
	    CanvasFade(afterGameMenuObjects, 1, 0.4f);
		CanvasFade(gameObjects, 0, 0.4f);
	}

	private void Lose()
	{
		gameIsGoing = false;
		lastAudioSource.Stop();
		yourCombo.text = maxMatchCombo.ToString() + "X";
		yourScore.text = score.ToString();
		winOrLoseText.text = "Вы проиграли(";

		ClearAllArrows();
	    EndGame();
	}

	private void SetNewRecord()
	{
		if (combo > ComboRecord)
		{
			ComboRecord = combo;
			yourCombo.text = combo.ToString() + "X"+ " (Новый рекорд!)";
		}

		if (score > ScoreRecord)
		{
			ScoreRecord = score;
			yourScore.text = score.ToString() + " (Новый рекорд!)";
		}
	}

	public override void StartGame()
	{
		CanvasFade(menuObjects, 1, 0.4f);
	}
	
	public void ChooseSong()
	{
		CanvasFade(songs, 1, 0.4f);
		
		CanvasFade(menuObjects, 0, 0.4f);
	}

	public void LaunchGameLoop(AudioSource audioSource)
	{
		CanvasFade(gameObjects, 1, 0.4f);
		
		CanvasFade(songs, 0, 0.4f);

		StartCoroutine(GameLoop(audioSource));
	}

	public void StartAgain()
	{
	    CanvasFade(afterGameMenuObjects, 0, 0.4f);

	    LaunchGameLoop(lastAudioSource);
	}

	private IEnumerator GameLoop(AudioSource audioSource)
	{
		gameIsGoing = true;
		lastAudioSource = audioSource;
		health = 3;
		score = 0;
		combo = 0;
		maxMatchCombo = 0;
		UpdateUI();

		audioSource.PlayOneShot(audioSource.clip);
		StartCoroutine(PregameDelay());

		float totalTime = audioSource.clip.length;
		float timeLeft = totalTime;
		float startTime = Time.time;

		while (timeLeft > 0)
		{
			if (health == 0)
	    	{
				Lose();
				yield break;
			}
			timeLeft = totalTime - (Time.time - startTime);
			timeText.GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(timeLeft).ToString();

		    yield return null; // Добавлено, чтобы не зависать
		} 

		yield return new WaitForSeconds(1f); // Даем немного времени перед очисткой
		ClearAllArrows();
		
		Debug.Log("player won" + audioSource.gameObject.GetComponent<SongForRhythmGame>().bounty);
		Win(audioSource.gameObject.GetComponent<SongForRhythmGame>().bounty);
	}

	private IEnumerator PregameDelay()
	{
		for (int i = 3; i > 0; i--)
		{
			pregameDelayText.text = i.ToString();
			pregameDelayText.DOFade(1, 0.2f);

			yield return new WaitForSeconds(0.8f); // Ждём 0.9 сек, прежде чем начать исчезновение

			pregameDelayText.DOFade(0, 0.2f);
			yield return new WaitForSeconds(0.1f); // Оставшееся время перед следующим числом
		}

		// Показываем "GO!" перед началом игры
		pregameDelayText.text = "GO!";
		pregameDelayText.DOFade(1, 0.2f);
		yield return new WaitForSeconds(0.5f); // Короткая пауза на "GO!"
		
		pregameDelayText.DOFade(0, 0.3f);
	}


	private void ClearAllArrows()
	{
		//AudioManager.Instance.SFXExplosionSound();
		foreach (GameObject arrow in arrows)
		{
			if (arrow != null)
			{
				arrow.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
				{
					Destroy(arrow);
				});
			}
		}
		arrows.Clear(); // Очищаем список стрелок
	}
	
	public void CloseMinigame()
	{
	    KTedpet.instance.CloseMinigame();
		CanvasFade(menuObjects, 0, 0.4f);
	}

	public void BackToMenu()
	{
	    CanvasFade(afterGameMenuObjects, 0, 0.4f);
		CanvasFade(menuObjects, 1, 0.4f);
	}

	public override void EndGame()
	{
		CanvasFade(gameObjects, 0, 0.4f);
		CanvasFade(afterGameMenuObjects, 1, 0.4f);
	}
	
	public IEnumerator SpawnArrow(int delayTime)
	{
		if (!gameIsGoing) yield break;

		int i = UnityEngine.Random.Range(0, spawnPoints.Length);
		yield return new WaitForSeconds(delayTime);

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
		float distance = Mathf.Abs(arrows[0].GetComponent<Arrow>().killPos.transform.position.y - arrows[0].transform.position.y);

		return distance <= 90f; // Теперь учитывается только диапазон ±70f
	}

	
	private void DestroyArrow()
	{
		if (arrows.Count == 0) return;

		GameObject arrowToDestroy = arrows[0];

		if (arrowToDestroy.transform.position.y >= arrowToDestroy.GetComponent<Arrow>().killPos.transform.position.y + 60f)
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

		//AudioManager.Instance.SFXQuestBitCompletion();

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

		if (combo > maxMatchCombo)
		{
			maxMatchCombo = combo;
		}
		combo = 0;
		
		//AudioManager.Instance.SFXFailedSound();
		
		ClearAllArrows();
		
		if (!easyModeToggle.isOn) health--;

		FlashEffect(true);
		UpdateUI();
	}

	
	private void UpdateUI()
	{
		comboText.GetComponentInChildren<TextMeshProUGUI>(0).text = combo.ToString() + "X";
		scoreText.GetComponentInChildren<TextMeshProUGUI>(0).text = score.ToString();

		if (easyModeToggle.isOn) return;

		foreach (var heart in hearts)
		{
			heart.SetActive(false); // Сначала скрываем все сердца
		}

		for (int i = 0; i < health; i++)
		{
			hearts[i].SetActive(true);
		}
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
			hitFlash.GetComponent<Image>().color = Color.white;
		}
		
		hitFlash.GetComponent<CanvasGroup>().DOFade(0.1f, 0.1f) // Быстро увеличиваем прозрачность до 50%
			.OnComplete(() => hitFlash.GetComponent<CanvasGroup>().DOFade(0, 0.1f)); // Затем плавно исчезаем
	}

	private void TextRainbowAnimation()
	{
		float hue = Mathf.Repeat(Time.time * 0.2f, 1f); // 0.2f - скорость изменения цвета
		Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f); // Полностью насыщенный и яркий цвет

/* 		foreach (var text in scoreText.GetComponentsInChildren<TextMeshProUGUI>())
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
		foreach (var rect in bitRects)
		{
			rect.GetComponent<RawImage>().color = rainbowColor;
		}*/
	}

	private void CanvasFade(CanvasGroup[] canvasGroups, int value, float time)
	{
	    foreach (CanvasGroup obj in canvasGroups)
		{
			obj.DOFade(value, time);

			if (value == 0)
			{
				obj.blocksRaycasts = false;
				obj.interactable = false;
			}
			else
			{
				obj.blocksRaycasts = true;
				obj.interactable = true;
			}
		}
	}

	// DATA
	public void LoadData(GameData gameData)
	{
		ComboRecord = gameData.MaxCombo;
		ScoreRecord = gameData.MaxScore;

		maxCombo.text = ComboRecord.ToString() + "X";
		maxScore.text = ScoreRecord.ToString();
	}

	public void SaveData(ref GameData gameData)
	{
		if (gameData.MaxCombo < combo)
		{
			gameData.MaxCombo = ComboRecord;
		}
		if (gameData.MaxScore < score)
		{
			gameData.MaxScore = ScoreRecord;
		}
	}
}
