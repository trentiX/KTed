using TMPro;
using UnityEngine;

public class KTedpet : MonoBehaviour
{
	// References
	[Header("Messages:")]
	[SerializeField] private GameObject messageTemplate;
	[SerializeField] private GameObject responseTemplate;
	[SerializeField] private GameObject messageBox;
	
	[Header("Other:")]
	[SerializeField] private CanvasGroup canvasGroup;
	
	// Code
	public void GenerateMessage(string message)
	{
		GameObject newMessage = Instantiate(messageTemplate, messageBox.transform);
        newMessage.SetActive(true);
        newMessage.GetComponentInChildren<TextMeshProUGUI>().text = message;
	}
}
