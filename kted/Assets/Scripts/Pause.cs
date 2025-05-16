using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UIElements;

public class Pause : MonoBehaviour
{
	[SerializeField] private GameObject panel;
	[SerializeField] private CanvasGroup prefab;

	public static bool isOpen = false;
	private DataPersistenceManager dataPersistenceManager;
	
	private void Awake()
	{
		dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
	}
	public void GoToHome()
	{
		//Cursor.visible = false;
		isOpen = false;
		Time.timeScale = 1f;
		dataPersistenceManager.SaveGame();
		
		prefab.DOFade(1, 2).SetEase(Ease.OutCubic).OnComplete(() =>
		{
			SceneManager.LoadScene(0);
		}); 
	}
	
	public void Resume(UnityEngine.UI.Button button)
	{
		//Cursor.visible = false;
		isOpen = false;
		panel.SetActive(false);
		Time.timeScale = 1f;
		
		button.interactable = false;
		button.interactable = true;
	}
}
