using UnityEngine;

public class MusicActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject musicMenu;
    [SerializeField] private GameObject prefab;

    [SerializeField] private Transform prefabMother;
    

    private GameObject sprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            var position = prefabMother.position;
			Player.interactButton.SetActive(true);         
			Player.skipButton.SetActive(true);   
            sprite = Instantiate(prefab, new Vector3(position.x , position.y + 0.75f), prefabMother.rotation, prefabMother);
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
			Player.interactButton.SetActive(false);
			Player.skipButton.SetActive(false);
            Destroy(sprite);
            if (player.Interactable is MusicActivator musicActivator && musicActivator == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        player.MusicUI.showMusicBox();
    }
}
