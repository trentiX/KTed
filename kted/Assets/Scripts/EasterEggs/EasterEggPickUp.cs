using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EasterEggPickUp : MonoBehaviour
{
    [SerializeField] private GameObject pickUpFX;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EasterEggManager easterEggManager = FindObjectOfType<EasterEggManager>();
            if (easterEggManager != null)
            {
                easterEggManager.IncreaseEasterEggCount();
                GameObject fx = Instantiate(pickUpFX, transform.position, quaternion.identity);
                Destroy(gameObject); // Опционально: уничтожить пасхалку после подбора
                Destroy(fx, 1.9f);
            }
        }
    }

}
