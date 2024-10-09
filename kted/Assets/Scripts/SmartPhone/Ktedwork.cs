using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Ktedwork : MonoBehaviour, IDataPersistence
{
    // Serialization
    [Header("RightBox:")]
    [SerializeField] private TextMeshProUGUI _taskName;
    [SerializeField] private TextMeshProUGUI _taskLongDesc;
    [SerializeField] private TextMeshProUGUI _taskReward;
    
    [Header("LeftBox:")]
    [SerializeField] private GameObject _completionStatusbBox;
    [SerializeField] private GameObject _completionStatusBar;
    
    // Variables
    private List<GameObject> _completionStatusBars = new List<GameObject>();
    public List<DialogueActivator> questChars;

    private int _interactedChars = 0;
    public bool questIsGoing;
    
    public SerializableDictionary<Quest, bool> _quests = new SerializableDictionary<Quest, bool>();
    public static UnityEvent<DialogueActivator> OnQuestInteracted = new UnityEvent<DialogueActivator>();
        
    public Quest _pointerClicked;
    public Quest _currQuest;
    
    // Code
    public void OpenQuest(Quest quest)
    {
        _taskName.text = quest.taskName;
        _taskReward.text = quest.taskReward.ToString() + " KTedbux";
        _taskLongDesc.text = quest.longDescription;

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
        List<GameObject> compStatusBarsDeletion = new List<GameObject>(_completionStatusBars);
        _completionStatusBars.Clear();
        foreach (var bar in compStatusBarsDeletion)
        {
            Destroy(bar);
        }
        
        questIsGoing = true;
        _currQuest = _pointerClicked;
        _currQuest.StartQuest();

        _completionStatusbBox.SetActive(true);
        for (int i = 0; i < questChars.Count; i++)
        {
            GameObject newBar = Instantiate(_completionStatusBar, _completionStatusbBox.transform);
            newBar.SetActive(true);

            newBar.GetComponent<Image>().color = new Color(163, 149, 148);
            _completionStatusBars.Add(newBar);
        }
    }

    public void SubmitQuestButton()
    {
        _currQuest.SubmitQuest();
        _completionStatusBar.SetActive(false);
    }

    public void Interacted(DialogueActivator dialogueActivator) 
    {
        OnQuestInteracted.Invoke(dialogueActivator);
        _completionStatusBars[_interactedChars].GetComponent<Image>().color
            = new Color(0, 168, 120);
        _interactedChars++;
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
