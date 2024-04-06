using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    [SerializeField] private string LocationName;
    private LocationText _locationText;
    
    private void Awake()
    {
        _locationText = FindObjectOfType<LocationText>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           ShowText(LocationName);
        }
    }

    private void ShowText(string nameOfLocation)
    {
        _locationText.locNameAnim(nameOfLocation);
    }
}
