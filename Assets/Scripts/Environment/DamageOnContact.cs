using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageOnContact : MonoBehaviour
{
    [Header("Damage Configuration")]
    public int damage = 1;
    public float knockbackForce = 10f;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        DamageEntity(other, damage, knockbackForce);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        DamageEntity(other, damage, knockbackForce);
    }

    private void DamageEntity(Collider2D other, int damage, float knockbackForce)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
            // get direction from projectile to player
            Vector2 direction = (Vector2)other.transform.position - (Vector2)transform.position;
            damagable.TakeKnockback(knockbackForce, direction);
        }    
    }

    private void DamageEntity(Collision2D other, int damage, float knockbackForce)
    {
        IDamagable damagable;
        if (other.collider.TryGetComponent<IDamagable>(out damagable))
        {
            damagable.TakeDamage(damage);
            // get direction from projectile to player
            Vector2 direction = (Vector2)other.transform.position - (Vector2)transform.position;
            damagable.TakeKnockback(knockbackForce, direction);
        }    
    }
}
