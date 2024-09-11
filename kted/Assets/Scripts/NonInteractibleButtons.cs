using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NonInteractibleButtons : MonoBehaviour
{
    private AudioManager _audioManager;
    
    public List<GameObject> allButtons; // Assign all buttons from the Inspector, or find them dynamically
    
    void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        // If you don't manually assign buttons, you can find all buttons by tag or type
        // For example: allButtons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Button"));

        foreach (GameObject button in allButtons)
        {
            PlaySound(button);
        }
    }

    void PlaySound(GameObject button)
    {
        EventTrigger eventTrigger = button.GetComponent<EventTrigger>();
        if (eventTrigger == null) 
        { 
            eventTrigger = button.AddComponent<EventTrigger>();
        }
        // Add PointerEnter event
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entry.callback.AddListener((eventData) => OnPointerEnter(button));
        eventTrigger.triggers.Add(entry);
    }

    private void OnPointerEnter(GameObject button)
    {
        if (button.GetComponent<UnityEngine.UI.Button>().interactable)
            _audioManager.SFXSound();
    }
}
