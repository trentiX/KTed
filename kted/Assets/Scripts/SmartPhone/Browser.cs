using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class Browser : MonoBehaviour, IDataPersistence
{
	// Serialization
	[Header("Tabs")]
	[SerializeField] private GameObject tabsbox;
	[SerializeField] private GameObject tabsTemplate;
	[SerializeField] private GameObject addNewTabButton;
	[SerializeField] private TextMeshProUGUI url;

	[Header("Browser main buttons:")] 
	[SerializeField] private GameObject[] browserMainButtons;
	
	[Header("Main page")] 
	[SerializeField] public Webpage mainPage;
	[SerializeField] public Sprite mainPageButtonIcon;
	[SerializeField] public string mainPageButtonTabName;
	[SerializeField] public Webpage mainPageButtonGoToPage;

	// Variables
	private List<GameObject> _tabsOpened = new List<GameObject>();
	private GameObject _addNewTab;
	private Tweener _browserAnim;
	private CanvasGroup _canvasGroup;
	private GameObject _extraTab;
	private int _extraClicksCount;
	private bool _easterEggFound = false;
	
	[HideInInspector] public Webpage currPage;
	[HideInInspector] public Webpage prevPage;
	[HideInInspector] public Tab currTab;
	[HideInInspector] public Tab prevTab;
	[HideInInspector] public bool browserOpen;

	
	// References
	private Webpage _webpage;
	private Player _player;

	// Code 
	private void Start()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
		_player = FindObjectOfType<Player>();
		
		_canvasGroup.alpha = 0;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
		
		currPage = mainPage;
		prevPage = mainPage;
		tabsTemplate.SetActive(false);
		OnBrowserStart();
	}
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
		{
			CloseBrowser();
		}
	}
	
	public Tab AddNewTab(Sprite icon, string tabName, Webpage page, bool firstPage)
	{
		if (page != mainPage && !firstPage)
		{
			_tabsOpened.Remove(currTab.gameObject);
			currTab.InitializeTab(icon, tabName, page, this, true);
			currPage.Close(page);
			currPage = page;
			_tabsOpened.Add(currTab.gameObject);
			url.text = page.url;
			return null;
		}
		
		Destroy(_addNewTab);
		GameObject newTab = Instantiate(tabsTemplate, tabsbox.transform);
		_tabsOpened.Add(newTab);

		_addNewTab = Instantiate(addNewTabButton, tabsbox.transform);
		_addNewTab.SetActive(true);
		
		// Get the existing Tab component and initialize it
		Tab tabComponent = newTab.GetComponent<Tab>();
		tabComponent.InitializeTab(icon, tabName, page, this,true);

		// Activate the tab now that it has been set up
		newTab.SetActive(true);

		if (_tabsOpened.Count > 4)
		{
			foreach (var tab in _tabsOpened)
			{
				RectTransform rectTransform = tab.GetComponent<RectTransform>();
				rectTransform.sizeDelta = new Vector2(1200/_tabsOpened.Count, rectTransform.sizeDelta.y);
			}
		}
		return tabComponent;
	}
	
	public void OpenPage(Webpage page, Tab tab)
	{
		currPage.Close(page); // Close currPage and open page
		prevPage = currPage;
		currPage = page;
		
		url.text = page.url;
		
		// Make prev tab transparent and assign new prevTab
		if (currTab != null)
		{
			ChangeAlpha(0, currTab.gameObject);
			prevTab = currTab;
		}
		// Assign new currTab
		currTab = tab;
		
		// Make currTab opaque
		ChangeAlpha(0.5f, tab.gameObject);
	}

	public void CloseTab(Tab tab)
	{
		tab.CloseTab();
		_tabsOpened.Remove(tab.gameObject);

		if (_tabsOpened.Count >= 4)
		{
			foreach (var tabOpened in _tabsOpened)
			{
				RectTransform rectTransform = tabOpened.GetComponent<RectTransform>();
				rectTransform.sizeDelta = new Vector2(1200/_tabsOpened.Count, rectTransform.sizeDelta.y);
			}
		}
		
		if (_tabsOpened.Count == 0)
		{
			CloseBrowser();
			return;
		}
				
		if (currTab == tab)
		{
			prevTab = _tabsOpened.Last().GetComponent<Tab>();
			prevPage = prevTab.itsPage;
			OpenPage(prevPage, prevTab);
		}
	}

	public void OpenBrowser(Webpage openPage, bool firstTime)
	{
		if (_browserAnim.IsActive() || !_player.canMove())
		{
			return;
		}
		
		//Cursor.visible = true;
		browserOpen = true;
		_browserAnim = _canvasGroup.DOFade(1, 0.2f).OnComplete((() =>
		{
			_canvasGroup.interactable = true;
			_canvasGroup.blocksRaycasts = true;
		}));
		
		// Creating first main page tab
		
		if (firstTime && _tabsOpened.Count > 0) return;
	
		foreach (var tab in _tabsOpened)
		{
			if (openPage == tab.GetComponent<Tab>().itsPage)
			{
				OpenPage(tab.GetComponent<Tab>().itsPage, tab.GetComponent<Tab>());
				return;
			}
		}
		
		if (currTab != null ) ChangeAlpha(0, currTab.gameObject);
		currTab = AddNewTab(openPage.shortCutButton.icon, openPage.shortCutButton.tabName, openPage, true);
		
		OpenPage(openPage, currTab);
	}
	
	public void CloseBrowser()
	{
		// Check if _browserAnim is null or not active before using it
		if (_browserAnim != null && _browserAnim.IsActive() && !_player.canMove())
		{
			return;
		}
		
		browserOpen = false;
		_browserAnim = _canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
		{
			_canvasGroup.interactable = false;
			_canvasGroup.blocksRaycasts = false;
		});
		//Cursor.visible = false;
	}
	
	public void OnShortcutClick(ShortCutButton shortCutButton)
	{
		AddNewTab(shortCutButton.icon, shortCutButton.tabName, shortCutButton.goToPage, false);
	}
	
	private void ChangeAlpha(float value, GameObject chat)
	{
		var color = chat.GetComponent<Image>().color;
		color.a = value;
		chat.GetComponent<Image>().color = color;
	}
	private void OnBrowserStart()
	{
		foreach (var button in browserMainButtons)
		{
			ChangeAlpha(0, button);
			
			EventTrigger eventTrigger = button.AddComponent<EventTrigger>();

			EventTrigger.Entry onEnter = new EventTrigger.Entry()
			{
				eventID = EventTriggerType.PointerEnter
			};
		
			EventTrigger.Entry onExit = new EventTrigger.Entry()
			{
				eventID = EventTriggerType.PointerExit
			};

			onEnter.callback.AddListener((AbstractEventData) =>
			{
				ChangeAlpha(0.5f, button);

			});
			onExit.callback.AddListener((AbstractEventData) =>
			{
				ChangeAlpha(0, button);
			});
		
			eventTrigger.triggers.Add(onEnter);
			eventTrigger.triggers.Add(onExit);
		}
	}
	private void AddExtraTab(Sprite icon, string tabName, Webpage page)
	{
		if (_extraTab != null)
		{
			return;
		}
		
		GameObject newTab = Instantiate(tabsTemplate, tabsbox.transform);
		Tab tabComponent = newTab.GetComponent<Tab>();
		
		RectTransform rectTransform = newTab.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(500, rectTransform.sizeDelta.y);

		switch (_extraClicksCount)
		{
			case 1:
				tabName = "Не нажимай!";
				break;
			case 2:
				tabName = "На кнопку!";
				break;
			case 3:
				tabName = "ПОЖАЛУЙСТА";
				break;
			case 4:
				tabName = "Удали другие";
				break;
			case 5:
				tabName = "ВКЛАДКИИ!!";
				break;
			case 6:
				tabName = "ПОЖАЛУЙСТААА";
				break;
			case 7:
				tabName = "МЫ ПРОСИМ";
				break;;
			case 8:
				tabName = "НЕ НАДООО";
				break;
			case 9:
				tabName = "Ты доигрался...";
				_easterEggFound = true;
				break;
		}
		
		tabComponent.InitializeTab(icon, tabName, page, this,false);
		newTab.SetActive(true);
		_extraClicksCount++;

		_extraTab = newTab;

		// Добавление Rigidbody2D для гравитации
		Rigidbody2D newTabRb = newTab.AddComponent<Rigidbody2D>();
		newTabRb.gravityScale = 90;
		newTabRb.mass = 50;
		
		// Уничтожение таба после падения (через несколько секунд)
		Destroy(newTab, 2f);
	}
	
	// DATA

	public void LoadData(GameData gameData)
	{
		_easterEggFound = gameData.browserEasterEggFound;
	}
	public void SaveData(ref GameData gameData)
	{
		gameData.browserEasterEggFound = _easterEggFound;
	}
}

