using UnityEngine;

public class ShortCutButton : MonoBehaviour
{
	[SerializeField] public Sprite icon;
	[SerializeField] public string tabName;
	[SerializeField] public Webpage goToPage;
	[SerializeField] private GameObject notificationMark;
	
	private NotificationManager notificationManager;
	
	private void Awake()
	{
		notificationManager = FindObjectOfType<NotificationManager>();
		DeleteNotification();
	}
	public void CreateNotification(string message)
	{
		if (notificationMark == null || Browser.instance.currPage == goToPage && Browser.instance.browserOpen) return;
		notificationMark.SetActive(true);
		notificationManager.SendNotification(message);
	}
	
	public void DeleteNotification()
	{
		if (notificationMark == null) return;
		notificationMark.SetActive(false);
		notificationManager.RemoveNotifications();
	}
}
