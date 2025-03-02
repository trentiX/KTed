using System.Collections.Generic;
using UnityEngine;

public class ChooseSong : MonoBehaviour
{
    [Header("List Songs")] 
    [SerializeField] private List<AudioClip> songs = new List<AudioClip>();

    [Header("List UI")]
    [SerializeField] private GameObject _songTemplate;
    [SerializeField] private Transform _songsBox;

    void Start()
    {
        SongsInstantiation();
    }

    private void SongsInstantiation()
    {
        foreach (var song in songs)
        {
            GameObject newSong = Instantiate(_songTemplate, _songsBox);
            newSong.SetActive(true);

            // Получаем компонент SongForRhythmGame
            SongForRhythmGame songComponent = newSong.GetComponent<SongForRhythmGame>();
            if (songComponent == null)
                songComponent = newSong.AddComponent<SongForRhythmGame>();

            // Устанавливаем песню
            songComponent.SetSong(song);
        }
    }
}
