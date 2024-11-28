using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	// Serialization
	[SerializeField] private GameObject notificationTemplate;
	[SerializeField] private GameObject hideAllButtonTemplate;
	[SerializeField] private GameObject notificationsBox;
	[SerializeField] private Webpage ktedpet;
	
	
	// Variables
	private List<GameObject> notifications;
	private Browser browser;
	private Player player;
	
	// Code
	private void Awake()
	{
		player = FindObjectOfType<Player>();
		browser = FindObjectOfType<Browser>();
		notifications = new List<GameObject>();	
	}
	
	public void SendNotification(string message)
	{
		if (!player.canMove()) return;
		GameObject newNotification = Instantiate(notificationTemplate, 
			notificationsBox.transform);
		newNotification.SetActive(true);
		
		newNotification.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenKtedPet);
		newNotification.GetComponentInChildren<TextMeshProUGUI>().text = message;
		
		notifications.Add(newNotification);
		
		if (notifications.Count > 1)
		{
			GameObject hideAllButton = Instantiate(hideAllButtonTemplate,
				notificationsBox.transform);
			hideAllButton.SetActive(true);
			hideAllButton.transform.SetSiblingIndex(0);
			
			notifications.Add(hideAllButton);
			hideAllButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(HideAllButton);
		}
			}
	
	public void RemoveNotifications()
	{
		foreach (var notif in notifications)
		{
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

