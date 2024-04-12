using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private int maxLives = 3;
    private int currentLives = 3;

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        if (currentLives <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player has died");
    }
}
