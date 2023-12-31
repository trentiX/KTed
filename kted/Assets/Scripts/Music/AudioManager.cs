using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource ambientSource;
    [SerializeField] List<AudioClip> musicClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxClips = new List<AudioClip>();
    [SerializeField] List<AudioClip> ambientClips = new List<AudioClip>();

    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";
    public const string AMBIENT_KEY = "ambientVolume";
    private void Awake()
    {
        if (instance == null)
        {
            instance = null;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SFXSound()
    {
        AudioClip clip = sfxClips[UnityEngine.Random.Range(0, sfxClips.Count)];
        
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic()
    {
        AudioClip clip = musicClips[UnityEngine.Random.Range(0, musicClips.Count)];

        if (musicSource.isPlaying)
        {
            return;
        }
        musicSource.PlayOneShot(clip);
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
