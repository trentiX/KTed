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
    private Tweener phoneHideAnim;
    
    //Booleans
    public bool SmartPhonePicked;
    private bool isTaken = false;
    private bool isTipTextShowed = false;
    private bool isRinging = false;
    
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
            SmartPhonePicked = true;
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
                MusicBoxOnDistanceControl();
            }   
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Messanger();
            }   
        }
    }

    private void PhoneIsPicked()
    {
        SmartPhonePicked = true;
        _locationCompleted.SmartPhonePickAnim("Вы подобрали смартфон!\n" +
                                              "Нажмите 1 ");
    }

    private void takePhone()
    {
        if (CheckPhoneAnim() && player.canMove())
        {
            if (isTaken)
            {
                hidePhone();
            }
            else
            {
                phonePopUpAnim = phoneImage.transform.DOMoveY(250, 2)
                    .SetEase(Ease.OutCubic)
                    .OnComplete((() => PhoneRingAnim()));
            }   
        }
    }
    private void Ring()
    {
        if (!isTipTextShowed && CheckPhoneAnim() && player.canMove())
        {
            //_audioManager.StartCoroutine(_audioManager.FadeOut(_audioSource,3));
            _audioManager.StartCoroutine(_audioManager.FadeIn(_audioSource,2, _audioManager.phoneRing));

            takePhone();
            isTaken = true;
            isRinging = true;
        }
    }

    private void PhoneRingAnim()
    {
        phoneImageIdleAnim = phoneImage.transform.DOMoveY(235, 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);

        if (!isRinging)
        {
            _tipPanel.tipPanelPopUp(phoneControllsHeader, phoneControlls);
            isTipTextShowed = true;
        }
        
        isTaken = true;
    }
    
    private void hidePhone()
    {
        if(isRinging)
            _audioManager.StartCoroutine(_audioManager.FadeOut(_audioSource,3));

        if (CheckPhoneAnim())
        {
            phoneHideAnim = phoneImage.transform.DOMoveY(-780, 2).OnComplete((() =>
            { 
                phoneImageIdleAnim.Kill();
            })).SetEase(Ease.InCubic);
        }

        if (!isRinging)
        {
            _tipPanel.tipPanelPopUp(phoneControllsHeader, phoneControlls);
            isTipTextShowed = false;
        }
        
        isTaken = false;
        isRinging = false;
    }

    private void Messanger()
    {
        
    }

    private void MusicBoxOnDistanceControl()
    {
        if (!isTipTextShowed && !isRinging)
            player.MusicUI.showMusicBox();
    }
    
    private bool CheckPhoneAnim()
    {
        if (!phoneHideAnim.IsActive() && !phonePopUpAnim.IsActive())
            return true;
        return false;   
    }
}
