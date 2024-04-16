using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationsManager : MonoBehaviour
{
    // Dictionary to store arrays of DialogueActivators and completion status for each station
    private Dictionary<string, (DialogueActivator[], bool)> stationCompletionStatus = new Dictionary<string, (DialogueActivator[], bool)>();

    [SerializeField] private LocationCompleted locationCompleted; // Assign this in the Inspector

    // Assign these arrays in the Inspector with corresponding DialogueActivators for each station
    [SerializeField] private DialogueActivator[] KanishItems;
    [SerializeField] private DialogueActivator[] AkhmetItems;
    [SerializeField] private DialogueActivator[] AmreItems;
    [SerializeField] private DialogueActivator[] AlikhanItems;
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        // Populate the dictionary with station names and corresponding DialogueActivator arrays
        stationCompletionStatus.Add("Каныш Сатпаев", (KanishItems, false));
        stationCompletionStatus.Add("Ахмет Байтурсынов", (AkhmetItems, false));
        stationCompletionStatus.Add("Амре Кашаубаев", (AmreItems, false));
        stationCompletionStatus.Add("Алихан Бокейханов", (AlikhanItems, false));
    }

    public void CheckIfCompleted(string name)
    {
        if (stationCompletionStatus.ContainsKey(name))
        {
            var (dialogueActivators, isCompleted) = stationCompletionStatus[name];

            if (!isCompleted && Cheking(dialogueActivators))
            {
                // Mark the station as completed if all DialogueActivators are interacted with
                stationCompletionStatus[name] = (dialogueActivators, true);
                CompleteLocation(name);
            }
        }
        else
        {
            Debug.LogWarning("Unknown location name: " + name);
        }
    }

    private bool Cheking(DialogueActivator[] items)
    {
        foreach (var item in items)
        {
            if (!item.Interacted)
            {
                return false; // Station is not completed
            }
        }
        _audioSource.Play();
        return true; // All DialogueActivators are interacted with, station is completed
    }

    private void CompleteLocation(string name)
    {
        if (locationCompleted != null)
        {
            Debug.Log("Location Completed: " + name);
            locationCompleted.LocationCompletedAnim();
        }
        else
        {
            Debug.LogWarning("LocationCompleted component not assigned to LocationsManager.");
        }
    }
}
