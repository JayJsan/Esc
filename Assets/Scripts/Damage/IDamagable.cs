using System.Collections;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage);
    void Die();
    IEnumerator InvincibilityTimer(float duration);
    public void TakeKnockback(float force, Vector2 direction);
    
}