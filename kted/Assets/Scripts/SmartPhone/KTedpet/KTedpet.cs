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
	[SerializeField][TextArea] private string[] gotoPlayPhrases;
	[SerializeField][TextArea] private string[] gotoStorePhrases;	
	[SerializeField][TextArea] private string[] goBackPhrases;
	
	// Serialization
	[Header("Messages:")]
	[SerializeField] private GameObject messageTemplate;
	[SerializeField] private GameObject responseTemplate;
	[SerializeField] private GameObject messageBox;
	
	[Header("Buttons:")]
	[SerializeField] private GameObject buttonTemplate;
	
	[Header("Rooms:")]
	[SerializeField] private GameObject mainRoom;
	[SerializeField] private GameObject playRoom;
	[SerializeField] private GameObject storeRoom;

	[Header("Other:")]
	[SerializeField] private CanvasGroup canvasGroup;
	
	// Variables
	private List<GameObject> possibleActivities = new List<GameObject>();
	private GameObject currRoom;
	private AudioManager audioManager;
	private Pet pet;
	
	// Code
	private void Start()
	{
		currRoom = mainRoom;
		pet = FindObjectOfType<Pet>();
		audioManager = FindObjectOfType<AudioManager>();
	}
	
	public void GenerateMessage(string message, string typeOfMessage)
	{
		// Создаём сообщение, но без текста
		GameObject newMessage = Instantiate(messageTemplate, messageBox.transform);
		newMessage.SetActive(true);

		LayoutRebuilder.ForceRebuildLayoutImmediate(newMessage.GetComponent<RectTransform>());	

		// Запускаем корутину для анимации точек, затем добавляем текст и кнопки
		StartCoroutine(MessageWithDotsAnimation(newMessage, message, typeOfMessage));
	}

	
	private IEnumerator MessageWithDotsAnimation(GameObject message, string finalText, string typeOfMessage)
	{
		TextMeshProUGUI messageText = message.GetComponentInChildren<TextMeshProUGUI>();
		
		// Анимация точек
		for (int i = 0; i <= 3; i++)
		{
			messageText.text = new string('.', i);
			LayoutRebuilder.ForceRebuildLayoutImmediate(message.GetComponent<RectTransform>());	
			yield return new WaitForSeconds(0.6f);
		}
		
		// Устанавливаем окончательный текст
		messageText.text = finalText;
		audioManager.SFXNotificationSound();
		
		// Перестраиваем UI после изменения текста
		LayoutRebuilder.ForceRebuildLayoutImmediate(message.GetComponent<RectTransform>());
		
		// Создаю уведомление
		gameObject.GetComponent<Webpage>().shortCutButton.CreateNotification(finalText);

		// Генерируем кнопки после завершения анимации
		StartCoroutine(GenerateButton(typeOfMessage));
	}

	private IEnumerator GenerateButton(string typeOfMessage)
	{
		// Создаем кнопки в зависимости от типа сообщения
		List<GameObject> buttons = typeOfMessage switch
		{
			"greeting" => CreateButtons(1),
			"whatToDo" => CreateButtons(3),
			_ => new List<GameObject>() // Если тип неизвестен, возвращаем пустой список
		};

		if (typeOfMessage == "greeting")
		{
			ConfigureButton(buttons[0], GetRandomPhrase(playerGreetingPhrases), Greet);
		}
		else if (typeOfMessage == "agree")
		{
			yield return new WaitForSeconds(Random.Range(4, 8));
			GenerateMessage(GetRandomPhrase(pet.continueDialoguePetPhrases), "whatToDo");
		}
		else if (typeOfMessage == "whatToDo")
		{
			ConfigureButton(buttons[0], GetRandomPhrase(goBackPhrases), GoBack);
			ConfigureButton(buttons[1], GetRandomPhrase(gotoPlayPhrases), GoToPlay);	
			ConfigureButton(buttons[2], GetRandomPhrase(gotoStorePhrases), GoToStore);	
		}
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
			GameObject newButton = Instantiate(buttonTemplate, messageBox.transform);
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
		
		GameObject newResponse = Instantiate(responseTemplate, messageBox.transform);
		newResponse.SetActive(true);
		newResponse.GetComponentInChildren<TextMeshProUGUI>().text = message;
						
		// After adding the new response, force its layout rebuild
		LayoutRebuilder.ForceRebuildLayoutImmediate(newResponse.GetComponent<RectTransform>());
	}
	
	public string GetRandomPhrase(string[] phrases)
	{
		return phrases[Random.Range(0, phrases.Length)];
	}
	
	public void GoToStore()
	{
		GenerateResponse(GetRandomPhrase(gotoStorePhrases));
			
		if (RoomTransition(storeRoom))
			pet.ReceiveAction("store");
	}
	
	public void GoToPlay()
	{
		GenerateResponse(GetRandomPhrase(gotoPlayPhrases));
		
		if (RoomTransition(playRoom))
			pet.ReceiveAction("play");
	}
	
	public void GoBack()
	{
		GenerateResponse(GetRandomPhrase(goBackPhrases));
		
		if (RoomTransition(mainRoom))
			pet.ReceiveAction("main");
	}
	
	public void Greet()
	{
		GenerateResponse(GetRandomPhrase(playerGreetingPhrases));
		
		pet.ReceiveAction("playerGreeting");
	}
	
	private bool RoomTransition(GameObject gotoRoom)
	{
		if (currRoom != null) 
		{
			if (currRoom == gotoRoom)
			{
				GenerateMessage(GetRandomPhrase(pet.gotoSameRoomPhrases), "whatToDo");
				return false;
			}
			
			CanvasGroup currRoomCanvas = currRoom.GetComponent<CanvasGroup>();
			
			currRoomCanvas.interactable = false;
			currRoomCanvas.blocksRaycasts = false;
			currRoomCanvas.DOFade(0, 0.4f).OnComplete(() =>
			{
				currRoom.SetActive(false);
			});
		}
		
		CanvasGroup gotoRoomCanvas = gotoRoom.GetComponent<CanvasGroup>();
		gotoRoom.SetActive(true);
		gotoRoomCanvas.DOFade(1, 0.4f).OnComplete(()=>
		{
			currRoom = gotoRoom;
		});
		gotoRoomCanvas.interactable = true;
		gotoRoomCanvas.blocksRaycasts = true;	
		
		return true;
	}
}