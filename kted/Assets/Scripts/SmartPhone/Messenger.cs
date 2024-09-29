using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Messenger : MonoBehaviour,IDataPersistence
{
    // Serialization
    [Header("Chats:")]
    [SerializeField] private GameObject chatTemplate;
    [SerializeField] private GameObject chatsBox;
    
    [Header("Messages:")]
    [SerializeField] private GameObject messageTemplate;
    [SerializeField] private GameObject responseTemplate;
    [SerializeField] private GameObject messageBox;
    [SerializeField] private GameObject messageImage;
    [SerializeField] private GameObject ktedGramTextBig;
    
    [Header("Other:")]
    [SerializeField] private CanvasGroup canvasGroup;
    
    // Variables
    public bool messengerIsOpen = false;
    public SerializableDictionary<GameObject, DialogueActivator> chats;
    public SerializableDictionary<DialogueActivator, Response> responses;
    public List<DialogueActivator> chatsTemp;
    public List<GameObject> messagesTemp;
    private Tweener _messengerTweener;
    
    // Instances
    private Player _player;

    // Code
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    public void AddNewChat(DialogueActivator dialogueActivator)
    {
        if (chatsTemp.Contains(dialogueActivator))
            return;
        chatsTemp.Add(dialogueActivator);
        
        // new chat instantiation
        GameObject newChat = Instantiate(chatTemplate, chatsBox.transform);
        newChat.SetActive(true);
                
        newChat.GetComponentInChildren<Image>().sprite = dialogueActivator.dialogueObject.sprite;
        newChat.GetComponentInChildren<TextMeshProUGUI>(0).text = dialogueActivator.dialogueObject.name;
        newChat.GetComponentInChildren<TextMeshProUGUI>(1).text = dialogueActivator.dialogueObject.DialogueRus[0];

        // new chat triggers
        EventTrigger eventTrigger = newChat.AddComponent<EventTrigger>();
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

        onClick.callback.AddListener((eventData) => ShowMessages(eventData, newChat, dialogueActivator,dialogueActivator.dialogueObject, true));
        onEnter.callback.AddListener((AbstractEventData) =>
        {
            ChangeAlpha(0.2f, newChat);

        });
        onExit.callback.AddListener((AbstractEventData) =>
        {
            ChangeAlpha(0, newChat);
        });
        
        eventTrigger.triggers.Add(onClick);
        eventTrigger.triggers.Add(onEnter);
        eventTrigger.triggers.Add(onExit);
        
        // new chat add to messenger
        if (chats.ContainsValue(dialogueActivator))
            return;
        chats?.Add(newChat, dialogueActivator);
    }

    private void ShowMessages(BaseEventData eventData, GameObject chat, DialogueActivator dialogueActivator, DialogueObject dialogueObject, bool newChat)
    {
        if (newChat)
        {
            // Clear the old messages
            foreach (var message in messagesTemp)
            {
                Destroy(message);
            }
            messagesTemp.Clear();
            ktedGramTextBig.SetActive(false);
        }
        
        // Set the image for the current chat
        messageImage.SetActive(true);
        messageImage.GetComponent<Image>().sprite = dialogueObject.sprite;

        for (int i = 0; i < dialogueObject.DialogueRus.Length; i++)
        {
            GameObject newMessage = Instantiate(messageTemplate, messageBox.transform);
            newMessage.SetActive(true);
            newMessage.GetComponentInChildren<TextMeshProUGUI>().text = dialogueObject.DialogueRus[i];

            messagesTemp.Add(newMessage);
            LayoutRebuilder.ForceRebuildLayoutImmediate(newMessage.GetComponent<RectTransform>());
            
            if (i == dialogueObject.DialogueRus.Length - 1 && dialogueObject.HasResponses) break;
        }

        Response myResponse = dialogueActivator.chooseResponse;
        if (dialogueObject.HasResponses)
        {
            if (!responses.ContainsKey(dialogueActivator))
            {
                responses.Add(dialogueActivator, dialogueActivator.chooseResponse);
            }
            else
            {
                responses.TryGetValue(dialogueActivator, out var response);
                {
                    myResponse = response;
                }
            }
            
            GameObject newResponse = Instantiate(responseTemplate, messageBox.transform);
            newResponse.SetActive(true);
            newResponse.GetComponentInChildren<TextMeshProUGUI>().text = myResponse.ResponseText;
            
            messagesTemp.Add(newResponse);
            
            // After adding the new response, force its layout rebuild
            LayoutRebuilder.ForceRebuildLayoutImmediate(newResponse.GetComponent<RectTransform>());
            
            ShowMessages(eventData, chat, dialogueActivator, myResponse.DialogueObject, false);
        }
        RebuildLayout(chat);
    }
    
    private void LoadChats()
    {
        foreach (var chat in chats)
        {
            AddNewChat(chat.Value);
        }
    }

    private void SaveMessages()
    {
        foreach (var chat in chatsTemp)
        {
            if (!responses.ContainsKey(chat) && chat.dialogueObject.HasResponses)
            {
                responses.Add(chat, chat.chooseResponse);
            }
        }
    }
    
    private void ChangeAlpha(float value, GameObject chat)
    {
        var color = chat.GetComponent<RawImage>().color;
        color.a = value;
        chat.GetComponent<RawImage>().color = color;
    }

    private void RebuildLayout(GameObject chat)
    {
        // Rebuild the layout for the entire message box to ensure flexible backgrounds work
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)messageBox.transform);

        // Rebuild the layout for chat
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chat.transform);

        // Rebuild parent layout groups
        LayoutGroup[] parentLayoutGroups = chat.GetComponentsInParent<LayoutGroup>();
        foreach (LayoutGroup group in parentLayoutGroups)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)group.transform);
        }
    }
    
    // DATA

    public void LoadData(GameData gameData)
    {
        chats = gameData.chatsInStorage;
        responses = gameData.responsesInStorage;
        LoadChats();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.chatsInStorage = chats;
        gameData.responsesInStorage = responses;
        SaveMessages();
    }
}

public static class ExtensionFunction
{
    public static T GetComponentInChildren<T>(this GameObject gameObject, int index)
    {
        return gameObject.transform.GetChild(index).GetComponent<T>();
    }
}