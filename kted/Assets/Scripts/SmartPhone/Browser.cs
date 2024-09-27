using System;
using System.Collections.Generic;
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
    [SerializeField] private GameObject addNewTab;
    
    [Header("Shortcuts")] 
    [SerializeField] private GameObject mainPage;

    // Variables
    private List<GameObject> _tabsOpened = new List<GameObject>();
    private Dictionary<GameObject, GameObject> shortcuts = new Dictionary<GameObject, GameObject>();
    private Tweener browserAnimation;
    private GameObject openedPage;
    private GameObject addNewTabTemp;
    
    // Code 
    private void Start()
    {
        openedPage = mainPage;
    }

    private void AddNewTab(GameObject tab, ShortCutButton shortCutButton)
    {
        if (tab == null)
        {
            Debug.Log("tab is null");
            return;
        }
        if (_tabsOpened.Count > 3) return;

        if (addNewTabTemp != null) Destroy(addNewTabTemp);
        GameObject newTab = Instantiate(tabsTemplate, tabsbox.transform);
        newTab.SetActive(true);
        _tabsOpened.Add(newTab);
        
        GameObject addNewTabButton = Instantiate(addNewTab, tabsbox.transform);
        addNewTabTemp = addNewTabButton;
        addNewTabTemp.SetActive(true);

        ChangeAlpha(0, newTab);

        newTab.GetComponentInChildren<TextMeshProUGUI>().text
            = shortCutButton.siteName;
        
        EventTrigger eventTrigger = newTab.AddComponent<EventTrigger>();
        EventTrigger.Entry onClick = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerClick
        };
        
        EventTrigger.Entry onEnter = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter
        };
        
        EventTrigger.Entry onExit = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerExit
        };

        onClick.callback.AddListener((eventData) => OpenPage(tab, shortCutButton));
        onEnter.callback.AddListener((AbstractEventData) =>
        {
            ChangeAlpha(0.5f, newTab);

        });
        onExit.callback.AddListener((AbstractEventData) =>
        {
            ChangeAlpha(0, newTab);
        });
        
        eventTrigger.triggers.Add(onClick);
        eventTrigger.triggers.Add(onEnter);
        eventTrigger.triggers.Add(onExit);
    }

    private void OpenPage(GameObject tab, ShortCutButton shortCutButton)
    {
        if (browserAnimation.IsActive()) return;
        shortcuts.TryGetValue(tab, out var tabsSite);
        {
            if (tabsSite == null)
            {
                Debug.Log("tabsSite is null");
                return;
            }

            url.text = shortCutButton.url;
            tabsSite.SetActive(true);
            tabsSite.GetComponent<CanvasGroup>().alpha = 0;
            
            browserAnimation = openedPage.GetComponent<CanvasGroup>().
                DOFade(0, 0.2f).OnComplete((() =>
                {
                    browserAnimation = tabsSite.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

                    openedPage.GetComponent<CanvasGroup>().interactable = false;
                    openedPage.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    
                    openedPage = tabsSite;
                    
                    tabsSite.GetComponent<CanvasGroup>().interactable = true;
                    tabsSite.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }));
        }
    }
    
    public void OnShortcutClick(ShortCutButton shortcut)
    {
        AddNewTab(shortcut.gameObject, shortcut);

        if (shortcut.site == null)
        {
            Debug.Log("shortcut site is null");
            return;
        }
        
        if (shortcuts.ContainsKey(shortcut.gameObject)) return;
        shortcuts.Add(shortcut.gameObject, shortcut.site);
    }

    public void OnHomeClick(GameObject mainPage)
    {
        if (browserAnimation.IsActive()) return;

        mainPage.SetActive(true);
        mainPage.GetComponent<CanvasGroup>().alpha = 0;
        url.text = "https//www.kted.net/main_page";
            
        browserAnimation = openedPage.GetComponent<CanvasGroup>().
            DOFade(0, 0.3f).OnComplete((() =>
            {
                browserAnimation = mainPage.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

                openedPage.GetComponent<CanvasGroup>().interactable = false;
                openedPage.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    
                openedPage = mainPage;
                    
                mainPage.GetComponent<CanvasGroup>().interactable = true;
                mainPage.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }));
    }
    
    private void ChangeAlpha(float value, GameObject chat)
    {
        var color = chat.GetComponent<Image>().color;
        color.a = value;
        chat.GetComponent<Image>().color = color;
    }
}
