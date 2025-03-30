using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TopDownMinigame : IPlayable, IDataPersistence
{   
    // Serialization
    
    // CanvasGroups
	[Header("CanvasGroups")]
    [SerializeField] private CanvasGroup[] gameObjects;	
	[SerializeField] private CanvasGroup[] menuObjects;
	[SerializeField] private CanvasGroup[] afterGameMenuObjects;
    
    // Variables
    
    // Gameobjects
	[Header("Gameobjects")]
	[SerializeField] private GameObject EnemyMeleePrefab;
	[SerializeField] private GameObject EnemyRangedPrefab;
    [SerializeField] private GameObject PlayerPrefab;
    
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
    private IEnumerator GameLoop()
    {
        
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
