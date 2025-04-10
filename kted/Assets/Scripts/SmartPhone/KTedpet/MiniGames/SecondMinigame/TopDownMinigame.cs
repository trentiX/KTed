using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopDownMinigame : IPlayable, IDataPersistence
{   
    // Serialization
    
    // CanvasGroups
	[Header("CanvasGroups")]
    [SerializeField] private CanvasGroup[] gameObjects;	
	[SerializeField] private CanvasGroup[] menuObjects;
	[SerializeField] private CanvasGroup[] afterGameMenuObjects;
        
    // Gameobjects
	[Header("Gameobjects")]
	[SerializeField] private GameObject EnemyMeleePrefab;
	[SerializeField] private GameObject EnemyRangedPrefab;
    [SerializeField] private GameObject PlayerPrefab;
	[SerializeField] private GameObject[] hearts;
	[SerializeField] private GameObject inGameScoreText;
	[SerializeField] private GameObject inGameKillsText;
	[SerializeField] private GameObject timeText;
	
	// Texts
	[Header("Texts")]
	[SerializeField] private TextMeshProUGUI inMenuYourTime;
	[SerializeField] private TextMeshProUGUI inMenuYourScore;
	
	
	// Variables
	private bool gameIsGoing;
	private int score;
	private int kills;
	private int health;
	private int TimeRecord;
	private int ScoreRecord;	


    // Code
    public void LaunchGameLoop()
	{
		CanvasFade(gameObjects, 1, 0.4f);
		
		CanvasFade(menuObjects, 0, 0.4f);

		StartCoroutine(GameLoop());
	}

	public void StartAgain()
	{
	    CanvasFade(afterGameMenuObjects, 0, 0.4f);

	    LaunchGameLoop();
	}
	
	private void Win()
	{
	    
	}
	
	private void Lose()
	{
	    
	}
	
    private IEnumerator GameLoop()
    {
    	UpdateUI();

        while (gameIsGoing)
		{
			// Game loop logic here
			yield return null;
		}
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
	
	private void SpawnEnemy()
	{
	    
	}
	
	private void UpdateUI()
	{
	    inGameKillsText.GetComponentInChildren<TextMeshProUGUI>(0).text = kills.ToString();
		inGameScoreText.GetComponentInChildren<TextMeshProUGUI>(0).text = score.ToString();

		foreach (var heart in hearts)
		{
			heart.SetActive(false); // Сначала скрываем все сердца
		}

		for (int i = 0; i < health; i++)
		{
			hearts[i].SetActive(true);
		}
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
    
    // Data
    public void LoadData(GameData data)
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData(ref GameData data)
    {
        //throw new System.NotImplementedException();
    }
}
