using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameData
{
	public int playersMoney;
	public bool phoneIsPicked;
	public bool browserEasterEggFound;
	public Vector3 playerPosition;
	public SerializableDictionary<GameObject, Vector3> eaterEggsPositionInStorage;
	public SerializableDictionary<string, bool> interactionsWithPet;
	public SerializableDictionary<DialogueActivator, List<Response>> responsesInStorage;
	public SerializableDictionary<GameObject, DialogueActivator> chatsInStorage;
	public SerializableDictionary<string, bool> ringedActionsInStorage;
	public SerializableDictionary<Quest, bool> questsInStorage;
	public SerializableDictionary<TestActivator, int> testsInStorage;
	public SerializableDictionary<Accessory, bool> boughtAccessoriesInStorage;
	public SerializableDictionary<Accessory, bool> equippedAccessoriesInStorage;
	public List<AudioClip> favouriteSongs;
	public int MaxCombo;
	public int MaxScore;
	public int adviceCounterInStorage;
	public int factsCounterInStorage;
	public List<PetAppearance> petAppearance; // Добавленное поле для внешнего вида питомца


	public GameData()
	{
		playersMoney = new int();
		this.phoneIsPicked = false;
		this.browserEasterEggFound = false;
		playerPosition = Vector3.zero;
		eaterEggsPositionInStorage = new SerializableDictionary<GameObject, Vector3>();
		interactionsWithPet = new SerializableDictionary<string, bool>();
		chatsInStorage = new SerializableDictionary<GameObject, DialogueActivator>();
		responsesInStorage = new SerializableDictionary<DialogueActivator, List<Response>>();
		ringedActionsInStorage = new SerializableDictionary<string, bool>();
		questsInStorage = new SerializableDictionary<Quest, bool>();
		testsInStorage = new SerializableDictionary<TestActivator, int>();
		boughtAccessoriesInStorage = new SerializableDictionary<Accessory, bool>();
		equippedAccessoriesInStorage = new SerializableDictionary<Accessory, bool>();
		favouriteSongs = new List<AudioClip>();
		MaxCombo = new int();
		MaxScore = new int();
		adviceCounterInStorage = new int();
		factsCounterInStorage = new int();
		petAppearance = new List<PetAppearance>(); // Инициализация значения по умолчанию
	}
}