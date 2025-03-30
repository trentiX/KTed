using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayRoomManager : MonoBehaviour
{
	// References
	[SerializeField] public List<GameObject> gamesIcons;
	
	// Variables
	[HideInInspector] public GameObject currGame;
	private GameObject leftGame;
	private GameObject rightGame;
	private List<GameObject> availableGames;
	private Tweener gamesAnim;
	private Ktedwork ktedwork;
	private AudioManager audioManager;
 	
	// Code
	private void Start()
	{
		audioManager = FindObjectOfType<AudioManager>();
		ktedwork = FindObjectOfType<Ktedwork>();

		availableGames = gamesIcons;
		
		leftGame = availableGames[availableGames.Count - 1];
		currGame = availableGames[0];
		rightGame = availableGames[1];
		
		foreach (var item in availableGames)
		{
			//item.GetComponent<Games>().buttonAdjustment();
			item.SetActive(false);
		}
		
		currGame.SetActive(true);
	}
	
	public void RightArrow()
	{
		if (gamesAnim.IsActive()) return;
		ChangeItemPosition(currGame, 500, 0, 0);
		
		leftGame.SetActive(true);
		leftGame.transform.localPosition = new Vector3(-500, 0, 0);
		leftGame.transform.localScale = new Vector3(0, 0, 0); 
		Color curItemImage 
			= leftGame.GetComponentInChildren<UnityEngine.UI.Image>().color;
		curItemImage.a = 0;
		ChangeItemPosition(leftGame, 0, 1f, 1f);
		
		rightGame = currGame;		
		currGame = leftGame;
		
		if (availableGames.IndexOf(leftGame) != 0)
		{
			leftGame = availableGames
				[availableGames.IndexOf(leftGame) - 1];		
		}
		else 
		{
			leftGame = availableGames
				[availableGames.Count - 1];	
		}
	}
	
	public void LeftArrow()
	{
		if (gamesAnim.IsActive()) return;
		
		ChangeItemPosition(currGame, -500, 0, 0);
		
		rightGame.SetActive(true);
		rightGame.transform.localPosition = new Vector3(500, 0, 0);
		rightGame.transform.localScale = new Vector3(0, 0, 0); 
		Color curItemImage 
			= rightGame.GetComponentInChildren<UnityEngine.UI.Image>().color;
		curItemImage.a = 0;
		ChangeItemPosition(rightGame, 0, 1f, 1f);
		
		leftGame = currGame;		
		currGame = rightGame;
		
		if (availableGames.IndexOf(rightGame) < availableGames.Count - 1)
		{
			rightGame = availableGames
				[availableGames.IndexOf(rightGame) + 1];		
		}
		else 
		{
			rightGame = availableGames[0];	
		}
	}
	
	private void ChangeItemPosition(GameObject item, float finalMoveX,
		float finalFade, float finalScale)
	{
		gamesAnim = item.transform.DOLocalMove(new Vector3(finalMoveX, 0, 0), 0.5f);
		UnityEngine.UI.Image curItemImage 
			= item.GetComponent<UnityEngine.UI.Image>();
		curItemImage.DOFade(finalFade, 0.5f);
		item.transform.DOScale(finalScale, 0.5f).OnComplete(() => 
		{
			if (finalFade == 0) item.SetActive(false);
		});
	}
}
