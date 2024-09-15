using System;
using System.Collections;
using System.Collections.Generic;
using DataSave;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Messanger : MonoBehaviour
{
    // Serialization
    [SerializeField] private GameObject chatTemplate;
    [SerializeField] private GameObject chatsBox;
    [SerializeField] private CanvasGroup _canvasGroup;

    // Variables
    public bool MessangerIsOpen = false;
    private bool dataIsLoaded = false;
    private Tweener messangerTweener;
    public List<DialogueObject> chats = new List<DialogueObject>();

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
            closeMessanger();
        }
    }

    public void addNewChat(DialogueObject dialogueObject)
    {
        if (dialogueObject == null)
        {
            return;
        }
        
        if (chats != null)
        {
            if (chats.Contains(dialogueObject))
            {
                return;
            }
        }
        
        GameObject newChat = Instantiate(chatTemplate, chatsBox.transform);
        newChat.SetActive(true); 
        newChat.GetComponentInChildren<Image>().sprite = dialogueObject.sprite;
        newChat.GetComponentInChildren<TextMeshProUGUI>().text = dialogueObject.name;

        chats.Add(dialogueObject);
    }

    private void closeMessanger()
    {
        // Check if messangerTweener is null or not active before using it
        if (messangerTweener != null && messangerTweener.IsActive() && !_player.canMove())
        {
            return;
        }

        MessangerIsOpen = false;
        messangerTweener = _canvasGroup.DOFade(0, 1f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        });
    }

    public void openMessanger()
    {
        // Check if messangerTweener is null or not active before using it
        if (messangerTweener != null && messangerTweener.IsActive())
        {
            return;
        }

        if (!dataIsLoaded)
        {
            getDataFromStorage();
            dataIsLoaded = true;
        }

        MessangerIsOpen = true;
        messangerTweener = _canvasGroup.DOFade(1, 1f).SetEase(Ease.OutExpo).OnComplete((() =>
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }));
    }

    private void getDataFromStorage()
    {
        if(chats == null)
            return;
        
        foreach (DialogueObject chat in chats)
        {
            addNewChat(chat);
        }
    }
}
