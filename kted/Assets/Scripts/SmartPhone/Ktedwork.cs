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
    [SerializeField] private TextMeshProUGUI _taskName;
    [SerializeField] private TextMeshProUGUI _taskLongDesc;
    [SerializeField] private TextMeshProUGUI _taskReward;
    [SerializeField] private TextMeshProUGUI _taskObjectives;

    // Variables
    public List<DialogueActivator> questChars;

    private int _interactedChars = 0;
    private int _goalAmountOfChars;
    public bool questIsGoing;
    
    public SerializableDictionary<Quest, bool> _quests = new SerializableDictionary<Quest, bool>();
    public static UnityEvent<DialogueActivator> OnQuestInteracted = new UnityEvent<DialogueActivator>();
        
    public Quest _pointerClicked;
    public Quest _currQuest;
    
    // Code
    public void OpenQuest(Quest quest)
    {
        _taskName.text = quest.taskName;
        _taskReward.text = "- " + quest.taskReward.ToString() + " KTedbux";
        _taskLongDesc.text = "- " + quest.longDescription;
        _taskObjectives.text = "- " + quest.questObjectives;

        _quests.TryGetValue(quest, out var completed);
        {
            if (completed)
            {
                // Change completed UI
            }
        }
    }

    public void StartQuestButton()
    {
        questIsGoing = true;
        _currQuest = _pointerClicked;
        _currQuest.StartQuest();

        _goalAmountOfChars = questChars.Count;
        _taskObjectives.text = "- " + _currQuest.questObjectives +
         $" ( {_interactedChars} / {_goalAmountOfChars} )";
    }

    public void SubmitQuestButton()
    {
        _currQuest.SubmitQuest();
    }

    public void Interacted(DialogueActivator dialogueActivator) 
    {
        OnQuestInteracted.Invoke(dialogueActivator);
        _interactedChars++;
        
        _taskObjectives.text = "- " + _currQuest.questObjectives +
         $" ( {_interactedChars} / {_goalAmountOfChars} )";
    }
    
    // DATA
    public void LoadData(GameData gameData)
    {
        _quests = gameData.questsInStorage;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.questsInStorage = _quests;
    }
}
