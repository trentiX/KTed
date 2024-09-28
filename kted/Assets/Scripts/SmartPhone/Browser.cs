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
    [SerializeField] private TextMeshProUGUI url;

    [Header("Browser main buttons:")] 
    [SerializeField] private GameObject[] browserMainButtons;
    
    [Header("Shortcuts")] 
    [SerializeField] private Webpage mainPage;

    // Variables
    private List<GameObject> _tabsOpened = new List<GameObject>();
    private Tweener browserAnimation;
    [HideInInspector] public Webpage currPage;
    [HideInInspector] public Webpage prevPage;
    
    // References
    private Webpage _webpage;
    
    // Code 
    private void Start()
    {
        currPage = mainPage;
        prevPage = mainPage;
        tabsTemplate.SetActive(false);
        OnBrowserStart();
    }
    private void AddNewTab(Sprite icon, string tabName, Webpage page)
    {
        GameObject newTab = Instantiate(tabsTemplate, tabsbox.transform);
        _tabsOpened.Add(newTab);

        // Get the existing Tab component and initialize it
        Tab tabComponent = newTab.GetComponent<Tab>();
        tabComponent.InitializeTab(icon, tabName, page, this);

        // Activate the tab now that it has been set up
        newTab.SetActive(true);
    }



    public void OpenPage(Webpage page)
    {
        prevPage.Close();
        page.Open();
        url.text = page.url;
    }

    public void CloseTab(Tab tab)
    {
        tab.CloseTab();
    }
    public void OnShortcutClick(ShortCutButton shortCutButton)
    {
        AddNewTab(shortCutButton.icon, shortCutButton.tabName, shortCutButton.goToPage);
    }
    public void OnHomeClick()
    {
        mainPage.Open();
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

