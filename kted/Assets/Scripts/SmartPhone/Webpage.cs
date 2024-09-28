using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Webpage : MonoBehaviour
{
    // Serialized
    [SerializeField] public string url;
    
    // Variables
    private Browser _browser;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _browser = FindObjectOfType<Browser>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        _browser.currPage = this;
        _canvasGroup.DOFade(1, 0.2f);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        _browser.prevPage = this;
        _canvasGroup.DOFade(0, 0.2f);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
