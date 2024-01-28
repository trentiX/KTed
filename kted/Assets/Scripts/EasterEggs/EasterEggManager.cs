using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EasterEggManager : MonoBehaviour
{
    public int easterEggsCount = 0;
    public static UnityEvent OnEasterEggPickupUpdated = new UnityEvent();

    public void IncreaseEasterEggCount()
    {
        easterEggsCount++;
        OnEasterEggPickupUpdated.Invoke();
    }

    public int GetEasterEggCount()
    {
        return easterEggsCount;
    }
}
