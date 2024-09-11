using UnityEngine;
using UnityEngine.Events;

public class EasterEggManager : MonoBehaviour
{
    [SerializeField] private GameObject[] musicPlates;
    public int easterEggsCount = 0;
    public static UnityEvent OnEasterEggPickupUpdated = new UnityEvent();
    private MusicUI _musicUI;

    private void Awake()
    {
        _musicUI = FindObjectOfType<MusicUI>(); // Find and assign the MusicUI component
        if (_musicUI == null)
        {
            Debug.LogError("MusicUI component not found in scene!");
        }
    }

    public void IncreaseEasterEggCount()
    {
        easterEggsCount++;
        OnEasterEggPickupUpdated.Invoke(); // Notify listeners that easter egg count has been updated
    }

    public void SmartPhonePickedUp()
    {
        OnEasterEggPickupUpdated.Invoke();
    }
    
    public void MusicPlatePickedUp(GameObject musicPlate)
    {
        if (_musicUI == null)
        {
            Debug.LogError("MusicUI component not assigned to EasterEggManager!");
            return;
        }

        // Enable specific UI button based on the picked music plate
        for (int i = 0; i < musicPlates.Length; i++)
        {
            if (musicPlate == musicPlates[i])
            {
                if (i < _musicUI.buttons.Length)
                {
                    _musicUI.buttons[i].interactable = true; // Enable the corresponding UI button
                }
                else
                {
                    Debug.LogError("Invalid music plate index for UI button assignment!");
                }
                break;
            }
        }
    }

    public int GetEasterEggCount()
    {
        return easterEggsCount;
    }
}