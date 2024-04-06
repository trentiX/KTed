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
    private void Start()
    {
        locName.SetActive(false);
    }

    public void locNameAnim(string nameOfLocation)
    {
        locName.SetActive(true);
        StartCoroutine(locationNameAnimation(nameOfLocation));
    }

    public IEnumerator locationNameAnimation(string locationName)
    {
        text.DOFade(0, 0.01f).SetEase(Ease.OutCubic);
        text.text = "Это локация..." + locationName;
        text.DOFade(1, 3).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(2);
        
        text.DOFade(0, 3).SetEase(Ease.InCubic).OnComplete(() =>
        {
            locName.SetActive(false);
        }); 
    }
}