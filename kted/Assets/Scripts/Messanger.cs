using System;
using System.Collections;
using System.Collections.Generic;
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
    private Tweener messangerTweener;

    public List<DialogueActivator> chats;
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

    public void addNewChat(DialogueActivator dialogueActivator)
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
        
        chats.Add(dialogueActivator);
    }
    
    private void closeMessanger()
    {
        // Check if messangerTweener is null or not active before using it
        if (messangerTweener != null && messangerTweener.IsActive() && !_player.canMove())
        {
            return;
        }

        MessangerIsOpen = false;
        messangerTweener = _canvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        });
    }

    public void openMessanger()
    {
        // Check if messangerTweener is null or not active before using it
        if (messangerTweener.IsActive())
        {
            return;
        }

        MessangerIsOpen = true;
        messangerTweener = _canvasGroup.DOFade(1, 1f).OnComplete((() =>
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }));
    }
}
