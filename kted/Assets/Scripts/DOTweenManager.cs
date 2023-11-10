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
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabMother;
    
    //keys
    private const float AnimationPlayTime = 10f;
    
    //circle
    private GameObject _circle;
    private const float CircleAnimTime = 2.5f;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _circle = Instantiate(prefab, prefabMother.position, prefabMother.rotation, prefabMother);
            _circle.transform.localScale = new Vector3(21f, 21f, 21f);
        }
        
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            _circle = Instantiate(prefab, prefabMother.position, prefabMother.rotation, prefabMother);
            _circle.transform.localScale = new Vector3(0f, 0f, 0f);
        }
        
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
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _circle.transform.DOScale(new Vector3(0f, 0f, 0f), CircleAnimTime).SetEase(Ease.InOutQuint);
            Destroy(prefab, CircleAnimTime);
        }
        
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
        _circle.transform.DOScale(new Vector3(21f, 21f, 21f), CircleAnimTime).SetEase(Ease.InOutQuint).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        }); 
        
    }
}
