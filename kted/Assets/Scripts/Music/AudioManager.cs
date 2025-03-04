using System;
using System.Collections;
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
	public void SFXQuestBitCompletion()
	{
		AudioClip clip = sfxClips[1];
		
		sfxSource.PlayOneShot(clip);
	}
	public void SFXNotificationSound()
	{
		AudioClip clip = sfxClips[2];

		sfxSource.PlayOneShot(clip);
	}

	public void ClickSound()
	{
		AudioClip clip = sfxClips[3];

		sfxSource.PlayOneShot(clip);
	}
	public void SFXFailedSound()
	{
		AudioClip clip = sfxClips[4];

		sfxSource.PlayOneShot(clip);
	}
	public void SFXExplosionSound()
	{
		AudioClip clip = sfxClips[5];

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

	public void PlaySong(AudioClip clip)
	{
		if (musicSource.isPlaying)
		{
			return; // Prevent overlapping songs
		}

		musicSource.clip = clip; // Set the clip to the AudioSource
		musicSource.Play();      // Start playing the clip
	}

	public void PauseSong()
	{
		if (musicSource.isPlaying)
		{
			musicSource.Pause(); // Pause the currently playing clip
		}
	}

	public void ResumeSong()
	{
		if (!musicSource.isPlaying && musicSource.clip != null)
		{
			musicSource.UnPause(); // Resume playback if it was paused
		}
	}

	public void StopMusic()
	{
		if (musicSource.isPlaying)
		{
			musicSource.Stop();
		}
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

	public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
	{
		float startVolume = audioSource.volume;

		while (audioSource.volume > 0)
		{
			audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

			yield return null;
		}

		audioSource.Stop();
		audioSource.volume = startVolume; // Сбрасываем громкость обратно после остановки
	}

	public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, Action songToPlay)
	{
		if (audioSource.isPlaying)
		{
			StartCoroutine(FadeOut(audioSource, FadeTime));
			yield return new WaitForSeconds(FadeTime);
		}

		songToPlay.Invoke();
		audioSource.volume = 0; // Начинаем с нулевой громкости

		float targetVolume = 1.0f; // Желаемая максимальная громкость

		while (audioSource.volume < targetVolume)
		{
			audioSource.volume += Time.deltaTime / FadeTime;

			yield return null;
		}

		audioSource.volume = targetVolume; // Устанавливаем максимальную громкость
	}
}
