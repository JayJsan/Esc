using System.Collections;

public interface IDamagable
{
    void TakeDamage(int damage);
    void Die();
    IEnumerator InvincibilityTimer(float duration);
}