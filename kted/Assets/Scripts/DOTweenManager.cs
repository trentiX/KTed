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
    [SerializeField] private CanvasGroup prefab;

    private void Start()
    {
        prefab.DOFade(0, 2);
    }
    
    public void StartGame()
    {
        prefab.DOFade(1, 2).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        }); 
    }
}
