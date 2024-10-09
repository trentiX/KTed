using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameData
{
    public bool phoneIsPicked;
    public bool browserEasterEggFound;
    public Vector3 playerPosition;
    public SerializableDictionary<DialogueActivator, Response> responsesInStorage;
    public SerializableDictionary<GameObject, DialogueActivator> chatsInStorage;
    public SerializableDictionary<string, bool> ringedActionsInStorage;
    public SerializableDictionary<Quest, bool> questsInStorage;
    public List<AudioClip> favouriteSongs;


    public GameData()
    {
        this.phoneIsPicked = false;
        this.browserEasterEggFound = false;
        playerPosition = Vector3.zero;
        chatsInStorage = new SerializableDictionary<GameObject, DialogueActivator>();
        responsesInStorage = new SerializableDictionary<DialogueActivator, Response>();
        ringedActionsInStorage = new SerializableDictionary<string, bool>();
        questsInStorage = new SerializableDictionary<Quest, bool>();
        favouriteSongs = new List<AudioClip>();
    }
}