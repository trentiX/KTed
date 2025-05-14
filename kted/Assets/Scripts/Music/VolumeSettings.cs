using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Music Sliders")]
    [SerializeField] private List<Slider> MusicSliders;

    [Header("SFX Slider")]
    [SerializeField] private Slider SFXSlider;

    [Header("Ambient Slider")]
    [SerializeField] private Slider AmbientSlider;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_AMBIENT = "AmbientVolume";

    private void Awake()
    {
        foreach (var slider in MusicSliders)
            slider.onValueChanged.AddListener(SetMusicVolume);

        if (SFXSlider != null)
            SFXSlider.onValueChanged.AddListener(SetSFXVolume);
        if (AmbientSlider != null)
            AmbientSlider.onValueChanged.AddListener(SetAmbientVolume);
    }

    private void Start()
    {
        float musicValue = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        float sfxValue = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
        float ambientValue = PlayerPrefs.GetFloat(AudioManager.AMBIENT_KEY, 1f);

        foreach (var slider in MusicSliders)
            slider.SetValueWithoutNotify(musicValue);

        if (SFXSlider != null)
            SFXSlider.SetValueWithoutNotify(sfxValue);
        if (AmbientSlider != null)
            AmbientSlider.SetValueWithoutNotify(ambientValue);

        SetMusicVolume(musicValue);
        SetSFXVolume(sfxValue);
        SetAmbientVolume(ambientValue);
    }

    private void OnDisable()
    {
        if (MusicSliders.Count > 0)
            PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, MusicSliders[0].value);
        if (SFXSlider != null)
            PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
        if (AmbientSlider != null)
            PlayerPrefs.SetFloat(AudioManager.AMBIENT_KEY, AmbientSlider.value);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);

        foreach (var slider in MusicSliders)
        {
            if (!Mathf.Approximately(slider.value, value))
                slider.SetValueWithoutNotify(value);
        }
    }

    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
    }

    void SetAmbientVolume(float value)
    {
        mixer.SetFloat(MIXER_AMBIENT, Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
    }
}
