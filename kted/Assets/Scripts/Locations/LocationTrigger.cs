using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    [SerializeField] private string LocationName;
    private LocationText _locationText;
    private Player _player;

    private void Awake()
    {
        _locationText = FindObjectOfType<LocationText>();
        _player = FindObjectOfType<Player>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShowText(LocationName);
            if (_player != null)
            {
                _player.location = LocationName;
            }
        }
    }

    private void ShowText(string nameOfLocation)
    {
        if (_locationText != null)
        {
            _locationText.locNameAnim(nameOfLocation);
        }
    }
}