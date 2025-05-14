using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            collision.gameObject.GetComponent<Enemy>()?.TakeDamage(1); // Assuming the enemy has a TakeDamage method   
        }
        else if (collision.gameObject.GetComponent<PlayerShooter>() != null)
        {
            collision.gameObject.GetComponent<PlayerShooter>()?.TakeDamage(1); // Assuming the player has a Take
        }
        else 
        {
            Debug.Log("Bullet hit something else: " + collision.gameObject.name);
        }
    }
}
