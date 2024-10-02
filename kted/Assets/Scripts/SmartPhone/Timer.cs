using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] public float songDuration;  // Total duration of the current song
    [SerializeField] private Image _timerFill;   // UI Image component representing the timer fill

    [HideInInspector] public float _timePlayed;  // Elapsed time of the current song
    private bool _isPlaying;    // Flag indicating whether the song is currently playing
    private bool _songPlayed;
    private Ktedtify _ktedtify;
    private void Start()
    {
        ResetTimer();
        _ktedtify = FindObjectOfType<Ktedtify>();
    }

    private void Update()
    {
        // Update timer only if the song is playing
        if (_isPlaying && _timePlayed < songDuration)
        {
            _timePlayed += Time.deltaTime;
            TimeSpan t = TimeSpan.FromSeconds(_timePlayed);
            _ktedtify._startTime.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
            _timerFill.fillAmount = _timePlayed / songDuration;
            _songPlayed = true;
        }
        else if (_timePlayed >= songDuration)
        {
            _isPlaying = false;
            
            if (!_songPlayed) return;
            _ktedtify.SkipSong();
        }
    }

    // Method to start the timer when a new song starts
    public void StartTimer(float duration)
    {
        songDuration = duration;
        _timePlayed = 0;
        _timerFill.fillAmount = 0;
        _isPlaying = true;
    }

    // Method to reset the timer
    public void ResetTimer()
    {
        _timePlayed = 0;
        _timerFill.fillAmount = 0;
        _isPlaying = false;
    }

    // Method to pause the timer
    public void PauseTimer()
    {
        _isPlaying = false;
    }

    // Method to resume the timer
    public void ResumeTimer()
    {
        _isPlaying = true;
    }
}