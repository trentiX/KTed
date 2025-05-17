using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	// Serialization
	[SerializeField] private Webpage ktedpet;
	
	// Player can move notification
	[Header("notification")]
	[SerializeField] private GameObject notificationTemplate;
	[SerializeField] private GameObject hideAllButtonTemplate;
	[SerializeField] private GameObject notificationsBox;
	
	// Player cant move notification
	[Header("Small window")]
	[SerializeField] private GameObject smallWinTemplate;
	[SerializeField] private TextMeshProUGUI smallWinBoxText;
	
	// Variables
	public List<GameObject> notifications;
	private Browser browser;
	private Player player;
	public static NotificationManager instance;
	
	// Code
	private void Awake()
	{
		instance = this;
		player = FindObjectOfType<Player>();
		browser = FindObjectOfType<Browser>();
		notifications = new List<GameObject>();	
	}
	
	public void SendNotification(string message)
	{
		if (!player.canMove()) return;
		
		if (notifications.Count > 3)
		{
			Destroy(notifications[0]);
			notifications.RemoveAt(0);
		}
		
		GameObject newNotification = Instantiate(notificationTemplate, notificationsBox.transform);
		newNotification.SetActive(true);
			
		newNotification.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenKtedPet);
		newNotification.GetComponentInChildren<TextMeshProUGUI>().text = message;
			
		notifications.Add(newNotification);
			
		if (notifications.Count > 2)
		{
			return;
		}
		
		if (notifications.Count > 1)
		{
			GameObject hideAllButton = Instantiate(hideAllButtonTemplate, notificationsBox.transform);
			hideAllButton.SetActive(true);
			hideAllButton.transform.SetSiblingIndex(0);
				
			notifications.Add(hideAllButton);
			hideAllButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(HideAllButton);
		}	
	}
	
	public void SendSmallWindowText(string message)
	{
		smallWinTemplate.SetActive(true);
		smallWinTemplate.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
		smallWinTemplate.GetComponent<CanvasGroup>().blocksRaycasts = true;
		smallWinTemplate.GetComponent<CanvasGroup>().interactable = true;
		smallWinBoxText.text = message;
		WindowDrag.instance.UpdateWindowSize();
	}
	public void CloseSmallWindowText()
	{
		smallWinTemplate.GetComponent<CanvasGroup>().DOFade(0, 0.5f);
		smallWinTemplate.GetComponent<CanvasGroup>().blocksRaycasts = false;
		smallWinTemplate.GetComponent<CanvasGroup>().interactable = false;
	}
	
	public void RemoveNotifications()
	{
		if (notifications == null) return; 
		foreach (var notif in notifications)
		{
			if (notif == null) return;
			Destroy(notif);
		}
		notifications.Clear();
	}
	
	public void HideAllButton()
	{
		RemoveNotifications();
	}
	
	public void OpenKtedPet()
	{
		browser.OpenBrowser(ktedpet, false);
		
		RemoveNotifications();
	}
}

