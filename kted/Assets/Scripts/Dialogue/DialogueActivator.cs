using System;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
	[Header("Dialogue activator props")]
	[SerializeField] public DialogueObject dialogueObject;
	[SerializeField] private GameObject prefab;
	[SerializeField] private Transform prefabMother;
	[SerializeField] private string id;
	
	[HideInInspector] public static event Action<DialogueObject, string> onInteracted;


	[ContextMenu("Generate guid for id")]
	public void GenerateGuid()
	{
		id = System.Guid.NewGuid().ToString();
	}
	
	[HideInInspector] public bool Interacted = false;
	[HideInInspector] public Response chooseResponse;
	private GameObject sprite;	
	private Messenger _messenger;
	private RingManager _ringManager;
	private Ktedwork _ktedwork;

	private void Start()
	{
		_messenger = FindObjectOfType<Messenger>();
		_ktedwork = FindObjectOfType<Ktedwork>();
		_ringManager = FindObjectOfType<RingManager>();
	}

	private void OnEnable()
	{
		ResponseHandler.onResponsePicked.AddListener(OnPickedResponse);
	}

	private void OnDisable()
	{
		ResponseHandler.onResponsePicked.AddListener(OnPickedResponse);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
		{
			var position = prefabMother.position;
			
			if (Player.interactButton != null) Player.interactButton.SetActive(true);

			Player.skipButton.SetActive(true);
			
			sprite = Instantiate(prefab, new Vector3(position.x , position.y + 0.75f), prefabMother.rotation, prefabMother);
			player.Interactable = this;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
		{
			if (Player.interactButton != null) Player.interactButton.SetActive(false);
			Player.skipButton.SetActive(false);
			Destroy(sprite);
			if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
			{
				player.Interactable = null;
			}
		}
	}

	public void Interact(Player player)
	{    
		if (DialogueUI.instance.DialogueOpen) return;

		if (_ktedwork.questIsGoing && _ktedwork.questChars.Contains(this))
		{
			DialogueObject dialogueObject = _ktedwork._currQuest.questDialogue[_ktedwork._currQuest.questDialogueActivators.IndexOf(this)];
			player.DialogueUI.showDialogue
			(dialogueObject,dialogueObject.name);
			_ktedwork.Interacted(this);
		}
		else
		{
			player.DialogueUI.showDialogue(dialogueObject, dialogueObject.name);
			Interacted = true;
			onInteracted?.Invoke(_ringManager._dialogueObjectOnInteracted, "DialogueAction");
			
			if (SmartPhone.instance != null ) _messenger.AddNewChat(this);
		}
	}

	private void OnPickedResponse(Response response)
	{
		chooseResponse = response;
	}
}