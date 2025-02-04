using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class KTedpet : MonoBehaviour
{
	// Phrases
	[Header("Players phrases:")]
	[SerializeField][TextArea] private string[] playerGreetingPhrases;
	[SerializeField][TextArea] private string[] gotoChooseGamePhrases;
	[SerializeField][TextArea] private string[] gotoPlayPhrases;
	[SerializeField][TextArea] private string[] gotoStorePhrases;	
	[SerializeField][TextArea] private string[] goBackPhrases;
	
	// Serialization
	[Header("Messages:")]
	[SerializeField] private GameObject messageTemplate;
	[SerializeField] private GameObject responseTemplate;
	[SerializeField] private GameObject messages;
	[SerializeField] private ScrollRect messageBox;

	
	[Header("Buttons:")]
	[SerializeField] private GameObject buttonTemplate;
	
	[Header("Rooms:")]
	[SerializeField] private GameObject mainRoom;
	[SerializeField] private GameObject chooseGameRoom;
	[SerializeField] private GameObject storeRoom;
	[SerializeField] private GameObject playingRoom;


	[Header("Minigames:")]
	[SerializeField] Games[] games;
	[SerializeField] private CanvasGroup[] messageBoxCanvas;

	
	[Header("Other:")]
	[SerializeField] private CanvasGroup canvasGroup;
	
	// Variables
	private List<GameObject> possibleActivities = new List<GameObject>();
	private GameObject currRoom;
	private AudioManager audioManager;
	private PlayRoomManager playRoomManager;
	private Pet pet;
	
	// Code
	private void Start()
	{
		currRoom = mainRoom;
		pet = FindObjectOfType<Pet>();
		audioManager = FindObjectOfType<AudioManager>();
		playRoomManager = FindObjectOfType<PlayRoomManager>();
	}
	
	public void GenerateMessage(string message, string typeOfMessage)
	{
		// Создаём сообщение, но без текста
		GameObject newMessage = Instantiate(messageTemplate, messages.transform);
		newMessage.SetActive(true);

		LayoutRebuilder.ForceRebuildLayoutImmediate(newMessage.GetComponent<RectTransform>());	
		LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
		messageBox.verticalNormalizedPosition = 0;
		
		// Запускаем корутину для анимации точек, затем добавляем текст и кнопки
		StartCoroutine(MessageWithDotsAnimation(newMessage, message, typeOfMessage));
	}

	
	private IEnumerator MessageWithDotsAnimation(GameObject message, string finalText, string typeOfMessage)
	{
		TextMeshProUGUI messageText = message.GetComponentInChildren<TextMeshProUGUI>();
		
		LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
		messageBox.verticalNormalizedPosition = 0;
		
		// Анимация точек
		for (int i = 0; i <= 3; i++)
		{
			messageText.text = new string('.', i);
			LayoutRebuilder.ForceRebuildLayoutImmediate(message.GetComponent<RectTransform>());	
			LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
			messageBox.verticalNormalizedPosition = 0;		
			yield return new WaitForSeconds(0.6f);
		}
		
		// Устанавливаем окончательный текст
		messageText.text = finalText;
		audioManager.SFXNotificationSound();
		
		// Перестраиваем UI после изменения текста
		LayoutRebuilder.ForceRebuildLayoutImmediate(message.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
		messageBox.verticalNormalizedPosition = 0;
		
		// Создаю уведомление
		gameObject.GetComponent<Webpage>().shortCutButton.CreateNotification(finalText);

		// Генерируем кнопки после завершения анимации
		GenerateButton(typeOfMessage);
	}

	private void GenerateButton(string typeOfMessage)
	{
		// Создаем кнопки в зависимости от типа сообщения
		List<GameObject> buttons = typeOfMessage switch
		{
			"greeting" => CreateButtons(1),
			"whatToDo" => CreateButtons(3),
			"play" => CreateButtons(2),
			_ => new List<GameObject>() // Если тип неизвестен, возвращаем пустой список
		};

		switch (typeOfMessage)
		{
			case "greeting":
				ConfigureButton(buttons[0], GetRandomPhrase(playerGreetingPhrases), Greet);
				break;

			case "agree":
				GenerateMessage(GetRandomPhrase(pet.continueDialoguePetPhrases), "whatToDo");
				break;
			case "whatToDo":
				ConfigureButton(buttons[0], GetRandomPhrase(goBackPhrases), GoBack);
				ConfigureButton(buttons[1], GetRandomPhrase(gotoChooseGamePhrases), GoToChooseGame);
				ConfigureButton(buttons[2], GetRandomPhrase(gotoStorePhrases), GoToStore);
				break;

			default:
				Debug.LogWarning($"Unhandled message type: {typeOfMessage}");
				break;
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
		messageBox.verticalNormalizedPosition = 0;
	}
	
	private void ConfigureButton(GameObject button, string text, UnityEngine.Events.UnityAction action)
	{
		button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(action);
		button.GetComponentInChildren<TextMeshProUGUI>().text = text;
		possibleActivities.Add(button);
	}

	private List<GameObject> CreateButtons(int amountOfButtons)
	{
		List<GameObject> createdButtons = new List<GameObject>();
			
		for (int i = 0; i < amountOfButtons; i++)
		{
			GameObject newButton = Instantiate(buttonTemplate, messages.transform);
			newButton.SetActive(true);
			createdButtons.Add(newButton);
			
			LayoutRebuilder.ForceRebuildLayoutImmediate(newButton.GetComponent<RectTransform>());
		}
		return createdButtons;
	}	
	
	private void GenerateResponse(string message)
	{
		foreach	(var button in possibleActivities)
		{
			Destroy(button);
		}
		possibleActivities.Clear();
		
		GameObject newResponse = Instantiate(responseTemplate, messages.transform);
		newResponse.SetActive(true);
		newResponse.GetComponentInChildren<TextMeshProUGUI>().text = message;
						
		// After adding the new response, force its layout rebuild
		LayoutRebuilder.ForceRebuildLayoutImmediate(newResponse.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
		messageBox.verticalNormalizedPosition = 0;
	}
	
	public string GetRandomPhrase(string[] phrases)
	{
		return phrases[Random.Range(0, phrases.Length)];
	}
	
	public void GoToStore()
	{
		GenerateResponse(GetRandomPhrase(gotoStorePhrases));

		if (CanRoomChange(storeRoom))
		{
	  		StartCoroutine(RoomTransition(storeRoom));
			pet.ReceiveAction("store");
		}
	}

	public void GoToChooseGame()
	{
		GenerateResponse(GetRandomPhrase(gotoChooseGamePhrases));

		if (CanRoomChange(chooseGameRoom))
		{
			StartCoroutine(RoomTransition(chooseGameRoom));
			pet.ReceiveAction("chooseGame");
		}
	}

	public void GoBack()
	{
		GenerateResponse(GetRandomPhrase(goBackPhrases));

		if (CanRoomChange(mainRoom))
		{
			StartCoroutine(RoomTransition(mainRoom));
			pet.ReceiveAction("main");
		}
	}

	public void GoToPlay()
	{
		playRoomManager.currGame.GetComponent<Games>().PlayStartAnim();
	}
	
	public void Greet()
	{
		GenerateResponse(GetRandomPhrase(playerGreetingPhrases));
		
		pet.ReceiveAction("playerGreeting");
	}
	
	private IEnumerator RoomTransition(GameObject gotoRoom)
	{
		yield return new WaitForSeconds(2.5f);
		
		CanvasGroup currRoomCanvas = currRoom.GetComponent<CanvasGroup>();
		
		currRoomCanvas.interactable = false;
		currRoomCanvas.blocksRaycasts = false;
		currRoomCanvas.DOFade(0, 0.4f).OnComplete(() =>
		{
			currRoom.SetActive(false);
		});
		
		CanvasGroup gotoRoomCanvas = gotoRoom.GetComponent<CanvasGroup>();
		gotoRoom.SetActive(true);
		gotoRoomCanvas.DOFade(1, 0.4f).OnComplete(()=>
		{
			currRoom = gotoRoom;
		});
		gotoRoomCanvas.interactable = true;
		gotoRoomCanvas.blocksRaycasts = true;		
	}
	
	public void StartMinigameAnim(Games game)
	{
		foreach (var canvas in messageBoxCanvas)
		{
			canvas.DOFade(0, 0.5f).OnComplete(() =>
			{
				canvas.gameObject.SetActive(false);
			});
		}
		
		playRoomManager.currGame.GetComponent<Games>().StartGame();
	}
	
	private bool CanRoomChange(GameObject gotoRoom)
	{
		if (currRoom != null) 
		{
			if (currRoom == gotoRoom)
			{
				GenerateMessage(GetRandomPhrase(pet.gotoSameRoomPhrases), "whatToDo");
				return false;
			}
		}
		return true;
	} 
}