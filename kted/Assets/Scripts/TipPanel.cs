using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TipPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI tipHeader;
    [SerializeField] private TextMeshProUGUI tipTextLabel;

    private bool fadeIn = true;

    private void Start()
    {
        _canvasGroup.alpha = 0;
    }

    public void tipPanelPopUp(string headerText, string tipText)
    {
        tipHeader.text = headerText;
        tipTextLabel.text = tipText;
        tipPanelFadeInOutAnim();
    }

    private void tipPanelFadeInOutAnim()
    {
        if (fadeIn)
        {
            _canvasGroup.DOFade(1, 1).SetEase(Ease.InCubic);
            fadeIn = false;
        }
        else
        {
            _canvasGroup.DOFade(0, 1);
            fadeIn = true;
        }
    }
}
