using System;
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
