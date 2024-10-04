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
    private AudioManager _audioManager;
    private FavouritePlaylist _favouritePlaylist;
    public List<GameObject> songsList = new List<GameObject>();


    // Code
    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void OnEnable()
    {
        _favouritePlaylist = FindObjectOfType<FavouritePlaylist>();
        _ktedtify = FindObjectOfType<Ktedtify>();
        OnPlaylistOpen(this);
        
        Ktedtify._onLiked.AddListener(OnLiked);
        Ktedtify._onUnLiked.AddListener(OnUnLiked);
        Ktedtify.updatePlaylist.AddListener(OnPlaylistOpen);
    }

    private void OnPlaylistOpen(Playlist playlist)
    {
        if (this != playlist) return;
        SongsInstantiation(songs);

        // Show playlist data
        _playlistUIDescription.text = _playlistDescription;
        _playlistUIName.text = _playlistName;
        _playlistUIIcon.sprite = _playlistIcon;
        _listenersUIAmount.text = _listenersAmount.ToString() + " слушателей на KTedtify";
    }

    public void SongsInstantiation(List<AudioClip> songsInst)
    {
        // Reset songs box
        List<GameObject> songsToRemove = new List<GameObject>(songsList);
        foreach (var song in songsToRemove)
        {
            Destroy(song);
        }
        songsList.Clear();
        Debug.Log("songlist cleared");

        if (_ktedtify._currPlayingPlaylist == this)
        {
            List<GameObject> songsQueueToRemove = new List<GameObject>(_ktedtify._songsQueue);
            foreach (var song in songsQueueToRemove)
            {
                Destroy(song);
            }
            _ktedtify._songsQueue.Clear();
        }
        _songNumber = 1;
        // Songs instantiation
        foreach (var song in songsInst)
        {
            GameObject newSong = Instantiate
                (_songTemplate, _songsBox.transform);
            newSong.SetActive(true);

            if (_ktedtify._currPlayingPlaylist == this)
            {
                _ktedtify._songsQueue.Add(newSong);
            }

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
            
            songsList.Add(newSong);
            Debug.Log("songlist added new song");
        }
    }

    private void OnLiked(AudioClip likedClip)
    {
        Debug.Log("OnLiked");

        if (!this.songs.Contains(likedClip)) return;
        Debug.Log("OnLiked passed");
        
        _favouritePlaylist.favouriteSongs.Add(likedClip);
        
        Debug.Log("FavSongsInstantiation");
        _favouritePlaylist.FavSongsInstantiation();
        
        _ktedtify.DuplicateCheck(songsList);
    }

    private void OnUnLiked(AudioClip unlikedClip)
    {
        Debug.Log("OnUnLiked");

        if (!this.songs.Contains(unlikedClip)) return;
        Debug.Log("OnUnLiked passed");

        _favouritePlaylist.favouriteSongs.Remove(unlikedClip);

        if (_ktedtify._currPlayingPlaylist == _favouritePlaylist)
        {
            _audioManager.StopMusic();
            _ktedtify.ObjectFade(_ktedtify._bottomPanel, 0, 0.01f);
            _ktedtify._bottomPanel.SetActive(false);
        }

        Debug.Log("FavSongsInstantiation");
        _favouritePlaylist.FavSongsInstantiation();
        
        _ktedtify.DuplicateCheck(songsList);
    }
}
