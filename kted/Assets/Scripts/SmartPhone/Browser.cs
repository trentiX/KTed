using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Browser : MonoBehaviour
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
    [SerializeField] private Webpage mainPage;
    [SerializeField] public Sprite mainPageButtonIcon;
    [SerializeField] public string mainPageButtonTabName;
    [SerializeField] public Webpage mainPageButtonGoToPage;

    // Variables
    private List<GameObject> _tabsOpened = new List<GameObject>();
    private GameObject _addNewTab;
    private Tweener _browserAnim;
    private CanvasGroup _canvasGroup;
    
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
    private void AddNewTab(Sprite icon, string tabName, Webpage page)
    {
        Destroy(_addNewTab);
        GameObject newTab = Instantiate(tabsTemplate, tabsbox.transform);
        _tabsOpened.Add(newTab);

        _addNewTab = Instantiate(addNewTabButton, tabsbox.transform);
        _addNewTab.SetActive(true);
        
        // Get the existing Tab component and initialize it
        Tab tabComponent = newTab.GetComponent<Tab>();
        tabComponent.InitializeTab(icon, tabName, page, this);

        // Activate the tab now that it has been set up
        newTab.SetActive(true);
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
        ChangeAlpha(1, tab.gameObject);
    }

    public void CloseTab(Tab tab)
    {
        tab.CloseTab();
        _tabsOpened.Remove(tab.gameObject);

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

    public void OpenBrowser()
    {
        if (_browserAnim.IsActive())
        {
            return;
        }

        // Creating first main page tab
        AddNewTab(mainPageButtonIcon, mainPageButtonTabName, mainPageButtonGoToPage);
        OpenPage(mainPage, _tabsOpened[0].GetComponent<Tab>());
        
        browserOpen = true;
        _browserAnim = _canvasGroup.DOFade(1, 0.5f).OnComplete((() =>
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }));
    }
    
    public void CloseBrowser()
    {
        // Check if _browserAnim is null or not active before using it
        if (_browserAnim != null && _browserAnim.IsActive() && !_player.canMove())
        {
            return;
        }

        if (_tabsOpened != null)
        {
            // Create a copy of the list to avoid modifying while iterating
            List<GameObject> tabsToRemove = new List<GameObject>(_tabsOpened);

            foreach (var tabOpened in tabsToRemove)
            {
                _tabsOpened.Remove(tabOpened);
                Destroy(tabOpened);
            }
        }
        
        browserOpen = false;
        _browserAnim = _canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        });
    }
    
    public void OnShortcutClick(ShortCutButton shortCutButton)
    {
        AddNewTab(shortCutButton.icon, shortCutButton.tabName, shortCutButton.goToPage);
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
}

