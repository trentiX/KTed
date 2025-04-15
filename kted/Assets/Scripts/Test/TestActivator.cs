using System.Collections.Generic;
using UnityEngine;

public class TestActivator : MonoBehaviour, IInteractable
{
	[SerializeField] public DialogueObject dialogueObject;
	[SerializeField] public string nameOfTest;
	[SerializeField] private GameObject prefab;
	[SerializeField] private GameObject interactButton;
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
			correctAnswers = corr;
		}
	}
	
	private void OnEnable()
	{
		ResponseHandler.onResponsePicked.AddListener(OnChoseAnswer);
	}

	private void OnDisable()
	{
		ResponseHandler.onResponsePicked.RemoveListener(OnChoseAnswer);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
		{
			var position = prefabMother.position;
			interactButton.SetActive(true);
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
			interactButton.SetActive(false);
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
		if (correctAnswers != 0)
		{
			testHandler.ShowResults(this);
		}
		else
		{
			currentQuestion = 0;
			testHandler.TestGoing = true;
			testHandler.currTestActivator = this;
			ResponseHandler.onResponsePicked.AddListener(OnChoseAnswer);
			player.DialogueUI.showDialogue(dialogueObject, dialogueObject.name);
		}
	}


	public void OnChoseAnswer(Response answer)
	{
		if (!testHandler.TestGoing) return;

		choseAnswer = answer;

		if (currentQuestion > 0 && currentQuestion - 1 < testAnswers.Count)
		{
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
		if (testHandler.TestGoing)
		{
			testHandler.ShowResults(this);
			testHandler.TestGoing = false;
			testHandler.tests[this] = correctAnswers;
			ResponseHandler.onResponsePicked.RemoveListener(OnChoseAnswer);
		}
	}
}