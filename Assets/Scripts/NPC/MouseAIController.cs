using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAIController : NPCAIController
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Mouse Configuration")]
    [SerializeField] private float attackAttemptCooldown = 3f;
    [SerializeField] private float grabWindow = 3f;
    [SerializeField] private float grabRange = 3f;
    [SerializeField] private float grabSpeed = 20f;
    [SerializeField] private float cursorSpeed = 10f;
    [SerializeField] private Rigidbody2D rb;
    //
    private bool canAttack = true;
    private bool inCooldown = false;
    private bool isGrabWindow = false;

    protected override void HandleStateCheck()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        if (inCooldown)
        {
            npcState = NPCState.Idle;
            return;
        }

        if (distance >= attackRange && distance <= chaseRange)
        {
            npcState = NPCState.Chasing;
        }
        else if (distance <= attackRange)
        {
            npcState = NPCState.Attacking;
        }
        else if (distance >= chaseRange && distance <= patrolRange)
        {
            npcState = NPCState.Patrolling;
        }
        else if (distance > patrolRange)
        {
            npcState = NPCState.Idle;
        }
    }

    protected override void HandleNPCState()
    {
        switch (npcState)
        {
            case NPCState.Idle:
                rb.velocity = Vector2.zero;
                break;
            case NPCState.Chasing:
                ChaseTarget();
                break;
            case NPCState.Attacking:
                AttackTarget();
                break;
            case NPCState.Patrolling:
                break;
            default:
                break;
        }
    }

    protected override void ChaseTarget()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            direction.Normalize();

            float randomisedGrabSpeed = grabSpeed + Random.Range(0, grabSpeed / 2);

            Vector2 force = (Vector2.Distance(target.position, transform.position) <= grabRange) 
            ? direction * randomisedGrabSpeed : direction * speed;
            transform.position += (Vector3)force * Time.deltaTime;
        }
    }

    protected override void AttackTarget()
    {
        if (target != null)
        {
            if (canAttack)
            {
                animator.SetBool("IsAttacking", true);
                Vector2 direction = (Vector2)target.position - (Vector2)transform.position;

                // randomise directino angle
                direction.x += Random.Range(-1f, 1f);

                direction.Normalize();

                float randomisedGrabSpeed = grabSpeed + Random.Range(0, grabSpeed / 2);

                rb.AddForce(direction * randomisedGrabSpeed, ForceMode2D.Impulse);

                if (!isGrabWindow)
                    StartCoroutine(GrabWindow());
            }
        }
    }

    private IEnumerator GrabWindow()
    {
        float randomisedGrabWindow = grabWindow + Random.Range(0, grabWindow / 2);
        isGrabWindow = true;
        yield return new WaitForSeconds(randomisedGrabWindow);
        canAttack = false;
        isGrabWindow = false;
        animator.SetBool("IsAttacking", false);
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        inCooldown = true;
        animator.SetBool("InCooldown", true);

        float randomisedCooldown = attackAttemptCooldown + Random.Range(0, attackAttemptCooldown / 2);

        yield return new WaitForSeconds(randomisedCooldown);
        animator.SetBool("InCooldown", false);
        inCooldown = false;
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (health.IsDead() || !isGrabWindow || inCooldown) return;

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                // get direction from key piece to player
                Vector2 direction = (Vector2)other.transform.position - (Vector2)transform.position;
                playerHealth.TakeKnockback(2f, direction);
            }
        }
    }
}
