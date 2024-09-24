using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RingManager : MonoBehaviour
{
    [Header("Dialogue objects for actions:")] 
    [SerializeField] public DialogueObject _dialogueObjectOnInteracted;
    [SerializeField] public DialogueObject _dialogueObjectEasterEgg;
    
    private SmartPhone _smartPhone;
    private Player _player;
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
        StartCoroutine(DelayedRing(dialogueObject));
    }

    private IEnumerator DelayedRing(DialogueObject dialogueObject)
    {
        yield return new WaitUntil(_player.canMove);
        yield return new WaitForSeconds(Random.Range(5,7));
        _smartPhone.Ring(dialogueObject);

    }
}
