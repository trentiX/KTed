using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
	[SerializeField] private UnityEngine.UI.Button _submitQuest;
	[SerializeField] private UnityEngine.UI.Button _startQuest;
	
	// Variables
	public List<DialogueActivator> questChars;
	private int _accBalanceInt;
	public bool questIsGoing;
	
	public SerializableDictionary<Quest, bool> _quests = new SerializableDictionary<Quest, bool>();
	public static UnityEvent<DialogueActivator> OnQuestInteracted = new UnityEvent<DialogueActivator>();
		
	public Quest _pointerClicked;
	public Quest _currQuest;
	
	// References
	
	private AudioManager audioManager;
	
	// Code
	
	private void Start()
	{
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
				_submitQuest.interactable = true;
			}
		}
	}

	public void StartQuestButton()
	{
		if (_currQuest != null) 
		{
			_currQuest.SubmitQuest();
		}

		questIsGoing = true;
		_currQuest = _pointerClicked;
		_currQuest.StartQuest();
		_currQuest.completionStatus.text = "Активный";


		_taskObjectives.text = "- " + _currQuest.questObjectives +
		 $" ( {_currQuest.interactedAmount} / {_currQuest.goalAmount} )";
	}

	public void SubmitQuestButton()
	{
		_currQuest.SubmitQuest();
		
		audioManager.QuestBitCompletion();
		
		_accBalanceInt += _currQuest.taskReward;
		_accBalance.text = _accBalanceInt + " KTedbux";
		
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
		}

		_taskObjectives.text = "- " + _currQuest.questObjectives +
		 $" ( {_currQuest.interactedAmount} / {_currQuest.goalAmount} )";
	}
	
	// DATA
	public void LoadData(GameData gameData)
	{
		_quests = gameData.questsInStorage;
		
		foreach(var quest in _quests)
		{
			if(quest.Value == true)
			{
				quest.Key.completionStatus.text = "Выполненный";
				quest.Key.interactedAmount = quest.Key.goalAmount;
			}
		}
	}

	public void SaveData(ref GameData gameData)
	{
		gameData.questsInStorage = _quests;
	}
}