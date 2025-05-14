using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    void Start()
    {
        health = 3; // Example health value
        damage = 1; // Example damage value
        speed = 2f; // Example speed value
    }
    
    public override void Move()
    {
        if (target == null) return;

        // Направление к игроку
        Vector3 direction = (target.position - transform.position).normalized;

        // Двигаем врага
        transform.position += direction * speed * Time.deltaTime;

        // Поворот к игроку (опционально, если хочешь чтобы он смотрел на игрока)
        transform.forward = direction;
    }
}
