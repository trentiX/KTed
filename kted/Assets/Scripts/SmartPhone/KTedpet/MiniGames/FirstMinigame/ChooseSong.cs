using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseSong : MonoBehaviour
{
    // Serialization

    [Header("List Songs")] 
    [SerializeField] public List<AudioClip> songs = new List<AudioClip>();

    [Header("List UI")]
    [SerializeField] private GameObject _songTemplate;
    [SerializeField] private GameObject _songsBox;

    // Code
    void Start()
    {
        SongsInstantiation(songs);
    }


    public void SongsInstantiation(List<AudioClip> songsInst)
    {
        // Songs instantiation
        foreach (var song in songsInst)
        {
            GameObject newSong = Instantiate
                (_songTemplate, _songsBox.transform);
            newSong.SetActive(true);


            // Add AudioSource and assign AudioClip
            AudioSource audioSource = newSong.AddComponent<AudioSource>();
            audioSource.clip = song;

            // Song name
            newSong.GetComponentInChildren<TextMeshProUGUI>(0).text
                = song.name;

            // Song Duration
            TimeSpan t = TimeSpan.FromSeconds(song.length);
            string answer = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            newSong.GetComponentInChildren<TextMeshProUGUI>(1).text
                = answer;
        }
    }
}
