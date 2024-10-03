using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Playlist : MonoBehaviour
{
    // Serialization
    [Header("Playlist data")] [SerializeField]
    private string _playlistName;

    [SerializeField] private string _playlistDescription;
    [SerializeField] private Sprite _playlistIcon;
    [SerializeField] private int _listenersAmount;
    [SerializeField] private bool _library;

    [Header("PlaylistSongs")] [SerializeField]
    public List<AudioClip> songs = new List<AudioClip>();

    [Header("Playlist UI")] [SerializeField]
    public GameObject playlistPanel;

    [SerializeField] private GameObject _songTemplate;
    [SerializeField] private GameObject _songsBox;
    [SerializeField] private TextMeshProUGUI _playlistUIName;
    [SerializeField] private TextMeshProUGUI _playlistUIDescription;
    [SerializeField] private Image _playlistUIIcon;
    [SerializeField] private TextMeshProUGUI _listenersUIAmount;


    // Variables
    private int _songNumber = 1;
    private Ktedtify _ktedtify;
    [HideInInspector] public List<GameObject> songsList = new List<GameObject>();


    // Code
    private void Start()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void OnEnable()
    {
        _ktedtify = FindObjectOfType<Ktedtify>();
        OnPlaylistOpen();
        Ktedtify.OnLiked.AddListener(LibraryPlaylistAdd);
        Ktedtify.OnDisLiked.AddListener(LibraryPlaylistRemove);
    }

    private void OnDisable()
    {
        Ktedtify.OnLiked.RemoveListener(LibraryPlaylistAdd);
        Ktedtify.OnDisLiked.AddListener(LibraryPlaylistRemove);
    }

    private void OnPlaylistOpen()
    {
        SongsInstantiation(songs);

        // Show playlist data
        _playlistUIDescription.text = _playlistDescription;
        _playlistUIName.text = _playlistName;
        _playlistUIIcon.sprite = _playlistIcon;
        _listenersUIAmount.text = _listenersAmount.ToString() + " слушателей на KTedtify";
    }

    private void SongsInstantiation(List<AudioClip> songsInst)
    {
        // Reset songs box
        List<GameObject> songsToRemove = new List<GameObject>(songsList);
        foreach (var song in songsToRemove)
        {
            Destroy(song);
        }

        songsList.Clear();
        _songNumber = 1;
        // Songs instantiation
        foreach (var song in songsInst)
        {
            GameObject newSong = Instantiate
                (_songTemplate, _songsBox.transform);
            newSong.SetActive(true);

            songsList.Add(newSong);
            _ktedtify._songsQueue.Add(newSong);

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
    }

    private void LibraryPlaylistAdd(GameObject addSong)
    {
        if (_library)
        {
            GameObject newSong = Instantiate
                (_songTemplate, _songsBox.transform);
            newSong.SetActive(true);

            songsList.Add(newSong);
            _ktedtify._songsQueue.Add(newSong);

            // Add AudioSource and assign AudioClip
            AudioSource audioSource = newSong.AddComponent<AudioSource>();
            audioSource.clip = addSong.GetComponent<AudioSource>().clip;
            // Song name
            newSong.GetComponentInChildren<TextMeshProUGUI>(0).text
                = addSong.name;

            // Song Duration
            TimeSpan t = TimeSpan.FromSeconds(addSong.GetComponent<AudioSource>().clip.length);
            string answer = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            newSong.GetComponentInChildren<TextMeshProUGUI>(1).text
                = answer;

            // Song number
            newSong.GetComponentInChildren<TextMeshProUGUI>(2).text
                = _songNumber.ToString();
            _songNumber++;

            _ktedtify._favouriteSongs.Add(addSong.name, false);
            _ktedtify._favouriteSongs.TryGetValue(addSong.name, out var liked);
            {
                _ktedtify._favouriteSongs[addSong.name] = liked;
            }
        }
    }
    private void LibraryPlaylistRemove(GameObject removeSong)
    {
        if (_library)
        {
            songsList.Remove(removeSong);
            Destroy(removeSong);
        }
    }
}
