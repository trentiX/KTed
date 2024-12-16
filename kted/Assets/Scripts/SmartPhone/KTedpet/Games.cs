using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Games : MonoBehaviour
{
	// Serialization
	[SerializeField] public GameObject image;	
	[SerializeField] public GameObject pet;
	
	// Variables
	private Tweener gameAnim;
	private PlayRoomManager playRoomManager;
	private KTedpet ktedPet;
	
	// Code
	private void Awake()
	{
		playRoomManager = FindObjectOfType<PlayRoomManager>();
		ktedPet = FindObjectOfType<KTedpet>();
	}
	
	// public void buttonAdjustment()
	// {	
	// 	playButton.onClick.RemoveAllListeners();
	// 	playButton.gameObject.GetComponentInChildren
	// 			<TextMeshProUGUI>().text = "Поиграть";
	// 	playButton.onClick.AddListener(PlayGame);
	// }
	
	virtual public void PlayGame()
	{
		// play game
		Debug.Log("play game");
	}
	
	private void OnEnable()
	{
		image.transform.Rotate(0, 0, -0.5f);
		gameAnim = image.transform.DORotate(new Vector3(0, 0, 1), 1f)
			.SetLoops(-1, LoopType.Yoyo)
			.SetEase(Ease.InOutSine);
	}
	private void OnDisable()
	{
		gameAnim.Kill();
	}
}
