using TMPro;
using UnityEngine;

public class TestHandler : MonoBehaviour, IDataPersistence
{
	[SerializeField] private GameObject testResultsPanel;
	[SerializeField] private TextMeshProUGUI nameOfTest;
	[SerializeField] private TextMeshProUGUI resultsOfTheTest;

	public SerializableDictionary<TestActivator, int> tests;
	public bool TestOpen = false;
	public bool TestGoing = false;
	public TestActivator currTestActivator;
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CloseResults();
		}
	}
	public void AddTestActivator(TestActivator testActivator)
	{
		tests.TryAdd(testActivator, testActivator.correctAnswers);
	}
	
	public void ShowResults(TestActivator testActivator)
	{
		TestOpen = true;
		currTestActivator = testActivator;
		testResultsPanel.SetActive(true);
		nameOfTest.text = testActivator.nameOfTest;
		resultsOfTheTest.text = testActivator.correctAnswers + " / " + testActivator.testAnswers.Count;
	}
	
	public void CloseResults()
	{
		testResultsPanel.SetActive(false);
		TestOpen = false;
	}
	
	public void TestAgain()
	{
		if (currTestActivator != null)
		{
			currTestActivator.correctAnswers = 0;
			currTestActivator.currentQuestion = 0;
			ResponseHandler.onResponsePicked.AddListener(currTestActivator.OnChoseAnswer);
		}

		CloseResults();

		TestGoing = true;
		FindObjectOfType<Player>().DialogueUI.showDialogue(currTestActivator.dialogueObject, currTestActivator.dialogueObject.name);
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
