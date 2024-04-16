using UnityEngine;

public class LocationsManager : MonoBehaviour
{
    [SerializeField] private DialogueActivator[] KanishItems;
    [SerializeField] private DialogueActivator[] AkhmetItems;
    [SerializeField] private DialogueActivator[] AmreItems;
    [SerializeField] private DialogueActivator[] AlikhanItems;

    [SerializeField] private LocationCompleted locationCompleted; // Assign this in the Inspector

    public void CheckIfCompleted(string name)
    {
        switch (name)
        {
            case "Каныш Сатпаев":
                if (Cheking(KanishItems))
                {
                    CompleteLocation();
                }
                break;
            case "Ахмет Байтурсынов":
                if (Cheking(AkhmetItems))
                {
                    CompleteLocation();
                }
                break;
            case "Амре Кашаубаев":
                if (Cheking(AmreItems))
                {
                    CompleteLocation();
                }
                break;
            case "Алихан Бокейханов":
                if (Cheking(AlikhanItems))
                {
                    CompleteLocation();
                }
                break;
            default:
                Debug.LogWarning("Unknown location name: " + name);
                break;
        }
    }

    private bool Cheking(DialogueActivator[] items)
    {
        foreach (var item in items)
        {
            if (!item.Interacted)
            {
                return false; // Location is not completed
            }
        }
        return true; // All items are interacted, location is completed
    }

    private void CompleteLocation()
    {
        if (locationCompleted != null)
        {
            Debug.Log("Location Completed: " + locationCompleted.name);
            locationCompleted.LocationCompletedAnim();
        }
        else
        {
            Debug.LogWarning("LocationCompleted component not assigned to LocationsManager.");
        }
    }
}
