using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Ktedtify : MonoBehaviour, IDataPersistence
{
    // Serialization
    [SerializeField] private GameObject _bottomPanel;
    [SerializeField] public TextMeshProUGUI _startTime;
    [SerializeField] private TextMeshProUGUI _endTime;
    [SerializeField] private TextMeshProUGUI _songName;
    [SerializeField] private GameObject _likeButton;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _homePage;
    
    // Variables
    public List<GameObject> _songsQueue;
    public SerializableDictionary<string, bool> _favouriteSongs;
    private Playlist _currPlaylist;
    private GameObject _currAudioClip;
    private AudioManager _audioManager;
    private Timer _timer;
    private bool _songPlaying;
    private bool _songLiked;
    public static UnityEvent<GameObject> OnLiked = new UnityEvent<GameObject>();
    public static UnityEvent<GameObject> OnDisLiked = new UnityEvent<GameObject>();
    public int songIndexInHistory;

    // Code   
    private void Start()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        _songsQueue = new List<GameObject>();
        _audioManager = FindObjectOfType<AudioManager>();
        _timer = FindObjectOfType<Timer>();
        
        // Bottom panel setup
        ObjectFade(_bottomPanel, 0, 0.01f);
        _bottomPanel.SetActive(false);
    }

    public void PlaylistOpen(Playlist playlist)
    {
        // In case from home to playlist
        ObjectFade(_homePage, 1, 0.3f);
        _homePage.SetActive(false);

        // In case from playlist to playlist
        if (_currPlaylist != null) ObjectFade(_currPlaylist.gameObject, 0, 0.3f);
        
        // Open playlist
        ObjectFade(playlist.playlistPanel, 1, 0.3f);
        _currPlaylist = playlist;
    }

    public void HomePageOpen(GameObject homePage)
    {
        ObjectFade(_homePage, 1, 0.3f);
        _homePage.SetActive(true);
        
        if (_currPlaylist != null) ObjectFade(_currPlaylist.gameObject, 0, 0.3f);
    }

    public void PlayPlaylist()
    {
        if (_currAudioClip != null) _currAudioClip.GetComponent<Image>().color = new Color(0.13f, 0.13f, 0.13f);
        // Play song from the start
        _currAudioClip = _songsQueue[0];
        _songsQueue[0].GetComponent<Image>().color = new Color(0.07843f, 0.07843f, 0.07843f);
        _audioManager.StopMusic();
        _songsQueue[0].GetComponent<AudioSource>().time = 0;
        _audioManager.PlaySong(_songsQueue[0].GetComponent<AudioSource>().clip);

        // Timer
        _timer.StartTimer(_songsQueue[0].GetComponent<AudioSource>().clip.length);

        _songPlaying = true;

        // Get song's index
        songIndexInHistory = int.Parse(_songsQueue[0].GetComponentInChildren<TextMeshProUGUI>(2).text) - 1;

        // Change bottom panel's data
        // Name
        _songName.text = _songsQueue[0].GetComponentInChildren<TextMeshProUGUI>(0).text;
        // Duration
        _endTime.text = _songsQueue[0].GetComponentInChildren<TextMeshProUGUI>(1).text;
        
        _pauseButton.SetActive(true);
        _continueButton.SetActive(false);
        
        // Bottom panel setup
        _bottomPanel.SetActive(true);
        ObjectFade(_bottomPanel, 1, 0.3f);
    }
    public void PlaySong(GameObject song)
    {
        if (song == null) return;

        if (_currAudioClip != null) _currAudioClip.GetComponent<Image>().color = new Color(0.13f, 0.13f, 0.13f);
        // Play song from the start
        _currAudioClip = song;
        song.GetComponent<Image>().color = new Color(0.07843f, 0.07843f, 0.07843f);
        _audioManager.StopMusic();
        song.GetComponent<AudioSource>().time = 0;
        _audioManager.PlaySong(song.GetComponent<AudioSource>().clip);

        // Timer
        _timer.StartTimer(song.GetComponent<AudioSource>().clip.length);

        _songPlaying = true;

        // Get song's index
        songIndexInHistory = int.Parse(song.GetComponentInChildren<TextMeshProUGUI>(2).text) - 1;

        // Change bottom panel's data
        // Name
        _songName.text = song.GetComponentInChildren<TextMeshProUGUI>(0).text;
        // Duration
        _endTime.text = song.GetComponentInChildren<TextMeshProUGUI>(1).text;
        
        _pauseButton.SetActive(true);
        _continueButton.SetActive(false);
        
        // Bottom panel setup
        _bottomPanel.SetActive(true);
        ObjectFade(_bottomPanel, 1, 0.3f);

        if (LikedCheck(song))
        {
            _songLiked = true;
            _likeButton.GetComponent<Image>().color = Color.red;
        }
        else
        {
            _songLiked = false;
            _likeButton.GetComponent<Image>().color = Color.gray;
        }
    }

    public void StopSong()
    {
        if (_songPlaying)
        {
            // Pause song
            _audioManager.PauseSong();
            _songPlaying = false;
            _timer.PauseTimer();
            _pauseButton.SetActive(false);
            _continueButton.SetActive(true);
        }
    }
    
    public void ContinueSong()
    {
        // Resume the song if it was paused before
        if (_timer._timePlayed > 0 && !_songPlaying)
        {
            _audioManager.ResumeSong();  // Assuming AudioManager has a ResumeSong method
            _timer.ResumeTimer();
            _continueButton.SetActive(false);
            _pauseButton.SetActive(true);
        }
    }
    
    public void SkipSong()
    {
        if (songIndexInHistory + 1 < _songsQueue.Count)
        {
            _songName.text = _songsQueue[songIndexInHistory + 1].GetComponentInChildren<TextMeshProUGUI>(0).text;
            _endTime.text = _songsQueue[songIndexInHistory + 1].GetComponentInChildren<TextMeshProUGUI>(1).text;
            if (_currAudioClip != null) _currAudioClip.GetComponent<Image>().color = new Color(0.13f, 0.13f, 0.13f);
            
            _currAudioClip = _songsQueue[songIndexInHistory + 1];
            _currAudioClip.GetComponent<Image>().color = new Color(0.07843f, 0.07843f, 0.07843f);

            _audioManager.StopMusic();
            _audioManager.PlaySong(_songsQueue[songIndexInHistory + 1]
                .GetComponent<AudioSource>().clip);
            _songPlaying = true;
            ChangeAlpha(1, _songsQueue[songIndexInHistory + 1]);
            
            // Update Timer
            _timer.StartTimer(_songsQueue[songIndexInHistory + 1]
                .GetComponent<AudioSource>().clip.length);

            // Update song index
            if (LikedCheck(_songsQueue[songIndexInHistory + 1]))
            {
                _songLiked = true;
                _likeButton.GetComponent<Image>().color = Color.red;
            }
            else
            {
                _songLiked = false;
                _likeButton.GetComponent<Image>().color = Color.gray;
            }
            songIndexInHistory++;
        }
    }

    public void PrevSong()
    {
        if (songIndexInHistory - 1 >= 0)
        {
            _songName.text = _songsQueue[songIndexInHistory - 1].GetComponentInChildren<TextMeshProUGUI>(0).text;
            _endTime.text = _songsQueue[songIndexInHistory - 1].GetComponentInChildren<TextMeshProUGUI>(1).text;
            if (_currAudioClip != null) _currAudioClip.GetComponent<Image>().color = new Color(0.13f, 0.13f, 0.13f);
            
            _currAudioClip = _songsQueue[songIndexInHistory - 1];
            _currAudioClip.GetComponent<Image>().color = new Color(0.07843f, 0.07843f, 0.07843f);

            _audioManager.StopMusic();
            _audioManager.PlaySong(_songsQueue[songIndexInHistory - 1]
                .GetComponent<AudioSource>().clip);
            _songPlaying = true;
            ChangeAlpha(1, _songsQueue[songIndexInHistory - 1]);
            
            // Update Timer
            _timer.StartTimer(_songsQueue[songIndexInHistory - 1]
                .GetComponent<AudioSource>().clip.length);

            // Update song index
            if (LikedCheck(_songsQueue[songIndexInHistory - 1]))
            {
                _songLiked = true;
                _likeButton.GetComponent<Image>().color = Color.red;
            }
            else
            {
                _songLiked = false;
                _likeButton.GetComponent<Image>().color = Color.gray;
            }
            songIndexInHistory--;
        }
    }

    public void LikeUnlikeSong()
    {
        if (_songLiked)
        {
            // Unlike
            _favouriteSongs[_currAudioClip.name] = false;
            _likeButton.GetComponent<Image>().color = Color.gray;
            _songLiked = false;
            OnDisLiked.Invoke(_currAudioClip);
        }
        else
        {
            // Like
            _favouriteSongs[_currAudioClip.name] = true;
            _likeButton.GetComponent<Image>().color = Color.red;
            _songLiked = true;
            OnLiked.Invoke(_currAudioClip);
        }
    }

    private void ChangeAlpha(float value, GameObject gameObject)
    {
        Image songImage = gameObject.GetComponent<Image>();
        Color color = songImage.color; // Get the current color
        color.a = value; // Set alpha to 0
        songImage.color = color; // Assign the modified color back
    }

    private bool LikedCheck(GameObject clip)
    {
        _favouriteSongs.TryGetValue(clip.name, out var liked);
        {
            return liked;
        }
    }

    private void ObjectFade(GameObject obj, float value, float dur)
    {
        obj.GetComponent<CanvasGroup>().DOFade(value, dur);
        if (value == 0)
        {
            obj.GetComponent<CanvasGroup>().interactable = false;
            obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            obj.GetComponent<CanvasGroup>().interactable = true;
            obj.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    
    // DATA
    public void LoadData(GameData gameData)
    {
        _favouriteSongs = gameData.favouriteSongs;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.favouriteSongs = _favouriteSongs;
    }
}
