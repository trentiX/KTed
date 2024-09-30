using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider AmbientSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_AMBIENT = "AmbientVolume";

    private void Awake()
    {
        if (MusicSlider != null) MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (SFXSlider != null) SFXSlider.onValueChanged.AddListener(SetSFXVolume);
        if (AmbientSlider != null) AmbientSlider.onValueChanged.AddListener(SetAmbientVolume);
    }

    private void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        MusicSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
        MusicSlider.value = PlayerPrefs.GetFloat(AudioManager.AMBIENT_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, MusicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
        PlayerPrefs.SetFloat(AudioManager.AMBIENT_KEY, AmbientSlider.value);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
    void SetAmbientVolume(float value)
    {
        mixer.SetFloat(MIXER_AMBIENT, Mathf.Log10(value) * 20);
    }
}
