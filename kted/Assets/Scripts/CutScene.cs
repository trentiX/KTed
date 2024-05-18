using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Playables;

public class CutScene : MonoBehaviour
{
    [SerializeField] private CanvasGroup prefab;
    [SerializeField] private PlayableDirector director;

    private bool onButtonCliked = false;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || onButtonCliked)
        {
            onButtonCliked = false;
            director.playableGraph.GetRootPlayable(0).SetSpeed(0);
            prefab.DOFade(1, 1).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                StartGame();
            }); 
        }
    }
    
    public void OnPointerDown()
    {
        onButtonCliked = true;
    }
}
