using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameData
{
	public int playersMoney;
	public bool phoneIsPicked;
	public bool petFirstInteraction;
	public bool browserEasterEggFound;
	public Vector3 playerPosition;
	public SerializableDictionary<DialogueActivator, Response> responsesInStorage;
	public SerializableDictionary<GameObject, DialogueActivator> chatsInStorage;
	public SerializableDictionary<string, bool> ringedActionsInStorage;
	public SerializableDictionary<Quest, bool> questsInStorage;
	public SerializableDictionary<TestActivator, int> testsInStorage;
	public SerializableDictionary<Accessory, bool> boughtAccessoriesInStorage;
	public List<AudioClip> favouriteSongs;


	public GameData()
	{
		playersMoney = new int();
		this.phoneIsPicked = false;
		this.petFirstInteraction = true;
		this.browserEasterEggFound = false;
		playerPosition = Vector3.zero;
		chatsInStorage = new SerializableDictionary<GameObject, DialogueActivator>();
		responsesInStorage = new SerializableDictionary<DialogueActivator, Response>();
		ringedActionsInStorage = new SerializableDictionary<string, bool>();
		questsInStorage = new SerializableDictionary<Quest, bool>();
		testsInStorage = new SerializableDictionary<TestActivator, int>();
		boughtAccessoriesInStorage = new SerializableDictionary<Accessory, bool>();
		favouriteSongs = new List<AudioClip>();
	}
}