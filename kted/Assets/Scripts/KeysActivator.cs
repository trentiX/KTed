using UnityEngine;
using DG.Tweening;
using TMPro;

public class KeysActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private TextMeshProUGUI[] text;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform prefabMother;

    private GameObject sprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            var position = prefabMother.position;
            sprite = Instantiate(prefab, new Vector3(position.x, position.y + 0.75f), prefabMother.rotation,
                prefabMother);
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Destroy(sprite);
            if (player.Interactable is KeysActivator keysActivator && keysActivator == this)
            {
                player.Interactable = null;
            }
        }
    }

    public void Interact(Player player)
    {
        keysAnim(); // Call keysAnim() directly since we're already inside the KeysActivator class
    }

    public void keysAnim()
    {
        foreach (var label in text)
        {
            label.DOFade(0.75f, 3).OnComplete(() => { label.DOFade(0, 5f).SetEase(Ease.InExpo);});
        }
    }
}