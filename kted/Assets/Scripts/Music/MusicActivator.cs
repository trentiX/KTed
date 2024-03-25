using UnityEngine;

public class MusicActivator : MonoBehaviour, IInteractable
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            if (player.Interactable is MusicPlayer musicPlayer && musicPlayer == this)
            {
                player.Interactable = null;
            }
        }
    }
    public void Interact(Player player)
    {
        player.AudioManager.PlayMusic();
    }
}
