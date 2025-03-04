using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongForRhythmGame : MonoBehaviour
{
    [Header("Song Data")]
    [SerializeField] private AudioSource audioSource;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private GameObject[] stars; // Список звездочек

    [HideInInspector] public int bounty; // Награда за прохождение песни
    private AudioClip song;
    private int difficulty; // Автоматически определяется в SetSongData()
    public float[] beats = {0.31f, 0.84f, 1.19f, 1.50f, 2.36f, 2.66f, 3.03f, 3.34f, 4.04f, 4.53f, 4.85f, 5.17f,
    6.10f, 6.40f, 6.71f, 7.02f, 7.67f, 8.41f, 8.74f, 9.05f, 9.81f, 10.28f, 10.58f, 10.89f,
    11.49f, 12.21f, 12.52f, 12.84f, 13.80f, 14.21f, 14.70f, 15.01f, 15.34f, 15.73f, 16.10f,
    16.59f, 17.51f, 17.82f, 18.13f, 18.50f, 18.81f, 19.12f, 19.64f, 19.95f, 20.26f, 20.57f,
    21.16f, 21.47f, 21.78f, 22.17f, 22.50f, 22.98f, 23.34f, 23.67f, 24.01f, 24.38f, 24.83f,
    25.18f, 25.51f, 25.89f, 26.21f, 26.72f, 27.12f, 27.43f, 27.74f, 28.09f, 28.62f, 29.01f,
    29.35f, 29.65f, 30.00f, 30.48f, 30.97f};

    public void SetSong(AudioClip clip)
    {
        song = clip;

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем AudioSource если его нет

        audioSource.clip = song;
        SetSongData();
    }

    private void SetSongData()
    {
        if (song == null) return;

        // Название песни
        if (nameText != null) nameText.text = song.name;

        // Длительность
        TimeSpan t = TimeSpan.FromSeconds(song.length);
        if (durationText != null) durationText.text = $"{t.Minutes:D2}:{t.Seconds:D2}";

        // Определяем сложность (например, если песня длиннее 2 минут — сложнее)
        if (song.length < 60)
        {
            difficulty = 1;
            bounty = 3;            
        }
        else if (song.length < 120)
        {
            difficulty = 2;  
            bounty = 5;                     
        }
        else
        {
            difficulty = 3;
            bounty = 10;                       
        }

        // Выставляем звезды по сложности
        for (int i = 0; i < difficulty; i++)
        {
            if (stars.Length > i)
                stars[i].GetComponent<RawImage>().color = new Color32(255, 200, 0, 255);
        }
    }

    public AudioClip GetSong()
    {
        return song;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
}
