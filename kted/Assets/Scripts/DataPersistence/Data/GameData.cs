using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool phoneIsPicked;
    public Vector3 playerPosition;
    public SerializableDictionary<DialogueActivator, Response> responsesInStorage;
    public SerializableDictionary<GameObject, DialogueActivator> chatsInStorage;
    public SerializableDictionary<DialogueObject, bool> ringedActionsInStorage;


    public GameData()
    {
        this.phoneIsPicked = false;
        playerPosition = Vector3.zero;
        chatsInStorage = new SerializableDictionary<GameObject, DialogueActivator>();
        responsesInStorage = new SerializableDictionary<DialogueActivator, Response>();
        ringedActionsInStorage = new SerializableDictionary<DialogueObject, bool>();
    }
}