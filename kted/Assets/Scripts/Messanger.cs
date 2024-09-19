using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Messanger : MonoBehaviour,IDataPersistence
{
    // Serialization
    [SerializeField] private GameObject chatTemplate;
    [SerializeField] private GameObject chatsBox;
    [FormerlySerializedAs("_canvasGroup")] [SerializeField] private CanvasGroup canvasGroup;
    
    // Variables
    [FormerlySerializedAs("MessangerIsOpen")] public bool messangerIsOpen = false;
    public static UnityEvent OnChatAdd = new UnityEvent();
    public int generalTurn = 0;
    public bool addChatAgain;
    public List<DialogueActivator> chats;
    private Tweener _messangerTweener;
    
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
        if (chats != null)
        {
            if (chats.Contains(dialogueActivator))
            {
                return;
            }
        }
        
        GameObject newChat = Instantiate(chatTemplate, chatsBox.transform);
        newChat.SetActive(true); 
        newChat.GetComponentInChildren<Image>().sprite = dialogueActivator.dialogueObject.sprite;
        newChat.GetComponentInChildren<TextMeshProUGUI>().text = dialogueActivator.dialogueObject.name;

        chats?.Add(dialogueActivator);

        if (addChatAgain)
        {
            OnChatAdd.Invoke();
            return;
        }
        generalTurn++;
    }
    
    private void CloseMessenger()
    {
        // Check if messangerTweener is null or not active before using it
        if (_messangerTweener != null && _messangerTweener.IsActive() && !_player.canMove())
        {
            return;
        }

        messangerIsOpen = false;
        _messangerTweener = canvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }

    public void OpenMessenger()
    {
        // Check if messangerTweener is null or not active before using it
        if (_messangerTweener.IsActive())
        {
            return;
        }

        messangerIsOpen = true;
        _messangerTweener = canvasGroup.DOFade(1, 1f).OnComplete((() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }));
    }
    
    // DATA

    public void LoadData(GameData gameData)
    {
        generalTurn = gameData.chatTurn;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.chatTurn = generalTurn;
    }
}
