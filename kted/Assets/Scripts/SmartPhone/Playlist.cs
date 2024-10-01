using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Playlist : MonoBehaviour
{
    // Serialization
    [Header("Playlist data")]
    [SerializeField] private string _playlistName;
    [SerializeField] private string _playlistDescription;
    [SerializeField] private Sprite _playlistIcon;
    [SerializeField] private int _listenersAmount;

    [Header("PlaylistSongs")] 
    [SerializeField] public List<AudioClip> songs = new List<AudioClip>();

    [Header("Playlist UI")] 
    [SerializeField] public GameObject playlistPanel;
    [SerializeField] private GameObject _songTemplate;
    [SerializeField] private GameObject _songsBox;
    [SerializeField] private TextMeshProUGUI _playlistUIName;
    [SerializeField] private TextMeshProUGUI _playlistUIDescription;
    [SerializeField] private Image _playlistUIIcon;
    [SerializeField] private TextMeshProUGUI _listenersUIAmount;

    
    // Variables
    private int _songNumber = 1;
    private List<GameObject> _songsList = new List<GameObject>();

    
    // Code
    private void OnEnable()
    {
        OnPlaylistOpen();
    }

    private void OnPlaylistOpen()
    {
        // Reset songs box
        List<GameObject> songsToRemove = new List<GameObject>(_songsList);
        foreach (var song in songsToRemove)
        {
            Destroy(song);
        }
        _songsList.Clear();
        _songNumber = 1;
        
        // Songs instantiation
        foreach (var song in songs)
        {
            GameObject newSong = Instantiate
                (_songTemplate, _songsBox.transform);
            newSong.SetActive(true);
            _songsList.Add(newSong);
            
            
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
            
            // Song number
            newSong.GetComponentInChildren<TextMeshProUGUI>(2).text
                = _songNumber.ToString();
            _songNumber++;
        }
        // Show playlist data
        _playlistUIDescription.text = _playlistDescription;
        _playlistUIName.text = _playlistName;
        _playlistUIIcon.sprite = _playlistIcon;
        _listenersUIAmount.text = _listenersAmount.ToString() + " слушателей на KTedtify";
    }
}
