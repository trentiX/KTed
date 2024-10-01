using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ktedtify : MonoBehaviour
{
    // Serialization
    [SerializeField] private Image _timerFill;
    [SerializeField] private TextMeshProUGUI _startTime;
    [SerializeField] private TextMeshProUGUI _endTime;
    [SerializeField] private TextMeshProUGUI _songName;
    [SerializeField] private TextMeshProUGUI _songAuthor;
    [SerializeField] private GameObject _homePage;
    
    // Variables
    private float _timePlayed;
    private List<AudioClip> _songsQueue = new List<AudioClip>();
    private Playlist _currPlaylist;
    private AudioClip _currAudioClip;
    private AudioManager _audioManager;
    private bool _songPlaying;
    private int _songIndexInHistory;

    // Code   
    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlaylistOpen(Playlist playlist)
    {
        if (_currPlaylist != null) _currPlaylist.gameObject.SetActive(false);
        _homePage.SetActive(false);
        
        foreach (var song in playlist.songs)
        {
            _songsQueue.Add(song);
        }
        
        playlist.playlistPanel.SetActive(true);
        _currPlaylist = playlist;
    }

    public void HomePageOpen(GameObject homePage)
    {
        homePage.SetActive(true);
        _currPlaylist.gameObject.SetActive(false);
    }
    
    public void PlayPauseSong(GameObject song)
    {
        if (_songPlaying)
        {
            // Pause song
            _audioManager.PauseSong();
            _songPlaying = false;
        }
        else
        {
            if (song == null) return;
            // Play song
            _audioManager.PlaySong(song.GetComponent<AudioSource>().clip);
            _songIndexInHistory = int.Parse
                (song.GetComponentInChildren<TextMeshProUGUI>(2).text) - 1;

            _songPlaying = true;
        }
    }

    public void SkipSong()
    {
        _audioManager.PlaySong(_songsQueue[_songIndexInHistory + 1]);
        _songPlaying = true;
    }

    public void PrevSong()
    {
        _audioManager.PlaySong(_songsQueue[_songIndexInHistory - 1]);
        _songPlaying = true;
    }
}
