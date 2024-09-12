using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmartPhone : MonoBehaviour
{
    //Serialization
    [SerializeField] private Transform smartPhoneInitPos;
    [SerializeField] private GameObject phoneImage;
    [SerializeField] private AudioSource _audioSource;
    
    //Other scripts
    private AudioManager _audioManager;
    private Player player;
    private LocationCompleted _locationCompleted;
    private TipPanel _tipPanel;
    
    //Animations
    private Tweener phoneImageIdleAnim;
    private Tweener phonePopUpAnim;
    
    //Booleans
    public bool SmartPhonePicked;
    private bool isRinging = false;
    private bool tipTextShow = false;
    
    //String
    private string phoneControlls = "\"1\" - Достать/убрать телефон или ответить на звонок" +
                                    "\n\"2\" - Открыть звуковой проигрыватель" +
                                    "\n\"3\" - Открыть мессенджер";
    
    private string phoneControllsHeader = "Как пользоватся смартфоном?)";
    
    //Code
    private void Start()
    {
        player = FindObjectOfType<Player>();
        _audioManager = FindObjectOfType<AudioManager>();
        _locationCompleted = FindObjectOfType<LocationCompleted>();
        _tipPanel = FindObjectOfType<TipPanel>();

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
                takePhone();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                takePhone();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Messanger();
            }   
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                MusicBoxOnDistanceControl();
            }   
        }
    }

    private void PhoneIsPicked()
    {
        SmartPhonePicked = true;
        _locationCompleted.SmartPhonePickAnim("Вы подобрали смартфон!");
    }

    private void takePhone()
    {
        if (isRinging)
        {
            hidePhone();
            tipTextShow = false;
        }
        else
        {
            if (!phonePopUpAnim.IsActive())
            {
                phonePopUpAnim = phoneImage.transform.DOMoveY(250, 2)
                    .SetEase(Ease.OutCubic)
                    .OnComplete((() => PhoneRingAnim()));
                tipTextShow = true;
                _tipPanel.tipPanelPopUp(phoneControllsHeader, phoneControlls);
            }
        }
    }
    private void Ring()
    {
        if (tipTextShow)
        {
            _audioManager.phoneRing();
            takePhone();
            isRinging = true;
        }
    }

    private void PhoneRingAnim()
    {
        phoneImageIdleAnim = phoneImage.transform.DOMoveY(235, 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic); 
    }
    
    private void hidePhone()
    {
        StartCoroutine(FadeOut(_audioSource, 2));
        phoneImage.transform.DOMoveY(-780, 2).OnComplete((() =>
        { 
            phoneImageIdleAnim.Kill();
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
