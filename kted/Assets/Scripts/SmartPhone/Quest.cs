using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
	// Serialization
	[SerializeField] public List<DialogueObject> questDialogue;
	[SerializeField] public List<DialogueActivator> questDialogueActivators;
	[SerializeField] public string taskGiverName;
	[SerializeField] public string taskName;
	[SerializeField] public TextMeshProUGUI completionStatus;
	[SerializeField][TextArea] public string shortDescription;
	[SerializeField][TextArea] public string longDescription;
	[SerializeField][TextArea] public string questObjectives;
	[SerializeField] public int taskReward;
	[SerializeField] public int interactedAmount;
	[SerializeField] public int goalAmount;

	[SerializeField] public Sprite taskGiverImage;

	// Variables
	[HideInInspector] public Ktedwork _ktedwork;
	public bool questIsOver;
	
	// Code
	private void OnEnable()
	{
		_ktedwork = FindObjectOfType<Ktedwork>();
	}
	
	private void OnClick()
	{
		if (Ktedwork.instance._pointerClicked != null) ChangeAlpha(0, Ktedwork.instance._pointerClicked.gameObject);
		Ktedwork.instance.OpenQuest(this);
		Ktedwork.instance._pointerClicked = this;
	}
	
	public virtual void StartQuest()
	{
		questIsOver = false;
		
	 	Ktedwork.instance._currQuest.completionStatus.text = "Активный";
	}

	public virtual void SubmitQuest()
	{
		if (!questIsOver) return;
		questIsOver = true;		
		Ktedwork.instance._currQuest.completionStatus.text = "Выполненный";
	}
	
	private void ChangeAlpha(float value, GameObject chat)
	{
		var color = chat.GetComponent<RawImage>().color;
		color.a = value;
		chat.GetComponent<RawImage>().color = color;
	}
	
	private void Start()
	{
		gameObject.GetComponentInChildren<Image>().sprite = taskGiverImage;
		gameObject.GetComponentInChildren<TextMeshProUGUI>(0).text = taskGiverName;
		gameObject.GetComponentInChildren<TextMeshProUGUI>(1).text = shortDescription;
		
		EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry onClick = new EventTrigger.Entry()
		{
			eventID = EventTriggerType.PointerClick
		};
		
		EventTrigger.Entry onEnter = new EventTrigger.Entry()
		{
			eventID = EventTriggerType.PointerEnter
		};
		
		EventTrigger.Entry onExit = new EventTrigger.Entry()
		{
			eventID = EventTriggerType.PointerExit
		};

		onClick.callback.AddListener((eventData) => OnClick());
		onEnter.callback.AddListener((AbstractEventData) =>
		{
			ChangeAlpha(0.1f, gameObject);
		});
		onExit.callback.AddListener((AbstractEventData) =>
		{
			if(Ktedwork.instance._pointerClicked == this) return;
			ChangeAlpha(0, gameObject);
		});
		
		eventTrigger.triggers.Add(onClick);
		eventTrigger.triggers.Add(onEnter);
		eventTrigger.triggers.Add(onExit);
	}

}

