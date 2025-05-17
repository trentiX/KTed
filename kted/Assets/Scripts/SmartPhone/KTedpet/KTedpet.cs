using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KTedpet : MonoBehaviour
{
    // Phrases
    [Header("Players phrases:")]
    [SerializeField][TextArea] private string[] playerGreetingPhrases;
    [SerializeField][TextArea] private string[] gotoChooseGamePhrases;
    [SerializeField][TextArea] private string[] gotoPlayPhrases;
    [SerializeField][TextArea] private string[] gotoStorePhrases;
    [SerializeField][TextArea] private string[] goBackPhrases;
    [SerializeField][TextArea] private string[] letsTalkPhrases;
    [SerializeField][TextArea] private string[] letsWalkPhrases;
    [SerializeField][TextArea] private string[] advicePhrases;
    [SerializeField][TextArea] private string[] factPhrases;
    [SerializeField][TextArea] private string[] morePhrases;


    // Serialization
    [Header("Messages:")]
    [SerializeField] private GameObject messageTemplate;
    [SerializeField] private GameObject responseTemplate;
    [SerializeField] private GameObject messages;
    [SerializeField] private ScrollRect messageBox;


    [Header("Buttons:")]
    [SerializeField] private GameObject buttonTemplate;
    [SerializeField] private UnityEngine.UI.Button[] buttonToDisable;

    [Header("Rooms:")]
    [SerializeField] private GameObject mainRoom;
    [SerializeField] private GameObject chooseGameRoom;
    [SerializeField] private GameObject storeRoom;
    [SerializeField] private GameObject playingRoom;


    [Header("Minigames:")]
    [SerializeField] Games[] games;
    [SerializeField] private CanvasGroup[] messageBoxCanvas;
    [SerializeField] private CanvasGroup darkPanelCanvas;


    [Header("Other:")]
    [SerializeField] private CanvasGroup canvasGroup;

    // Variables
    public static KTedpet instance;
    private List<GameObject> possibleActivities = new List<GameObject>();
    private GameObject currRoom;
    private AudioManager audioManager;
    private PlayRoomManager playRoomManager;
    private PetShopManager petShopManager;
    private Pet pet;
    public bool gameMode = false;

    // Code
    private void Start()
    {
        instance = this;
        currRoom = mainRoom;
        pet = FindObjectOfType<Pet>();
        audioManager = FindObjectOfType<AudioManager>();
        playRoomManager = FindObjectOfType<PlayRoomManager>();
        petShopManager = FindObjectOfType<PetShopManager>();
    }

    public void GenerateMessage(string message, string typeOfMessage)
    {
        // Создаём сообщение, но без текста
        GameObject newMessage = Instantiate(messageTemplate, messages.transform);
        newMessage.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(newMessage.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
        messageBox.verticalNormalizedPosition = 0;

        // Запускаем корутину для анимации точек, затем добавляем текст и кнопки
        StartCoroutine(MessageWithDotsAnimation(newMessage, message, typeOfMessage));
    }


    private IEnumerator MessageWithDotsAnimation(GameObject message, string finalText, string typeOfMessage)
    {
        TextMeshProUGUI messageText = message.GetComponentInChildren<TextMeshProUGUI>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
        messageBox.verticalNormalizedPosition = 0;

        // Анимация точек
        for (int i = 0; i <= 3; i++)
        {
            messageText.text = new string('.', i);
            LayoutRebuilder.ForceRebuildLayoutImmediate(message.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
            messageBox.verticalNormalizedPosition = 0;
            yield return new WaitForSeconds(0.6f);
        }

        // Устанавливаем окончательный текст
        messageText.text = finalText;
        audioManager.SFXNotificationSound();

        // Перестраиваем UI после изменения текста
        LayoutRebuilder.ForceRebuildLayoutImmediate(message.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
        messageBox.verticalNormalizedPosition = 0;

        // Создаю уведомление
        gameObject.GetComponent<Webpage>().shortCutButton.CreateNotification(finalText);

        // Генерируем кнопки после завершения анимации
        GenerateButton(typeOfMessage);
    }

    private void GenerateButton(string typeOfMessage)
{
    // Очищаем предыдущие кнопки, если генерируется новый набор ответов
    ClearPossibleActivities();

    // Создаем кнопки в зависимости от типа сообщения
    List<GameObject> buttons = typeOfMessage switch
    {
        "greeting" => CreateButtons(1),
        "talk" => CreateButtons(2),
        "intersection" => CreateButtons(2),
        "whatToDo" => CreateButtons(3),
        "fact" => CreateButtons(2), 
        "advice" => CreateButtons(2),
        _ => new List<GameObject>() // Если тип неизвестен, возвращаем пустой список
    };

    switch (typeOfMessage)
    {
        case "greeting":
            string greeting = GetRandomPhrase(playerGreetingPhrases);
            ConfigureButton(buttons[0], greeting, () => Greet(greeting));
            break;

        case "agree":
            GenerateMessage(GetRandomPhrase(pet.continueDialoguePetPhrases), "intersection");
            break;
            
        case "fact":
            string morePhrase = GetRandomPhrase(morePhrases);
            ConfigureButton(buttons[0], morePhrase, () => TellFact(morePhrase));
            
            string walkPhrase = GetRandomPhrase(letsWalkPhrases);
            ConfigureButton(buttons[1], walkPhrase, () => ChangeRoom(walkPhrase));
            break;    
        
        case "advice":
            string morePhrase1 = GetRandomPhrase(morePhrases);
            ConfigureButton(buttons[0], morePhrase1, () => TellAdvice(morePhrase1));
            
            string walkPhrase1 = GetRandomPhrase(letsWalkPhrases);
            ConfigureButton(buttons[1], walkPhrase1, () => ChangeRoom(walkPhrase1));
            break;
            
        case "talk":
            string factPhrase = GetRandomPhrase(factPhrases);
			ConfigureButton(buttons[0], factPhrase, () => TellFact(factPhrase));
			
			string advicePhrase = GetRandomPhrase(advicePhrases);
            ConfigureButton(buttons[1], advicePhrase, () => TellAdvice(advicePhrase));
			break;
            
		case "intersection":
            string talkPhrase = GetRandomPhrase(letsTalkPhrases);
            ConfigureButton(buttons[0], talkPhrase, () => ToTalk(talkPhrase));
            
            string walkPhrase2 = GetRandomPhrase(letsWalkPhrases);
            ConfigureButton(buttons[1], walkPhrase2, () => ChangeRoom(walkPhrase2));
			break;
			
        case "whatToDo":
            string goBackPhrase = GetRandomPhrase(goBackPhrases);
            ConfigureButton(buttons[0], goBackPhrase, () => GoBack(goBackPhrase));
            
            string gotoChooseGamePhrase = GetRandomPhrase(gotoChooseGamePhrases);
            ConfigureButton(buttons[1], gotoChooseGamePhrase, () => GoToChooseGame(gotoChooseGamePhrase));
            
            string gotoStorePhrase = GetRandomPhrase(gotoStorePhrases);
            ConfigureButton(buttons[2], gotoStorePhrase, () => GoToStore(gotoStorePhrase));
            break;

        default:
            Debug.LogWarning($"Unhandled message type: {typeOfMessage}");
            break;
    }

    LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
    messageBox.verticalNormalizedPosition = 0;
    }

    private void ConfigureButton(GameObject button, string text, UnityEngine.Events.UnityAction action)
    {
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(action);
        button.GetComponentInChildren<TextMeshProUGUI>().text = text;
        possibleActivities.Add(button);
    }

    private List<GameObject> CreateButtons(int amountOfButtons)
    {
        List<GameObject> createdButtons = new List<GameObject>();

        for (int i = 0; i < amountOfButtons; i++)
        {
            GameObject newButton = Instantiate(buttonTemplate, messages.transform);
            newButton.SetActive(true);
            createdButtons.Add(newButton);

            LayoutRebuilder.ForceRebuildLayoutImmediate(newButton.GetComponent<RectTransform>());
        }
        return createdButtons;
    }

    private void GenerateResponse(string message)
    {
        ClearPossibleActivities();

        GameObject newResponse = Instantiate(responseTemplate, messages.transform);
        newResponse.SetActive(true);
        newResponse.GetComponentInChildren<TextMeshProUGUI>().text = message;

        // After adding the new response, force its layout rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(newResponse.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(messageBox.content);
        messageBox.verticalNormalizedPosition = 0;
    }

    public string GetRandomPhrase(string[] phrases)
    {
        return phrases[Random.Range(0, phrases.Length)];
    }

    // Все методы теперь принимают строку как Greet()
    public void ChangeRoom(string phrase)
    {
        GenerateResponse(phrase);
        pet.ReceiveAction("walk");
    }

    public void ToTalk(string phrase)
    {
        GenerateResponse(phrase);
        pet.ReceiveAction("talk");
    }

    public void TellFact(string phrase)
    {
        GenerateResponse(phrase);
        pet.ReceiveAction("fact");
    }

    public void TellAdvice(string phrase)
    {
        GenerateResponse(phrase);
        pet.ReceiveAction("advice");
    }

    public void GoToStore(string phrase)
    {
        GenerateResponse(phrase);
        
        if (CanRoomChange(storeRoom))
        {
            StartCoroutine(RoomTransition(storeRoom));
            pet.ReceiveAction("store");
            
            foreach (GameObject accessory in petShopManager.accessories)
            {
                accessory.GetComponent<Accessory>().buyButton.interactable = true;
            }
        }
    }

    public void GoToChooseGame(string phrase)
    {
        GenerateResponse(phrase);
        
        if (CanRoomChange(chooseGameRoom))
        {
            StartCoroutine(RoomTransition(chooseGameRoom));
            pet.ReceiveAction("chooseGame");
            
            foreach (GameObject game in playRoomManager.gamesIcons)
            {
                game.GetComponent<Games>().playButton.interactable = true;
            }
        }
    }

    public void GoBack(string phrase)
    {
        GenerateResponse(phrase);
        
        if (CanRoomChange(mainRoom))
        {
            StartCoroutine(RoomTransition(mainRoom));
            pet.ReceiveAction("main");
        }
    }

    public void GoToPlay() // Этот метод остается без изменений, т.к. не использует фразы
    {
        playRoomManager.currGame.GetComponent<Games>().PlayStartAnim();
    }

    public void Greet(string phrase) // Образец, по которому сделаны остальные
    {
        GenerateResponse(phrase);
        pet.ReceiveAction("playerGreeting");
    }

    private IEnumerator RoomTransition(GameObject gotoRoom)
    {
        yield return new WaitForSeconds(2.5f);

        CanvasGroup currRoomCanvas = currRoom.GetComponent<CanvasGroup>();

        currRoomCanvas.interactable = false;
        currRoomCanvas.blocksRaycasts = false;
        currRoomCanvas.DOFade(0, 0.4f).OnComplete(() =>
        {
            currRoom.SetActive(false);
        });

        CanvasGroup gotoRoomCanvas = gotoRoom.GetComponent<CanvasGroup>();
        gotoRoom.SetActive(true);
        gotoRoomCanvas.DOFade(1, 0.4f).OnComplete(() =>
        {
            currRoom = gotoRoom;
        });
        gotoRoomCanvas.interactable = true;
        gotoRoomCanvas.blocksRaycasts = true;
    }

    public void StartMinigameAnim(Games game)
    {
        foreach (var canvas in messageBoxCanvas)
        {
            canvas.DOFade(0, 0.5f).OnComplete(() =>
            {
                canvas.gameObject.SetActive(false);
            });
        }

        darkPanelCanvas.DOFade(0.7f, 0.5f);

        gameMode = true;
        foreach (var but in Browser.instance._tabsOpened)
        {
            but.GetComponent<Tab>()._clickable = false;
            but.GetComponentInChildren<UnityEngine.UI.Button>(2).interactable = false; // Close tab button
            Browser.instance._addNewTab.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        foreach (var but in Browser.instance.browserMainButtons)
        {
            but.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        playRoomManager.currGame.GetComponent<Games>().StartGame();
    }

    public void CloseMinigame()
    {
        foreach (var canvas in messageBoxCanvas)
        {
            canvas.gameObject.SetActive(true);
            canvas.DOFade(1, 0.5f);
        }

        darkPanelCanvas.DOFade(0, 0.5f);

        gameMode = false;
        foreach (var but in Browser.instance._tabsOpened)
        {
            but.GetComponent<Tab>()._clickable = true;
            but.GetComponentInChildren<UnityEngine.UI.Button>(2).interactable = true; // Close tab button
            Browser.instance._addNewTab.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        foreach (var but in Browser.instance.browserMainButtons)
        {
            but.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    private bool CanRoomChange(GameObject gotoRoom)
    {
        if (currRoom != null)
        {
            if (currRoom == gotoRoom)
            {
                GenerateMessage(GetRandomPhrase(pet.gotoSameRoomPhrases), "whatToDo");
                return false;
            }
        }
        return true;
    }

    // Метод для очистки списка и уничтожения существующих кнопок ответов
    private void ClearPossibleActivities()
    {
        foreach (var button in possibleActivities)
        {
            Destroy(button);
        }
        possibleActivities.Clear();
    }
}