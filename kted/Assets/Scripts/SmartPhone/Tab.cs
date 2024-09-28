using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    // Serialization
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI tabName;
    
    // Variables
    private Browser _browser;
    private Webpage _itsPage;

    public void InitializeTab(Sprite iconSprite, string tabNameText, Webpage goToPage, Browser browser)
    {
        icon.sprite = iconSprite;
        tabName.text = tabNameText;
        _itsPage = goToPage;
        _browser = browser;
    }
    
    private void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
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

        onClick.callback.AddListener((eventData) => OnClick());
        onEnter.callback.AddListener((AbstractEventData) =>
        {
            ChangeAlpha(0.5f, gameObject);
        });
        onExit.callback.AddListener((AbstractEventData) =>
        {
            ChangeAlpha(0, gameObject);
        });
        
        eventTrigger.triggers.Add(onClick);
        eventTrigger.triggers.Add(onEnter);
        eventTrigger.triggers.Add(onExit);
    }

    private void OnClick()
    {
        _browser.OpenPage(_itsPage);
    }

    public void CloseTab()
    {
        Destroy(gameObject);
    }

    private void ChangeAlpha(float value, GameObject chat)
    {
        var color = chat.GetComponent<Image>().color;
        color.a = value;
        chat.GetComponent<Image>().color = color;
    }
}
