using System;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable, IDataPersistence
{
    [SerializeField] public DialogueObject dialogueObject;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabMother;
    [SerializeField] private string id;

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
        _messanger.addNewChat(this);
    }
    
    // DATA
    public void LoadData(GameData gameData)
    {
        gameData.dialogueObjInteracted.TryGetValue(id, out Interacted);
        if (Interacted)
        {
            _messanger.addNewChat(this);
        }
    }

    public void SaveData(ref GameData gameData)
    {
        if (gameData.dialogueObjInteracted.ContainsKey(id))
        {
            gameData.dialogueObjInteracted.Remove(id);
        }
        gameData.dialogueObjInteracted.Add(id, Interacted);
    }
}