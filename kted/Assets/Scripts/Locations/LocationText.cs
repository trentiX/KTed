using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LocationText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject locName;

    private Coroutine activeAnimation;

    private void Start()
    {
        locName.SetActive(false);
    }

    public void locNameAnim(string nameOfLocation)
    {
        locName.SetActive(true);

        // Stop any active animation
        if (activeAnimation != null)
        {
            StopCoroutine(activeAnimation);
        }

        activeAnimation = StartCoroutine(locationNameAnimation(nameOfLocation));
    }

    private IEnumerator locationNameAnimation(string locationName)
    {
        // Kill any existing text animations
        text.DOKill();

        text.DOFade(0, 0.01f).SetEase(Ease.OutCubic);
        text.text = "Это локация..." + locationName;
        text.DOFade(1, 3).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(2);

        text.DOFade(0, 3).SetEase(Ease.InCubic).OnComplete(() =>
        {
            locName.SetActive(false);
        });

        activeAnimation = null; // Clear the active animation reference
    }
}