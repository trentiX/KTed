using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SmartPhone : MonoBehaviour, IDataPersistence
{
	// Serialization
	[SerializeField] private Transform smartPhoneInitPos;
	[SerializeField] private GameObject phoneImage;
	[SerializeField] private AudioSource _audioSource;
	
	// Other scripts
	private AudioManager _audioManager;
	private Player player;
	private LocationCompleted _locationCompleted;
	private Browser _browser;
	private Messenger _messenger;
	private DialogueUI _dialogueUI;
	private Pet pet;
	
	// Animations
	private Tweener phoneImageIdleAnim;
	private Tweener phonePopUpAnim;
	private Tweener phoneHideAnim;
	
	// Variables
	public static SmartPhone instance { get; private set; }
	public bool SmartPhonePicked;
	private bool isRinging = false;
	private DialogueObject tempDialogueObject;
	
	// String
	private string phoneControlls = "\"1\" - Достать/убрать телефон или ответить на звонок" +
									"\n\"2\" - Открыть звуковой проигрыватель" +
									"\n\"3\" - Открыть мессенджер";
	
	private string phoneControllsHeader = "Как пользоватся смартфоном?)";
	
	//Code
	private void Start()
	{
		instance = this;
		player = FindObjectOfType<Player>();
		_audioManager = FindObjectOfType<AudioManager>();
		_locationCompleted = FindObjectOfType<LocationCompleted>();
		_browser = FindObjectOfType<Browser>();
		_messenger = FindObjectOfType<Messenger>();
		_dialogueUI = FindObjectOfType<DialogueUI>();
		pet = FindObjectOfType<Pet>();

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
				if (phonePopUpAnim.IsActive()) return;
				answerTheCall();
			}    
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				OpenBrowser();
			}   
		}
	}

	private void PhoneIsPicked()
	{
		SmartPhonePicked = true;
		_locationCompleted.SmartPhonePickAnim("Вы подобрали смартфон!");
		
		pet.FirstInteractionMessage();
	}

	private void answerTheCall()
	{
		if (isRinging)
		{
			isRinging = false;
			StartCoroutine(HidePhone());
		}
	}
	public void Ring(DialogueObject dialogueObject)
	{
		if (!SmartPhonePicked) return;
		
		if (CheckPhoneAnim() && player.canMove())
		{
			//_audioManager.StartCoroutine(_audioManager.FadeOut(_audioSource,3));
			_audioManager.StartCoroutine(_audioManager.FadeIn(_audioSource, 2, _audioManager.phoneRing));
			
			PhonePopUpAnim();
		   
			tempDialogueObject = dialogueObject;
			isRinging = true;
		}
	}

	private void OpenBrowser()
	{
		_browser.gameObject.SetActive(true);
		_browser.OpenBrowser(_browser.mainPage, true);
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
		
		_audioManager.StartCoroutine(_audioManager.FadeOut(_audioSource, 3));

		yield return new WaitForSeconds(2);
			
		_dialogueUI.showDialogue(tempDialogueObject, "Разработчик КТеда");

		isRinging = false;
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
