using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EasterEggManager easterEggManager = FindObjectOfType<EasterEggManager>();
            if (easterEggManager != null)
            {
                easterEggManager.IncreaseEasterEggCount();
                Destroy(gameObject); // Опционально: уничтожить пасхалку после подбора
            }
        }
    }

}
