using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class KTedpet : MonoBehaviour
{
	// Phrases
	private string[] playPhrases = {"go to play", "lets play!"};
	private string[] gotoStorePhrases = {"go shop", "lets shop!"};	
	
	// References
	[Header("Messages:")]
	[SerializeField] private GameObject messageTemplate;
	[SerializeField] private GameObject responseTemplate;
	[SerializeField] private GameObject messageBox;
	
	[Header("Responses:")]
	[SerializeField] private GameObject buttonTemplate;
	
	[Header("Other:")]
	[SerializeField] private CanvasGroup canvasGroup;
	
	// Variables
	private List<GameObject> possibleActivities = new List<GameObject>();
	// Code
	public void GenerateMessage(string message, string typeOfMessage)
	{
		GameObject newMessage = Instantiate(messageTemplate, messageBox.transform);
		newMessage.SetActive(true);
		newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
		
		
		foreach	(var button in possibleActivities)
		{
			Destroy(button);
		}
		possibleActivities.Clear();
		GenerateButton(typeOfMessage);
	}
	
	private void GenerateButton(string typeOfMessage)
	{
		GameObject firstButton = Instantiate(messageTemplate, messageBox.transform);
		firstButton.SetActive(true);
		possibleActivities.Add(firstButton);
		
		GameObject secondButton = Instantiate(messageTemplate, messageBox.transform);
		secondButton.SetActive(true);
		possibleActivities.Add(secondButton);

		
		switch (typeOfMessage)
		{
			case "start":
				firstButton.GetComponent<UnityEngine.UI.Button>().onClick
					.AddListener(GoToPlay);
				firstButton.GetComponentInChildren<TextMeshProUGUI>().text
					= "Поиграем!";
				
				secondButton.GetComponent<UnityEngine.UI.Button>().onClick
					.AddListener(GoToStore);
				secondButton.GetComponentInChildren<TextMeshProUGUI>().text
					= "В магазин!";
				break;
		}
	}
	
	private void GenerateResponse(string message)
	{
		GameObject newResponse = Instantiate(responseTemplate, messageBox.transform);
		newResponse.SetActive(true);
		newResponse.GetComponentInChildren<TextMeshProUGUI>().text = message;
						
		// After adding the new response, force its layout rebuild
		LayoutRebuilder.ForceRebuildLayoutImmediate(newResponse.GetComponent<RectTransform>());
	}
	
	public void GoToStore()
	{
		GenerateResponse(gotoStorePhrases
			[Random.Range(0, gotoStorePhrases.Length)]);
	}
	
	public void GoToPlay()
	{
		GenerateResponse(playPhrases
			[Random.Range(0, playPhrases.Length)]);
	}
}
