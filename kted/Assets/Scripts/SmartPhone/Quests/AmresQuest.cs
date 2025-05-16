using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmresQuest : Quest
{
	// Serialization
	[SerializeField] private List<GameObject> _neededDisks;

	// Variables
	private SerializableDictionary<GameObject, bool> _signedDisks;
	private int _collectedDisks = 0;
	
	public static AmresQuest instance;
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
	{        
		Ktedwork.OnQuestCollected.AddListener(CollectDisk);
		
		_signedDisks = new SerializableDictionary<GameObject, bool>();
		foreach (var character in _neededDisks)
		{
			_signedDisks.TryAdd(character, false);
			character.SetActive(false);
		}
	}

	private void OnDisable()
	{
		Ktedwork.OnQuestCollected.RemoveListener(CollectDisk);
	}

	private void CollectDisk(GameObject gameObject)
	{
		_signedDisks[gameObject] = true;

		_collectedDisks++;

		if (_collectedDisks == _neededDisks.Count)
		{
			questIsOver = true;
		}
	}

	public override void StartQuest()
	{
		base.StartQuest();
		Ktedwork.instance.objectsToInteract = new List<GameObject>(_neededDisks);
		SpawnDisks();
	}

	public override void SubmitQuest()
	{
		base.SubmitQuest();
		Ktedwork.instance.questChars.Clear();
		_signedDisks.Clear();
	}
	
	public void SpawnDisks()
	{
	    foreach (var disk in _neededDisks)
        {
            disk.SetActive(true);
        }
	}
	public void CheckOnComplete()
	{
        _ktedwork = FindObjectOfType<Ktedwork>();
	    if (_ktedwork._quests.ContainsKey(this) && _ktedwork._quests[this] == true)
        {
            EasterEggManager easterEggManager = FindObjectOfType<EasterEggManager>();
            foreach (var disk in _neededDisks)
            {
                easterEggManager.MusicPlatePickedUp(disk);
            }
        }
	}
}
