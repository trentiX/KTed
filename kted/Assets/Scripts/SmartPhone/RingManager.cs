using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RingManager : MonoBehaviour, IDataPersistence
{
	[Header("Dialogue objects for actions:")] 
	[SerializeField] public DialogueObject _dialogueObjectOnInteracted;
	[SerializeField] public DialogueObject _dialogueObjectEasterEgg;
	
	// Other scripts
	private SmartPhone _smartPhone;
	private Player _player;
	
	// Variables
	private SerializableDictionary<string, bool> ringedActions;
	private bool isGoingToRing = false;
	private void Start()
	{
		_smartPhone = FindObjectOfType<SmartPhone>();
		_player = FindObjectOfType<Player>();
	}
	private void OnEnable()
	{
		EasterEggPickUp.EasterEggPickedUp += Ring;
		DialogueActivator.onInteracted += Ring;
	}

	private void OnDisable()
	{
		EasterEggPickUp.EasterEggPickedUp -= Ring;
		DialogueActivator.onInteracted -= Ring;
	}

	private void Ring(DialogueObject dialogueObject, string action)
	{
		if (_smartPhone.SmartPhonePicked == false) return;
		
		ringedActions.TryGetValue(action, out var alreadyRinged);
		{
			if (alreadyRinged == true) return;
		}

		StartCoroutine(DelayedRing(dialogueObject, action));
	}

	private IEnumerator DelayedRing(DialogueObject dialogueObject, string action)
	{
		if (isGoingToRing) yield break;
		isGoingToRing = true;
		
		// Задержка перед началом звонка
		yield return StartCoroutine(GenerateDelay());

		// Ожидание, пока игрок сможет двигаться
		yield return new WaitUntil(() => _player.canMove());

		// Короткая задержка перед звонком
		yield return new WaitForSeconds(2);

		// Звоним на смартфон
		_smartPhone.Ring(dialogueObject);

		// Добавляем действие в список выполненных
		ringedActions?.Add(action, true);

		// Сбрасываем флаг, позволяющий другим звонкам инициироваться
		isGoingToRing = false;
	}

// Метод для генерации случайной задержки
	private IEnumerator GenerateDelay()
	{
		float delay = Random.Range(5, 7);
		yield return new WaitForSeconds(delay);
	}

	// DATA

	public void LoadData(GameData gameData)
	{
		ringedActions = gameData.ringedActionsInStorage;
	}
	public void SaveData(ref GameData gameData)
	{
		gameData.ringedActionsInStorage = ringedActions;
	}
}
