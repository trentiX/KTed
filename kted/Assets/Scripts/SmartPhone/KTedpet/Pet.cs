using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour, IDataPersistence
{
	// Phrases
	
	// References
	[Header("Pet's phrases")]
	[SerializeField][TextArea] public string[] onPlayerJoinPetPhrases;
	[SerializeField][TextArea] public string[] onPlayerChoseActionPetPhrases;
	[SerializeField][TextArea] public string[] continueDialoguePetPhrases;
	[SerializeField][TextArea] public string[] gotoSameRoomPhrases;
	[SerializeField][TextArea] public string[] onItemPurchasedPetPhrases;
	[SerializeField][TextArea] public string[] onTalkPetPhrases;
	[SerializeField][TextArea] public string[] onChangeRoomPetPhrases;
	[SerializeField][TextArea] public string[] advicePetPhrases;
	[SerializeField][TextArea] public string[] factPetPhrases;
	[SerializeField][TextArea] public string[] walkPetPhrases;

	
	[Header("Interaction phrases")]
	[SerializeField][TextArea] public string[] onPlayerFirstInteraction;
	[SerializeField][TextArea] public string[] onFirstKtedWorkEnterInteraction;
	[SerializeField][TextArea] public string[] onFirstKtedGramEnterInteraction;
	[SerializeField][TextArea] public string[] onFirstKtedTifyEnterInteraction;
	[SerializeField][TextArea] public string[] onFirstRythmGameEnterInteraction;


    // Variables
	private List<string> possibleInteractions = new List<string>()
	{
		"firstInteraction",
		"firstKtedWorkEnter",
		"firstKtedGramEnter",
		"firstKtedTifyEnter",
		"firstRythmGameEnter"
	};
	
	public static Pet instance;
	private KTedpet ktedpet;
	private SmartPhone smartPhone;
	private SerializableDictionary<string, bool> interactions = new SerializableDictionary<string, bool>();
	private int factsCounter = 0;
	private int adviceCounter = 0;

    // Code
    void Awake()
    {
        instance = this;
    }
    private void OnEnable()
	{
		ktedpet = FindObjectOfType<KTedpet>();
		smartPhone = FindObjectOfType<SmartPhone>();
	}
	
	public void ReceiveAction(string action)
	{
		switch (action)
		{
			case "walk":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(walkPetPhrases), "whatToDo");
				break;
				
			case "advice":
				PhrasesOneByOne(advicePetPhrases, "advice");
				break;
				
			case "fact":
				PhrasesOneByOne(factPetPhrases, "fact");
				break;
				
			case "changeRoom":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(onChangeRoomPetPhrases), "whatToDo");
				break;
				
			case "talk":
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase
					(onTalkPetPhrases), "talk");
				break;

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
					(continueDialoguePetPhrases), "intersection");
				break;
		}
	}
	
	private void PhrasesOneByOne(string[] phrases, string action)
	{
		string phrase = "";

		if (action == "advice")
		{
			phrase = phrases[adviceCounter];
			adviceCounter = (adviceCounter + 1) % phrases.Length; // Инкремент с цикличностью
		}
		else if (action == "fact")
		{
			phrase = phrases[factsCounter];
			factsCounter = (factsCounter + 1) % phrases.Length; // Инкремент с цикличностью
		}

		// Разделяем фразу на две части
		string[] splitPhrases = SplitPhraseInTwo(phrase);

		// Запускаем метод отправки сообщений по одному
		StartCoroutine(SeparatedPhrases(splitPhrases, action, 2f));
	}

	// Метод деления фразы на две части
	private string[] SplitPhraseInTwo(string phrase)
	{
		int middle = phrase.Length / 2;

		// Если середина попадает не на пробел, ищем ближайший пробел
		if (phrase[middle] != ' ')
		{
			int leftSpace = phrase.LastIndexOf(' ', middle);
			int rightSpace = phrase.IndexOf(' ', middle);

			middle = (leftSpace >= 0) ? leftSpace : (rightSpace >= 0 ? rightSpace : middle);
		}

		// Добавляем троеточие к первой части
		string firstPart = phrase.Substring(0, middle).Trim() + "...";
		string secondPart = "..." + phrase.Substring(middle).Trim();

		return new string[]
		{
			firstPart,
			secondPart
		};
	}

	private IEnumerator SeparatedPhrases(string[] phrases, string typeOfMessage, float delay)
	{
		for (int i = 0; i < phrases.Length; i++)
		{
			if (i == phrases.Length - 1)
			{
				ktedpet.GenerateMessage(phrases[i], typeOfMessage);
				break;
			}	
			
			ktedpet.GenerateMessage(phrases[i], "");
				
			yield return new WaitForSeconds(delay); // Задержка между сообщениями	
		}
	}
	
	// Метод для первого взаимодействия с игроком (оставляем без изменений)
	public void FirstInteractionMessage()
	{
		if (smartPhone == null) return;
		if (smartPhone.SmartPhonePicked)
		{
			interactions.TryGetValue("firstInteraction", out bool value);
			
			if (value)
			{
				StartCoroutine(SeparatedPhrases(onPlayerFirstInteraction, "greeting", 4f));
				interactions["firstInteraction"] = false;
			}
			else
			{
				ktedpet.GenerateMessage(ktedpet.GetRandomPhrase(onPlayerJoinPetPhrases), "greeting");
			}
		}
	}

	// Универсальный метод для обработки первых взаимодействий
	public void HandleFirstInteraction(string interactionKey, string[] phrases)
	{
		if (smartPhone.SmartPhonePicked)
		{
			interactions.TryGetValue(interactionKey, out bool value);
			
			if (value)
			{
				NotificationManager.instance.SendSmallWindowText(ktedpet.GetRandomPhrase(phrases));
				interactions[interactionKey] = false;
			}
		}
	}

	// Примеры использования универсального метода
	public void FirstKtedWorkEnterMessage()
	{
		HandleFirstInteraction("firstKtedWorkEnter", onFirstKtedWorkEnterInteraction);
	}

	public void FirstKtedGramEnterMessage()
	{
		HandleFirstInteraction("firstKtedGramEnter", onFirstKtedGramEnterInteraction);
	}

	public void FirstKtedTifyEnterMessage()
	{
		HandleFirstInteraction("firstKtedTifyEnter", onFirstKtedTifyEnterInteraction);
	}

	public void FirstRythmGameEnterMessage()
	{
		HandleFirstInteraction("firstRythmGameEnter", onFirstRythmGameEnterInteraction);
	}
	
	public void CommentGamePlay(string message)
	{
	    NotificationManager.instance.SendSmallWindowText(message);
	}
	
	// DATA
	public void LoadData(GameData gameData)
	{
		for (int i = 0; i < possibleInteractions.Count; i++)
		{
			if (gameData.interactionsWithPet.TryGetValue(possibleInteractions[i], out bool value))
			{
				interactions.Add(possibleInteractions[i], value);
			}
			else
			{
				interactions.Add(possibleInteractions[i], true);
			}
		}
		
		FirstInteractionMessage();
		adviceCounter = gameData.adviceCounterInStorage;
		factsCounter = gameData.factsCounterInStorage;
	}

	public void SaveData(ref GameData gameData)
	{
		gameData.adviceCounterInStorage = adviceCounter;
		gameData.factsCounterInStorage = factsCounter;
		
		for(int i = 0; i < possibleInteractions.Count; i++)
		{
			if (interactions.TryGetValue(possibleInteractions[i], out bool value))
			{
				gameData.interactionsWithPet[possibleInteractions[i]] = value;
			}
		}
	}
}
