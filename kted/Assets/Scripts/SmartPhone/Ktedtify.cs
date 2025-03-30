using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;

public class Ktedtify : MonoBehaviour
{
    // Serialization
    [SerializeField] public GameObject _bottomPanel;
    [SerializeField] public TextMeshProUGUI _startTime;
    [SerializeField] private TextMeshProUGUI _endTime;
    [SerializeField] private TextMeshProUGUI _songName;
    [SerializeField] private GameObject _likeButton;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _homePage;
    
    // Variables
    public static Ktedtify instance;
    public List<GameObject> _songsQueue;
    public Playlist _currPlaylist;
    public Playlist _currPlayingPlaylist;
    private GameObject _currAudioClip;
    private AudioManager _audioManager;
    private FavouritePlaylist _favouritePlaylist;
    private Timer _timer;
    public bool _songPlaying;
    public static UnityEvent<AudioClip> _onLiked = new UnityEvent<AudioClip>();
    public static UnityEvent<AudioClip> _onUnLiked = new UnityEvent<AudioClip>();
    public static UnityEvent<Playlist> updatePlaylist = new UnityEvent<Playlist>();
    private bool _liked;
    public int songIndexInHistory;

    // Code   
    private void Start()
    {
        instance = this;
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        _songsQueue = new List<GameObject>();
        _audioManager = FindObjectOfType<AudioManager>();
        _favouritePlaylist = FindObjectOfType<FavouritePlaylist>();
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
        _currPlayingPlaylist = _currPlaylist;
        _songsQueue = _currPlaylist.songsList;

        _currAudioClip = _songsQueue[0];
        _songsQueue[0].GetComponent<Image>().color = new Color(0.07843f, 0.07843f, 0.07843f);
        _audioManager.StopMusic();
        _songsQueue[0].GetComponent<AudioSource>().time = 0;
        _audioManager.PlaySong(_songsQueue[0].GetComponent<AudioSource>().clip);
        _currPlayingPlaylist = _currPlaylist;

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
        
        CheckOnLiked(_songsQueue[songIndexInHistory]
            .GetComponent<AudioSource>().clip);
        DuplicateCheck(_songsQueue);
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
        _currPlayingPlaylist = _currPlaylist;
        _songsQueue = _currPlaylist.songsList;

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

        CheckOnLiked(song.GetComponent<AudioSource>().clip);
        DuplicateCheck(_songsQueue);
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
            
            CheckOnLiked(_songsQueue[songIndexInHistory + 1]
                .GetComponent<AudioSource>().clip);
            songIndexInHistory++;
            _continueButton.SetActive(false);
            _pauseButton.SetActive(true);
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
            CheckOnLiked(_songsQueue[songIndexInHistory - 1]
                .GetComponent<AudioSource>().clip);
            songIndexInHistory--;
            _continueButton.SetActive(false);
            _pauseButton.SetActive(true);
        }
    }

    public void LikeUnLike()
    {
        if (_liked)
        {
            // Unlike
            _likeButton.GetComponent<Image>().color = Color.gray;
            _onUnLiked.Invoke(_currAudioClip.GetComponent<AudioSource>().clip);
            _liked = false;
        }
        else
        {
            // Like
            _likeButton.GetComponent<Image>().color = Color.red;
            _onLiked.Invoke(_currAudioClip.GetComponent<AudioSource>().clip);
            _liked = true;
        }
    }

    public void DuplicateCheck(List<GameObject> songs)
    {
        if (songs.Count <= 1) return;

        for (int i = 0; i < songs.Count - 1; i++)  // Чтобы не выйти за границы списка
        {
            if (songs[i] == songs[i + 1])
            {
                songs.RemoveAt(i + 1);
                i--;  // Уменьшаем индекс, чтобы не пропустить элемент после удаления
            }
        }
    }

    private bool CheckOnLiked(AudioClip clip)
    {
        if (_favouritePlaylist.favouriteSongs.Contains(clip))
        {
            _likeButton.GetComponent<Image>().color = Color.red;
            _liked = true;
            return true;
        }
        else
        {
            _likeButton.GetComponent<Image>().color = Color.gray;
            _liked = false;
            return false;
        }
    }
    private void ChangeAlpha(float value, GameObject gameObject)
    {
        Image songImage = gameObject.GetComponent<Image>();
        Color color = songImage.color; // Get the current color
        color.a = value; // Set alpha to 0
        songImage.color = color; // Assign the modified color back
    }

    public void ObjectFade(GameObject obj, float value, float dur)
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
}
