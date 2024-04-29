using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [Header("Configuration")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private float flashDuration = 0.2f;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Material whiteMaterial;
    private Material defaultMaterial;
    // ==============================
    private int currentLives = 3;
    private bool isInvincible = false;

    private void Start()
    {
        currentLives = maxLives;
        defaultMaterial = spriteRenderer.material;
        UIController.Instance.SetPlayerHealth(this);
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        currentLives -= damage;
        if (currentLives <= 0)
        {
            Die();
        }
        UIController.Instance.UpdateHealthText();

        if (!isInvincible)
        {
            StartCoroutine(InvincibilityTimer(invincibilityDuration));
            StartCoroutine(FlashSprite(flashDuration));
        }
    }

    public void TakeKnockback(float force, Vector2 direction)
    {
        direction.Normalize();
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void Die()
    {
        Debug.Log("Player has died");
        UIController.Instance.LoseGame();
    }

    public int GetHealth()
    {
        return currentLives;
    }

    public IEnumerator InvincibilityTimer(float duration)
    {
        isInvincible = true;

        yield return new WaitForSeconds(duration);

        isInvincible = false;
    }

    public IEnumerator FlashSprite(float duration)
    {
        spriteRenderer.material = whiteMaterial;
        yield return new WaitForSeconds(duration);
        spriteRenderer.material = defaultMaterial;
    }
}
