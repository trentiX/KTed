using UnityEngine;

public class PictureBoxActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject pictureBoxMenu;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabMother;
    [SerializeField] private Sprite photo;
    [SerializeField] private string text;


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
            if (player.Interactable is PictureBoxActivator pictureBoxActivator && pictureBoxActivator == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        player.PictureBoxUI.showPictureBox(photo, text);
    }
}