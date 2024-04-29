using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject deathParticlesPrefab;
    [Header("Configuration")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float flashDuration = 0.2f;
    // ========== Private Variables ==========
    private bool isInvincible = false;
    private Material defaultMaterial;
    private int currentHealth = 3;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
                return;
            }
            StartCoroutine(InvincibilityTimer(invincibilityDuration));
            StartCoroutine(FlashSprite(flashDuration));
        }
    }

    public void Die()
    {
        isDead = true;
        spriteRenderer.material = flashMaterial;
        GameObject deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity, this.transform);
        float deathDuration = deathParticles.GetComponent<ParticleSystem>().main.duration;
        StartCoroutine(DelayDisableSprite(deathDuration / 4f));
        Destroy(gameObject, deathDuration);
    }

    public IEnumerator DelayDisableSprite(float duration)
    {
        yield return new WaitForSeconds(duration);
        spriteRenderer.enabled = false;
    }

    public IEnumerator InvincibilityTimer(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    public IEnumerator FlashSprite(float duration)
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = defaultMaterial;
    }

    public void TakeKnockback(float force, Vector2 direction)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
