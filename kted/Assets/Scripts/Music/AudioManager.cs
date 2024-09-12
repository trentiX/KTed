using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource ambientSource;
    [SerializeField] List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> easterEggClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> ambientClips = new List<AudioClip>();

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";
    public const string AMBIENT_KEY = "ambientVolume";

    private static AudioManager _instance;
    private NowPlaying _nowPlaying;
    private string songName;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    _instance = obj.AddComponent<AudioManager>();
                }
            }
            return _instance;
        }
    }


    private void Awake()
    {
        _nowPlaying = FindObjectOfType<NowPlaying>();
    }

    public void SFXSound()
    {
        AudioClip clip = sfxClips[0];
        
        sfxSource.PlayOneShot(clip);
    }

    public void phoneRing()
    {
        songName = "Рингтон телефона";
        AudioClip clip = musicClips[4];

        if (musicSource.isPlaying)
        {
            return;
        }
        
        _nowPlaying.nowPlayingAnim(songName);
        musicSource.PlayOneShot(clip);
    }

    public void EasterEggSound()
    {
        AudioClip clip = easterEggClips[UnityEngine.Random.Range(0, easterEggClips.Count)];
        
        sfxSource.PlayOneShot(clip);
    }

    public void PlayFirstSong()
    {
        songName = "Балкадиша";
        AudioClip clip = musicClips[0];

        if (musicSource.isPlaying)
        {
            return;
        }
        
        _nowPlaying.nowPlayingAnim(songName);
        musicSource.PlayOneShot(clip);
    }
    public void PlaySecSong()
    {
        songName = "Ағаш аяқ";
        AudioClip clip = musicClips[1];

        if (musicSource.isPlaying)
        {
            return;
        }
        
        _nowPlaying.nowPlayingAnim(songName);
        musicSource.PlayOneShot(clip);
    }
    public void PlayThirdSong()
    {
        songName = "Дударай";
        AudioClip clip = musicClips[2];

        if (musicSource.isPlaying)
        {
            return;
        }
        
        _nowPlaying.nowPlayingAnim(songName);
        musicSource.PlayOneShot(clip);
    }
    
    public void PlayFourthSong()
    {
        songName = "Үш дос";
        AudioClip clip = musicClips[3];

        if (musicSource.isPlaying)
        {
            return;
        }
        
        _nowPlaying.nowPlayingAnim(songName);
        musicSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void StopSfx()
    {
        sfxSource.Stop();
    }
    
    public void AmbientSound()
    {
        AudioClip clip = ambientClips[UnityEngine.Random.Range(0, ambientClips.Count)];
        
        ambientSource.PlayOneShot(clip);
    }

    void LoadValue()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float ambientVolume = PlayerPrefs.GetFloat(AMBIENT_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_AMBIENT, Mathf.Log10(ambientVolume) * 20);
    }
}
