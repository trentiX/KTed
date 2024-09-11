using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmartPhone : MonoBehaviour
{
    [SerializeField] private Transform smartPhoneInitPos;
    [SerializeField] private GameObject phoneImage;
    [SerializeField] private AudioSource _audioSource;
    
    public bool SmartPhonePicked;
    private AudioManager _audioManager;
    private bool isRinging = false;
    private Player player;
    private Tweener phoneImageAnim;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        _audioManager = FindObjectOfType<AudioManager>();

        if (!SmartPhonePicked)
        {
            gameObject.transform.position = smartPhoneInitPos.position;
            Debug.Log("phone on init position");
        }
        else
        {
            PhoneIsPicked();
        }
    }

    private void OnEnable()
    {
        EasterEggManager.OnEasterEggPickupUpdated.AddListener(PhoneIsPicked);
    }

    private void OnDisable()
    {
        EasterEggManager.OnEasterEggPickupUpdated.RemoveListener(PhoneIsPicked);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Ring();
        }
        if (SmartPhonePicked)
        {
            gameObject.transform.position = new Vector3(10000, 10000, 10000);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                getCall();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Messanger();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                MusicBoxOnDistanceControl();
            }   
        }
    }

    private void PhoneIsPicked()
    {
        SmartPhonePicked = true;
    }

    private void Ring()
    {
        _audioManager.phoneRing();
        isRinging = true;
        phoneImage.SetActive(true);
        phoneImage.transform.DOMoveY(250, 2).SetEase(Ease.OutCubic).OnComplete((() => PhoneRingAnim()));  
    }

    private void PhoneRingAnim()
    {
        phoneImageAnim = phoneImage.transform.DOMoveY(235, 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic); 
    }
    
    private void getCall()
    {
        if (isRinging)
        {
            endCall();
        }
    }

    private void endCall()
    {
        StartCoroutine(FadeOut(_audioSource, 2));
        phoneImage.transform.DOMoveY(-780, 2).OnComplete((() =>
        { 
            phoneImageAnim.Kill();
            _audioManager.StopMusic();
        })).SetEase(Ease.InCubic);
    }

    private void Messanger()
    {
        
    }

    private void MusicBoxOnDistanceControl()
    {
        player.MusicUI.showMusicBox();
    }
    
    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop ();
        audioSource.volume = startVolume;
    }
}
