using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlihansQuest : Quest
{
    // Serialization
    [SerializeField] private List<DialogueActivator> _neededChars;

    // Variables
    private SerializableDictionary<DialogueActivator, bool> _signedPeople;
    private int _interactedChars = 0;

    // Code
    private void OnEnable()
    {
        Ktedwork.OnQuestInteracted.AddListener(InteractedWithChar);
        
        _signedPeople = new SerializableDictionary<DialogueActivator, bool>();
        foreach (var character in _neededChars)
        {
            _signedPeople.TryAdd(character, false);
        }
    }

    private void OnDisable()
    {
        Ktedwork.OnQuestInteracted.RemoveListener(InteractedWithChar);
    }

    private void InteractedWithChar(DialogueActivator dialogueActivator)
    {
        _signedPeople[dialogueActivator] = true;

        _interactedChars++;

        if (_interactedChars == _neededChars.Count)
        {
            questIsOver = true;
        }
    }

    public override void StartQuest()
    {
        base.StartQuest();
        _ktedwork.questChars = new List<DialogueActivator>(_neededChars);
    }

    public override void SubmitQuest()
    {
        base.SubmitQuest();
        _ktedwork.questChars.Clear();
    }
}
