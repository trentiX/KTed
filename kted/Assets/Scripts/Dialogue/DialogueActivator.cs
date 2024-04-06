using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabMother;
    [SerializeField] private string name;

    private GameObject sprite;

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
        player.DialogueUI.showDialogue(dialogueObject, name);
    }
}
