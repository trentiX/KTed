using TMPro;
using UnityEngine;

public class TestHandler : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject testResultsPanel;
    [SerializeField] private TextMeshProUGUI nameOfTest;
    [SerializeField] private TextMeshProUGUI resultsOfTheTest;

    public static TestHandler instance;
    public SerializableDictionary<TestActivator, int> tests;
    public bool TestOpen = false;
    public bool TestGoing = false;
    public TestActivator currTestActivator;
    
    private bool allowSpaceToClose = true; // Новый флаг

    private void Awake()
    {
        instance = this;    
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && allowSpaceToClose)
        {
            CloseResults();
        }
    }

    public void ShowResults(TestActivator testActivator)
    {
        TestOpen = true;
        currTestActivator = testActivator;
        testResultsPanel.SetActive(true);
        nameOfTest.text = testActivator.nameOfTest;
        resultsOfTheTest.text = testActivator.correctAnswers + " / " + testActivator.testAnswers.Count;
        allowSpaceToClose = true; // Разрешаем закрывать пробелом
    }
    
    public void CloseResults()
    {
        if (TestGoing) return;

        testResultsPanel.SetActive(false);
        TestOpen = false;
    }
    
    public void TestAgain()
    {
    //     if (currTestActivator == null)
    //     {
    //         Debug.LogError("currTestActivator is null. Cannot start the test again.");
    //         return;
    //     }

    //     allowSpaceToClose = false; // Запрещаем закрывать пробелом во время теста
        
    //     currTestActivator.correctAnswers = 0;
    //     currTestActivator.currentQuestion = 0;
    //     ResponseHandler.onResponsePicked.AddListener(currTestActivator.OnChoseAnswer);
    //     CloseResults();

    //     TestGoing = true;
        
    //     // Подписываемся на событие завершения диалога
    //     DialogueUI.OnDialogueClosed += OnTestDialogueComplete;
    //     Player.playerInstance.DialogueUI.showDialogue(currTestActivator.dialogueObject, currTestActivator.dialogueObject.name);
    }

    private void OnTestDialogueComplete()
    {
        // Отписываемся от события
        DialogueUI.OnDialogueClosed -= OnTestDialogueComplete;
        // Разрешаем снова закрывать пробелом после завершения теста
        allowSpaceToClose = true;
    }

	public void AddTestActivator(TestActivator testActivator)
	{
		tests.TryAdd(testActivator, testActivator.correctAnswers);
	}

	// DATA
	public void SaveData(ref GameData  gameData)
	{
		gameData.testsInStorage = tests;
	}
	public void LoadData(GameData gameData)
	{
		tests = gameData.testsInStorage;
	}
}
