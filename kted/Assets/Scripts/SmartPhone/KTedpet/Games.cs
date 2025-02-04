using DG.Tweening;
using TMPro;
using UnityEngine;

public class Games : MonoBehaviour
{
	// Serialization
	[SerializeField] public GameObject image;	
	[SerializeField] public UnityEngine.UI.Button playButton;
	[SerializeField] private IPlayable game;

	
	// Variables
	private Tweener gameAnim;
	private PlayRoomManager playRoomManager;
	private KTedpet ktedPet;
	
	// Code
	private void Awake()
	{
		playRoomManager = FindObjectOfType<PlayRoomManager>();
		ktedPet = FindObjectOfType<KTedpet>();
		
		buttonAdjustment();
	}
	
	public void buttonAdjustment()
	{	
		playButton.onClick.RemoveAllListeners();
		playButton.gameObject.GetComponentInChildren
				<TextMeshProUGUI>().text = "Поиграть";
		playButton.onClick.AddListener(ktedPet.GoToPlay);
	}
	
	public void PlayStartAnim()
	{
		ktedPet.StartMinigameAnim(this);
		Debug.Log("play game");
	}
	
	public void StartGame()
	{
		game.StartGame();
	}	
	
	private void OnEnable()
	{
		if (image == null) return;
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
