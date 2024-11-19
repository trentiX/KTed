using UnityEngine;

public class ShortCutButton : MonoBehaviour
{
	[SerializeField] public Sprite icon;
	[SerializeField] public string tabName;
	[SerializeField] public Webpage goToPage;
	[SerializeField] private GameObject notificationMark;
	
	private void Awake()
	{
		DeleteNotification();
	}
	public void CreateNotification()
	{
		if (notificationMark == null) return;
		notificationMark.SetActive(true);
	}
	
	public void DeleteNotification()
	{
		if (notificationMark == null) return;
		notificationMark.SetActive(false);
	}
}
