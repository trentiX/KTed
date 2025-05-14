using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // Serialization
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;


    // Variables
    public int health = 3; // Player's health
    public int damage = 1; // Damage dealt to enemies
    public float speed = 5f; // Player's movement speed
    
    
    // Code
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    
    private void Shoot()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(bulletSpawnPoint.up * 10f, ForceMode2D.Impulse); // Adjust the force as needed
        }
        Destroy(newBullet, 3f); // Destroy the bullet after 2 seconds to avoid cluttering the scene
    }
    
    public void TakeDamage(int amount)
    {
        // Implement player damage logic here
        Debug.Log("Player took damage: " + amount);
    }
}
