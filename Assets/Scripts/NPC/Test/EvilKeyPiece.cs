using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKeyPiece : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private EnemyHealth health;
    [Header("Configuration")]
    [SerializeField] private bool randomiseDirectionSpread = false;
    [SerializeField] private float spreadAngle = 0f;
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 10f;
    // ========== Private Variables ==========

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;

        if (randomiseDirectionSpread)
        {
            float angle = Random.Range(-spreadAngle, spreadAngle);
            direction = Quaternion.Euler(0, 0, angle) * direction;
        }

        direction.Normalize();
        Vector2 force = direction * speed;
        rb.AddForce(force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (health.IsDead()) return;

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                // get direction from key piece to player
                Vector2 direction = (Vector2)other.transform.position - rb.position;
                playerHealth.TakeKnockback(knockbackForce, direction);
            }
        }
    }
}
