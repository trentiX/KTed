using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Messenger : MonoBehaviour,IDataPersistence
{
    // Serialization
    [SerializeField] private GameObject chatTemplate;
    [SerializeField] private GameObject chatsBox;
    [SerializeField] private CanvasGroup canvasGroup;
    
    // Variables
    public bool messengerIsOpen = false;
    public SerializableDictionary<GameObject, DialogueActivator> chats;
    public List<DialogueActivator> chatsTemp;
    private Tweener _messengerTweener;
    
    // Instances
    private Player _player;

    // Code
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CloseMessenger();
        }
    }

    public void AddNewChat(DialogueActivator dialogueActivator)
    {
        if (chatsTemp.Contains(dialogueActivator))
            return;
        chatsTemp.Add(dialogueActivator);
        
        GameObject newChat = Instantiate(chatTemplate, chatsBox.transform);
        newChat.SetActive(true);
        newChat.GetComponentInChildren<Image>().sprite = dialogueActivator.dialogueObject.sprite;
        newChat.GetComponentInChildren<TextMeshProUGUI>().text = dialogueActivator.dialogueObject.name;
        
        if (chats.ContainsValue(dialogueActivator))
            return;
        chats?.Add(newChat, dialogueActivator);
    }

    private void LoadChats()
    {
        foreach (var chat in chats)
        {
            AddNewChat(chat.Value);
        }
    }
    
    private void CloseMessenger()
    {
        // Check if messengerTweener is null or not active before using it
        if (_messengerTweener != null && _messengerTweener.IsActive() && !_player.canMove())
        {
            return;
        }

        messengerIsOpen = false;
        _messengerTweener = canvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }

    public void OpenMessenger()
    {
        // Check if messengerTweener is null or not active before using it
        if (_messengerTweener.IsActive())
        {
            return;
        }

        messengerIsOpen = true;
        _messengerTweener = canvasGroup.DOFade(1, 1f).OnComplete((() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }));
    }
    
    // DATA

    public void LoadData(GameData gameData)
    {
        chats = gameData.chatsInStorage;
        LoadChats();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.chatsInStorage = chats;
    }
}
