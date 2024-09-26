using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameData
{
    public bool phoneIsPicked;
    public Vector3 playerPosition;
    public SerializableDictionary<DialogueActivator, Response> responsesInStorage;
    public SerializableDictionary<GameObject, DialogueActivator> chatsInStorage;
    public SerializableDictionary<string, bool> ringedActionsInStorage;


    public GameData()
    {
        this.phoneIsPicked = false;
        playerPosition = Vector3.zero;
        chatsInStorage = new SerializableDictionary<GameObject, DialogueActivator>();
        responsesInStorage = new SerializableDictionary<DialogueActivator, Response>();
        ringedActionsInStorage = new SerializableDictionary<string, bool>();
    }
}