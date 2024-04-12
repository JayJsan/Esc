using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentProjectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D projectileCollider;

    [Header("Projectile Configuration")]
    public float projectileSpeed = 5f;
    public float projectileLifetime = 5f;
    public int projectileDamage = 1;

    private void Start()
    {
        rb.velocity = transform.up * projectileSpeed;
        if (gameObject != null)
            Destroy(gameObject, projectileLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(projectileDamage);
            // get direction from projectile to player
            Vector2 direction = (Vector2)other.transform.position - rb.position;
            damagable.TakeKnockback(10f, direction);

            if (gameObject != null)
                Destroy(this);
        }    
    }
    
    public int GetDamage()
    {
        return projectileDamage;
    }
}
