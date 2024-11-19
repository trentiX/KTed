using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
	// Phrases
	[Header("Pet's phrases")]
	[SerializeField][TextArea] public string[] onPlayerJoinPetPhrases;
	[SerializeField][TextArea] public string[] onPlayerChoseActionPetPhrases;
	[SerializeField][TextArea] public string[] continueDialoguePetPhrases;
	[SerializeField][TextArea] public string[] gotoSameRoomPhrases;

	// References
	private KTedpet ktedpet;
	
	// Variables
	
	// Code
	private void OnEnable()
	{
		ktedpet = FindObjectOfType<KTedpet>();
		
		ktedpet.GenerateMessage(ktedpet.GetRandomPhrase(onPlayerJoinPetPhrases), "greeting");
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
				
			case "playerGreeting":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(continueDialoguePetPhrases), "whatToDo");
				break;
		}
	}
}
