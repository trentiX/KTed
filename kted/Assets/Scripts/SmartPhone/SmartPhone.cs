using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SmartPhone : MonoBehaviour, IDataPersistence
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
    private Messenger _messenger;
    private DialogueUI _dialogueUI;
    
    //Animations
    private Tweener phoneImageIdleAnim;
    private Tweener phonePopUpAnim;
    private Tweener phoneHideAnim;
    
    //Variables
    public bool SmartPhonePicked;
    private bool isRinging = false;
    private DialogueObject tempDialogueObject;
    
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
        _messenger = FindObjectOfType<Messenger>();
        _dialogueUI = FindObjectOfType<DialogueUI>();

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
        if (SmartPhonePicked)
        {
            gameObject.transform.position = new Vector3(10000, 10000, 10000);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                answerTheCall();
            }    
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MusicBoxOnDistanceControl();
            }   
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Messenger();
            }   
        }
    }

    private void PhoneIsPicked()
    {
        SmartPhonePicked = true;
        _locationCompleted.SmartPhonePickAnim("Вы подобрали смартфон!");
    }

    private void answerTheCall()
    {
        if (isRinging)
        {
            StartCoroutine(HidePhone());
        }
    }
    public void Ring(DialogueObject dialogueObject)
    {
        if (CheckPhoneAnim() && player.canMove())
        {
            //_audioManager.StartCoroutine(_audioManager.FadeOut(_audioSource,3));
            _audioManager.StartCoroutine(_audioManager.FadeIn(_audioSource,2, _audioManager.phoneRing));
            
           PhonePopUpAnim();
           
            tempDialogueObject = dialogueObject;
            isRinging = true;
        }
    }

    private void PhonePopUpAnim()
    {
        phonePopUpAnim = phoneImage.transform.DOMoveY(250, 2)
            .SetEase(Ease.OutCubic)
            .OnComplete((() => PhoneRingAnim()));
    }
    private void PhoneRingAnim()
    {
        phoneImageIdleAnim = phoneImage.transform.DOMoveY(235, 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutCubic);
    }
    
    private IEnumerator HidePhone()
    {
        if (CheckPhoneAnim())
        {
            phoneHideAnim = phoneImage.transform.DOMoveY(-780, 2).OnComplete((() =>
            { 
                phoneImageIdleAnim.Kill();
            })).SetEase(Ease.InCubic);
        }
        
        _audioManager.StartCoroutine(_audioManager.FadeOut(_audioSource,3));

        yield return new WaitForSeconds(2);
            
        _dialogueUI.showDialogue(tempDialogueObject, "Разработчик КТеда");

        isRinging = false;
    }

    private void Messenger()
    {
        _messenger.gameObject.SetActive(true);
        _messenger.OpenMessenger();
    }

    private void MusicBoxOnDistanceControl()
    {
        if (!isRinging)
            player.MusicUI.showMusicBox();
    }
    
    private bool CheckPhoneAnim()
    {
        if (!phoneHideAnim.IsActive() && !phonePopUpAnim.IsActive())
            return true;
        return false;   
    }
    
    // DATA

    public void LoadData(GameData data)
    {
        this.SmartPhonePicked = data.phoneIsPicked;
    }

    public void SaveData(ref GameData data)
    {
        data.phoneIsPicked = this.SmartPhonePicked;
    }
}