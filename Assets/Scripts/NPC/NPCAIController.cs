using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public enum NPCState
{
    Idle,
    Chasing,
    Attacking,
    Patrolling
}

public class NPCAIController : MonoBehaviour
{
    public NPCState npcState = NPCState.Idle;
    [Header("References")]
    [SerializeField] protected Transform target;
    [SerializeField] protected EnemyHealth health;
    [Header("Configuration")]
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float chaseRange = 5f;
    [SerializeField] protected float patrolRange = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // target = PlayerMovementStateController.Instance.transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleStateCheck();
        HandleNPCState();
    }

    protected virtual void HandleStateCheck()
    {
        if (Vector2.Distance(transform.position, target.position) <= chaseRange)
        {
            npcState = NPCState.Chasing;
        }
        else if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            npcState = NPCState.Attacking;
        }
        else if (Vector2.Distance(transform.position, target.position) > chaseRange)
        {
            npcState = NPCState.Idle;
        }
    }

    protected virtual void HandleNPCState()
    {
        switch (npcState)
        {
            case NPCState.Idle:
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

    protected virtual void ChaseTarget()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            direction.Normalize();
            Vector2 force = direction * speed;
            transform.position += (Vector3)force * Time.deltaTime;
        }
    }

    protected virtual void AttackTarget()
    {
        if (target != null)
        {
            
        }
    }

    public virtual void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
