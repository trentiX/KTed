using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField] private UnityEngine.UI.Button storeButton;
	[SerializeField] private UnityEngine.UI.Button playButton;
	
	[Header("Other:")]
	[SerializeField] private CanvasGroup canvasGroup;
	
	// Variables
	private List<GameObject> possibleActivities = new List<GameObject>();
	// Code
	public void GenerateMessage(string message)
	{
		GameObject newMessage = Instantiate(messageTemplate, messageBox.transform);
		newMessage.SetActive(true);
		newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
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
