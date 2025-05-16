using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopersQuest : Quest
{
    	// Serialization
	[SerializeField] private List<GameObject> _neededEastereggs;

	// Variables
	private SerializableDictionary<GameObject, bool> _signedEastereggs;
	private int _collectedDisks = 0;

    private void OnEnable()
	{        
		Ktedwork.OnQuestCollected.AddListener(CollectEasteregg);
		
		_signedEastereggs = new SerializableDictionary<GameObject, bool>();
		foreach (var character in _neededEastereggs)
		{
			_signedEastereggs.TryAdd(character, false);
			character.SetActive(false);
		}
	}

	private void OnDisable()
	{
		Ktedwork.OnQuestCollected.RemoveListener(CollectEasteregg);
	}

	private void CollectEasteregg(GameObject gameObject)
	{
		_signedEastereggs[gameObject] = true;

		_collectedDisks++;

		if (_collectedDisks == _neededEastereggs.Count)
		{
			questIsOver = true;
		}
	}

	public override void StartQuest()
	{
		base.StartQuest();
		Ktedwork.instance.objectsToInteract = new List<GameObject>(_neededEastereggs);
		SpawnEastereggs();
	}

	public override void SubmitQuest()
	{
		base.SubmitQuest();
		Ktedwork.instance.questChars.Clear();
		_signedEastereggs.Clear();
	}
	
	private void SpawnEastereggs()
	{
	    foreach (var disk in _neededEastereggs)
        {
            disk.SetActive(true);
        }
	}
}
