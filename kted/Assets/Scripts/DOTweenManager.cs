using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DOTweenManager : MonoBehaviour
{
    //components
    [SerializeField] private SpriteRenderer[] keys;
    [SerializeField] private TextMeshProUGUI[] instructions;
    [SerializeField] private CanvasGroup prefab;
    [SerializeField] private GameObject prefabObject;
    [SerializeField] private Transform prefabMother;
    
    //keys
    private const float AnimationPlayTime = 10f;
    
    //circle
    private GameObject _circle;
    private const float CircleAnimTime = 2;

    private void Awake()
    {
        prefabObject.SetActive(true);
        foreach (var key in keys)
        {
            key.color = new Color(1f, 1f ,1,0f);
        }

        foreach (var text in instructions)
        {
            text.color = new Color(0f, 0f, 0f, 0f);
        }
    }

    private void Start()
    {
        prefab.DOFade(0, 2).SetEase(Ease.InOutQuint);
        
        foreach (var key in keys)
        {
            key.DOColor(new Color(1f, 1f, 1f, 1f), AnimationPlayTime).SetEase(Ease.InOutQuint);
        }

        foreach (var text in instructions)
        {
            text.DOColor(new Color(0f, 0f, 0f, 1f),AnimationPlayTime).SetEase(Ease.InOutQuint);
        }
    }
    
    public void StartGame()
    {
        prefab.DOFade(1, 2).SetEase(Ease.InOutQuint).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        }); 
    }
}
