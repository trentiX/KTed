using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BeautifulButtons : MonoBehaviour
{
    public List<GameObject> allButtons; // Assign all buttons from the Inspector, or find them dynamically
    
    void Start()
    {
        // If you don't manually assign buttons, you can find all buttons by tag or type
        // For example: allButtons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Button"));

        foreach (GameObject button in allButtons)
        {
            AddHoverEffect(button);
        }
    }

    void AddHoverEffect(GameObject button)
    {
        // Initialize the EventTrigger component for each button
        EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = button.AddComponent<EventTrigger>();
        }

        // Add PointerEnter event
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((eventData) => OnPointerEnter(button));
        eventTrigger.triggers.Add(entry);

        // Add PointerExit event
        EventTrigger.Entry exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exitEntry.callback.AddListener((eventData) => OnPointerExit(button));
        eventTrigger.triggers.Add(exitEntry);
        
        EventTrigger.Entry downEntry = new EventTrigger.Entry() { eventID = EventTriggerType.PointerDown };
        downEntry.callback.AddListener((eventData) => OnPointerDown(button));
        eventTrigger.triggers.Add(downEntry);
    }

    private void OnPointerEnter(GameObject button)
    {
        if (button.GetComponent<UnityEngine.UI.Button>().interactable)
        {
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = "--> " + buttonText.text; // Add arrow on hover
            }   
        }
    }

    private void OnPointerExit(GameObject button)
    {
        if (button.GetComponent<UnityEngine.UI.Button>().interactable)
        {
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null && buttonText.text.StartsWith("-->"))
            {
                buttonText.text = buttonText.text.Substring(4); // Remove the arrow on exit
            }
        }
    }

    private void OnPointerDown(GameObject button)
    {
        if (button.GetComponent<UnityEngine.UI.Button>().interactable)
        {
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null && buttonText.text.StartsWith("--> "))
            {
                buttonText.text = buttonText.text.Substring(4); // Remove the arrow on exit
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject button in allButtons)
            {
                OnPointerExit(button);
            }
        }
    }
}
