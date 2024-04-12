using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentProjectile : MonoBehaviour, IDamager
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
            DealDamage(damagable);
            if (gameObject != null)
                Destroy(this);
        }    
    }

    public void DealDamage(IDamagable target)
    {
        target.TakeDamage(projectileDamage);
    }

    public int GetDamage()
    {
        return projectileDamage;
    }
}
