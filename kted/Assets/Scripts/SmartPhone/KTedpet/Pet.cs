using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour, IDataPersistence
{
	// Phrases
	[Header("Pet's phrases")]
	[SerializeField][TextArea] public string[] onPlayerFirstInteraction;
	[SerializeField][TextArea] public string[] onPlayerJoinPetPhrases;
	[SerializeField][TextArea] public string[] onPlayerChoseActionPetPhrases;
	[SerializeField][TextArea] public string[] continueDialoguePetPhrases;
	[SerializeField][TextArea] public string[] gotoSameRoomPhrases;

	// References
	private KTedpet ktedpet;
	private SmartPhone smartPhone;
	private bool firstInteraction;
	
	// Variables
	
	// Code
	private void OnEnable()
	{
		ktedpet = FindObjectOfType<KTedpet>();
		smartPhone = FindObjectOfType<SmartPhone>();
	}
	
	public void ReceiveAction(string action)
	{
		switch (action)
		{
			case "store":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(onPlayerChoseActionPetPhrases), "agree");
				break;
				
			case "main":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(onPlayerChoseActionPetPhrases), "agree");
				break;
				
			case "chooseGame":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(onPlayerChoseActionPetPhrases), "agree");
				break;	
				
			case "playerGreeting":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(continueDialoguePetPhrases), "whatToDo");
				break;
		}
	}
	
	public void FirstInteractionMessage()
	{
		if (smartPhone.SmartPhonePicked)
		{
			if (firstInteraction)
			{
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase(onPlayerFirstInteraction), "greeting");
				firstInteraction = false;	
			}
			else
			{
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase(onPlayerJoinPetPhrases), "greeting");
			}
		}
	}
	
	// DATA
	public void LoadData(GameData gameData)
	{
		firstInteraction = gameData.petFirstInteraction;
		
		FirstInteractionMessage();
	}

	public void SaveData(ref GameData gameData)
	{
		gameData.petFirstInteraction = firstInteraction;
	}
}
