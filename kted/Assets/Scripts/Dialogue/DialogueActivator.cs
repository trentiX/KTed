using System;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable, IDataPersistence
{
    [SerializeField] public DialogueObject dialogueObject;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabMother;
    [SerializeField] private string id;
    [SerializeField] private int interactionTurn;


    [ContextMenu("Generate guid for id")]
    public void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    public bool Interacted = false;
    private GameObject sprite;
    private Messanger _messanger;

    private void Start()
    {
        _messanger = FindObjectOfType<Messanger>();
    }

    private void OnEnable()
    {
        Messanger.OnChatAdd.AddListener(AddChatAgain);
    }

    private void OnDisable()
    {
        Messanger.OnChatAdd.RemoveListener(AddChatAgain);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            var position = prefabMother.position;
            sprite = Instantiate(prefab, new Vector3(position.x , position.y + 0.75f), prefabMother.rotation, prefabMother);
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Destroy(sprite);
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        player.DialogueUI.showDialogue(dialogueObject, dialogueObject.name);
        Interacted = true;
        
        interactionTurn = _messanger.generalTurn + 1;
        _messanger.addChatAgain = false;
        _messanger.AddNewChat(this);
    }

    private void AddChatAgain()
    {
        _messanger = FindObjectOfType<Messanger>();

        if (interactionTurn != _messanger.generalTurn + 1) return;
        _messanger.addChatAgain = true;
        _messanger.AddNewChat(this);
    }
    
    // DATA
    public void LoadData(GameData gameData)
    {
        if (gameData == null)
        {
            Debug.LogError("GameData is null.");
            return;
        }

        if (gameData.dialogueObjInteracted == null)
        {
            Debug.LogError("dialogueObjInteracted is null.");
            return;
        }

        if (gameData.dialogueObjInteracted.TryGetValue(id, out var interactionData))
        {
            if (interactionData != null && interactionData.TryGetValue(true, out var turn))
            {
                Interacted = true;
                interactionTurn = turn;

                // If the object was interacted with, add it to the messenger chat
                if (_messanger != null)
                {
                    AddChatAgain();
                }
                else
                {
                    Debug.Log("messenger is null");
                    AddChatAgain();
                }
            }
        }
        else
        {
            Debug.LogError($"No interaction data found for id: {id}");
        }
    }
    
    public void SaveData(ref GameData gameData)
    {
        // Create or update the inner dictionary
        var interactionData = new SerializableDictionary<bool, int>();
        interactionData.Add(Interacted, interactionTurn);

        // Ensure the outer dictionary is initialized
        if (gameData.dialogueObjInteracted == null)
        {
            gameData.dialogueObjInteracted = new SerializableDictionary<string, SerializableDictionary<bool, int>>();
        }

        // Debug information
        Debug.Log($"Saving data for ID: {id}");
        Debug.Log($"Interacted: {Interacted}, InteractionTurn: {interactionTurn}");

        // Update or add the interaction data for the current id
        if (gameData.dialogueObjInteracted.ContainsKey(id))
        {
            gameData.dialogueObjInteracted[id] = interactionData;
            Debug.Log($"Updated existing entry for ID: {id}");
        }
        else
        {
            gameData.dialogueObjInteracted.Add(id, interactionData);
            Debug.Log($"Added new entry for ID: {id}");
        }
    }
}