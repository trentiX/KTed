using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class NowPlaying : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject nowPlaying;
    private void Start()
    {
        nowPlaying.SetActive(false);
    }

    public void nowPlayingAnim(string nameOfSong)
    {
        nowPlaying.SetActive(true);
        StartCoroutine(musicNameAnimation(nameOfSong));
    }

    public IEnumerator musicNameAnimation(string songName)
    {
        text.DOFade(0, 0.01f).SetEase(Ease.OutCubic);
        text.text = "Сейчас играет... " + songName;
        text.DOFade(1, 3).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(5);
        
        text.DOFade(0, 3).SetEase(Ease.InCubic).OnComplete(() =>
        {
            nowPlaying.SetActive(false);
        }); 
    }
}
