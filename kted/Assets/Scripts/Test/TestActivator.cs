using System.Collections.Generic;
using UnityEngine;

public class TestActivator : MonoBehaviour, IInteractable
{
	[SerializeField] public DialogueObject dialogueObject;
	[SerializeField] public string nameOfTest;
	[SerializeField] private GameObject prefab;
	[SerializeField] private int questionAmount;
	[SerializeField] private Transform prefabMother;
	[SerializeField] public List<string> testAnswers; 
	
	private GameObject sprite;
	[HideInInspector] public Response choseAnswer;
	public int correctAnswers = 0;
	public int currentQuestion = 0;
	private TestHandler testHandler;

	private void Start()
	{
		testHandler = FindObjectOfType<TestHandler>();
		testHandler.AddTestActivator(this);
		
		testHandler.tests.TryGetValue(this, out var corr);
		{
			Debug.Log(gameObject.name + "Already has correct answers: " + corr);
			correctAnswers = corr;
		}
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
		{
			var position = prefabMother.position;
			Player.interactButton.SetActive(true);
			Player.skipButton.SetActive(true);

			sprite = Instantiate(prefab, new Vector3(position.x, position.y + 0.75f), prefabMother.rotation,
				prefabMother);
			player.Interactable = this;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
		{
			Player.interactButton.SetActive(false);
			Player.skipButton.SetActive(false);
			Destroy(sprite);
			if (player.Interactable is KeysActivator keysActivator && keysActivator == this)
			{
				player.Interactable = null;
			}
		}
		testHandler.TestGoing = false;
	}

	public void Interact(Player player)
	{
		currentQuestion = 0;
		testHandler.TestGoing = true;
		testHandler.currTestActivator = this;
		ResponseHandler.onResponsePicked.RemoveListener(OnChoseAnswer); // 👈 Удаляем старую подписку
		ResponseHandler.onResponsePicked.AddListener(OnChoseAnswer);
		player.DialogueUI.showDialogue(dialogueObject, dialogueObject.name);
	}


	public void OnChoseAnswer(Response answer, DialogueObject dialogueObject)
	{
		// Проверяем, что это текущий активный тест
		if (!testHandler.TestGoing || testHandler.currTestActivator != this) 
			return;

		choseAnswer = answer;
		Debug.Log("Chose answer: " + choseAnswer.ResponseText);
		if (currentQuestion > 0 && currentQuestion - 1 < testAnswers.Count)
		{
			if (choseAnswer.ResponseText == "Откажусь")
			{
			    TestOver();
			}
			if (choseAnswer.ResponseText == testAnswers[currentQuestion - 1])
			{
				correctAnswers++;
			}
		}

		currentQuestion++;

		if (currentQuestion == testAnswers.Count + 1)
		{
			Debug.Log("Test is over");
			TestOver();
		}
	}
	
	private void TestOver()
	{
		if (testHandler.TestGoing && testHandler.currTestActivator == this)
		{
			Debug.Log("Test is over, correct answers: " + correctAnswers);
			testHandler.TestGoing = false;
			testHandler.tests[this] = correctAnswers;
			ResponseHandler.onResponsePicked.RemoveListener(OnChoseAnswer);
			
			// Показываем результаты после завершения теста
			testHandler.ShowResults(this);
		}
	}
}