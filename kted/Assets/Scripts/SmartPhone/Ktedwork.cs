using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Ktedwork : MonoBehaviour, IDataPersistence
{
	// Serialization
	[Header("Quest data:")]
	[SerializeField] private GameObject _taskDesc;
	[SerializeField] private GameObject _ktedWorkLogo;
	[SerializeField] private TextMeshProUGUI _taskName;
	[SerializeField] private TextMeshProUGUI _taskLongDesc;
	[SerializeField] private TextMeshProUGUI _taskReward;
	[SerializeField] private TextMeshProUGUI _taskObjectives;
	[SerializeField] private TextMeshProUGUI _accBalance;	
	[SerializeField] private TextMeshProUGUI _accBalanceInPet;	
	[SerializeField] private UnityEngine.UI.Button _submitQuest;
	[SerializeField] private UnityEngine.UI.Button _startQuest;
	
	// Variables
	public List<DialogueActivator> questChars;
	public List<GameObject> objectsToInteract;
	public int _accBalanceInt;
	public bool questIsGoing;
	
	public SerializableDictionary<Quest, bool> _quests = new SerializableDictionary<Quest, bool>();
	public static UnityEvent<DialogueActivator> OnQuestInteracted = new UnityEvent<DialogueActivator>();
	public static UnityEvent<GameObject> OnQuestCollected = new UnityEvent<GameObject>();
		
	public Quest _pointerClicked;
	public Quest _currQuest;

	public static Ktedwork instance { get; private set; }
 	
	// References
	
	private AudioManager audioManager;
	
	// Code
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		audioManager = FindObjectOfType<AudioManager>();	
	}
	
	public void OpenQuest(Quest quest)
	{
		_ktedWorkLogo.SetActive(false);
		_taskDesc.SetActive(true);
		
		_pointerClicked = quest;
		
		_taskName.text = quest.taskName;
		_taskReward.text = "- " + quest.taskReward.ToString() + " KTedbux";
		_taskLongDesc.text = "- " + quest.longDescription;

		_taskObjectives.text = "- " + _pointerClicked.questObjectives +
		 $" ( {_pointerClicked.interactedAmount} / {_pointerClicked.goalAmount} )";
		 
		_submitQuest.interactable = false;
		
		_quests.TryGetValue(quest, out var completed);
		{
			if (completed)
			{
				_startQuest.interactable = false;
				_submitQuest.interactable = false;
			}
			else 
			{
				_startQuest.interactable = true;
				_submitQuest.interactable = false;
			}
		}
		if (_pointerClicked != null && _pointerClicked.interactedAmount == _pointerClicked.goalAmount 
			&& _quests.ContainsKey(_pointerClicked) && _quests[_pointerClicked] == false)
		{
			_submitQuest.interactable = true;
		}
	}

	public void StartQuestButton()
	{
		if (questIsGoing) return;

		questIsGoing = true;
		_currQuest = _pointerClicked;
		_currQuest.StartQuest();
		_currQuest.completionStatus.text = "Активный";

		_taskObjectives.text = "- " + _currQuest.questObjectives +
		 $" ( {_currQuest.interactedAmount} / {_currQuest.goalAmount} )";
		 		
		_quests[_currQuest] = false;
	}

	public void SubmitQuestButton()
	{
		_currQuest.SubmitQuest();
		
		questIsGoing = false;
		audioManager.SFXQuestBitCompletion();
		
		_accBalanceInt += _currQuest.taskReward;
		_accBalance.text = _accBalanceInt + " KTedbux";
		_accBalanceInPet.text = _accBalanceInt + " KTedbux";
		
		_currQuest.completionStatus.text = "Выполненный";
		_startQuest.interactable = false;
		_submitQuest.interactable = false;
		_quests[_currQuest] = true;
	}

	public void Interacted(DialogueActivator dialogueActivator) 
	{
		OnQuestInteracted.Invoke(dialogueActivator);
		
		questChars.Remove(dialogueActivator);
		_currQuest.interactedAmount++;
		
		if (_currQuest.interactedAmount == _currQuest.goalAmount)
		{
			_submitQuest.interactable = true;
			audioManager.SFXNotificationSound();
		}

		_taskObjectives.text = "- " + _currQuest.questObjectives +
		 $" ( {_currQuest.interactedAmount} / {_currQuest.goalAmount} )";
	}
	
	public void Collected(GameObject gameObject)
	{
	    OnQuestCollected.Invoke(gameObject);
	    
	    objectsToInteract.Remove(gameObject);
	    _currQuest.interactedAmount++;
	    
	    if (_currQuest.interactedAmount == _currQuest.goalAmount)
	    {
	        _submitQuest.interactable = true;
	        audioManager.SFXNotificationSound();
	    }
	    
	    _taskObjectives.text = "- " + _currQuest.questObjectives +
	     $" ( {_currQuest.interactedAmount} / {_currQuest.goalAmount} )";
	}
	
	public void AccBalanceUIUpdate(int amountOfMoney)
	{
		_accBalanceInt = amountOfMoney;
		
		_accBalance.text = _accBalanceInt + " KTedbux";
		_accBalanceInPet.text = _accBalanceInt + " KTedbux";
	}
	// DATA
	public void LoadData(GameData gameData)
	{	
		if (gameData.questsInStorage != null) _quests = gameData.questsInStorage;
		
		AccBalanceUIUpdate(gameData.playersMoney);
		
		foreach(var quest in _quests)
		{
			if(quest.Value == true)
			{
				quest.Key.completionStatus.text = "Выполненный";
				quest.Key.interactedAmount = quest.Key.goalAmount;
			}
		}
		
		AmresQuest.instance.CheckOnComplete(); // Set active eastereggs if quest is completed
	}

	public void SaveData(ref GameData gameData)
	{
		gameData.questsInStorage = _quests;
		gameData.playersMoney = _accBalanceInt;
	}
}
