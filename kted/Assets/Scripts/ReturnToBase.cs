using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToBase : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabMother;
    [SerializeField] private GameObject interactButton;
    
    private GameObject sprite;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
			interactButton.SetActive(true);
            var position = prefabMother.position;
            sprite = Instantiate(prefab, new Vector3(position.x , position.y + 0.75f), prefabMother.rotation, prefabMother);
            player.Interactable = this;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
			interactButton.SetActive(false);
            Destroy(sprite);
            if (player.Interactable is ReturnToBase returnToBase && returnToBase == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        prefabMother.position = new Vector3(22, -2, 0);
    }
}
