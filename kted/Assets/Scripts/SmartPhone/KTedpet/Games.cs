using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Games : MonoBehaviour
{
    // Serialization
	[SerializeField] public GameObject image;	
	[SerializeField] public UnityEngine.UI.Button playButton;
	
	
	// Variables
	private Tweener gameAnim;
	private PlayRoomManager playRoomManager;
	
	// Code
	private void Awake()
	{
		playRoomManager = FindObjectOfType<PlayRoomManager>();
	}
	
	public void buttonAdjustment()
	{	
		playButton.onClick.RemoveAllListeners();
		playButton.gameObject.GetComponentInChildren
				<TextMeshProUGUI>().text = "Поиграть";
			playButton.onClick.AddListener(playRoomManager.PlayGame);
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
