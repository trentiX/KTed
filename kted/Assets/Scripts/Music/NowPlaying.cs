using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class NowPlaying : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject nowPlaying;

    private Tweener nowPlayingAnimation;

    private void Start()
    {
        nowPlaying.SetActive(false);
    }

    public void nowPlayingAnim(string nameOfSong)
    {
        nowPlaying.SetActive(true);
        StartCoroutine(musicNameAnimation(nameOfSong));
    }

    private IEnumerator musicNameAnimation(string songName)
    {
        if (!nowPlayingAnimation.IsActive())
        {
            // Устанавливаем новый текст
            text.text = "Сейчас играет... " + songName;

            // Плавно показываем новый текст
            text.alpha = 0;
            nowPlayingAnimation = text.DOFade(1, 2).SetEase(Ease.OutCubic);

            // Ждем 5 секунд перед началом затухания
            yield return new WaitForSeconds(5);

            // Плавно скрываем текст
            nowPlayingAnimation = text.DOFade(0, 2).SetEase(Ease.InCubic).OnComplete(() =>
            {
                nowPlaying.SetActive(false);
            });
        }
        else
        {
            nowPlayingAnimation = text.DOFade(0, 2).SetEase(Ease.InCubic).OnComplete(() =>
            {
                StartCoroutine(musicNameAnimation(songName));
            });
        }
    }
}