using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RingManager : MonoBehaviour, IDataPersistence
{
    [Header("Dialogue objects for actions:")] 
    [SerializeField] public DialogueObject _dialogueObjectOnInteracted;
    [SerializeField] public DialogueObject _dialogueObjectEasterEgg;
    
    // Other scripts
    private SmartPhone _smartPhone;
    private Player _player;
    
    // Variables
    private SerializableDictionary<DialogueObject, bool> ringedActions;
    private void Start()
    {
        _smartPhone = FindObjectOfType<SmartPhone>();
        _player = FindObjectOfType<Player>();
    }
    private void OnEnable()
    {
        EasterEggPickUp.EasterEggPickedUp += Ring;
        DialogueActivator.onInteracted += Ring;
    }

    private void OnDisable()
    {
        EasterEggPickUp.EasterEggPickedUp -= Ring;
        DialogueActivator.onInteracted -= Ring;
    }

    private void Ring(DialogueObject dialogueObject)
    {
        ringedActions.TryGetValue(dialogueObject, out var alreadyRinged);
        {
            if (alreadyRinged == true) return;
        }

        StartCoroutine(DelayedRing(dialogueObject));
    }

    private IEnumerator DelayedRing(DialogueObject dialogueObject)
    {
        yield return new WaitForSeconds(Random.Range(5,7));
        yield return new WaitUntil(() => _player.canMove());
        _smartPhone.Ring(dialogueObject);
        
        ringedActions?.Add(dialogueObject, true);
    }
    
    // DATA

    public void LoadData(GameData gameData)
    {
        ringedActions = gameData.ringedActionsInStorage;
    }
    public void SaveData(ref GameData gameData)
    {
        gameData.ringedActionsInStorage = ringedActions;
    }
}
