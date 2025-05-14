using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private PlayerShooter playerShooter;

    public int health;
    public int damage;
    public float speed;
    public Transform target;

    private void Awake()
    {
        playerShooter = FindObjectOfType<PlayerShooter>();
        target = playerShooter.transform;
    }

    private void Update()
    {
        Move();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Тут можно добавить анимацию смерти, выпадение предметов и т.д.
        Destroy(gameObject);
    }
    
    public virtual void Move(){}
}
