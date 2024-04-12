using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBurstAbility : MonoBehaviour, IAbility
{
    [Header("Reference")]
    [SerializeField] private ParticleSystem airBurstParticles;
    [Header("Configuration")]
    [SerializeField] private float burstRadius = 5f;
    [SerializeField] private float burstForce = 10f;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float duration = 0.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Execute()
    {
        Debug.Log("Air Burst Ability Activated");
        // circle overlap with radius at centre of player
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, burstRadius);
        // check any rigidbodies in the circle
        // apply force to rigidbodies with direction away from player

        airBurstParticles.gameObject.SetActive(true);

        foreach (Collider2D collider in hit)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

            if (collider.gameObject == gameObject)
                continue;

            if (rb != null)
            {
                Vector2 direction = rb.position - (Vector2)transform.position;
                rb.AddForce(direction.normalized * burstForce, ForceMode2D.Impulse);
            }
        }
    }

    public float GetAbilityDuration()
    {
        return duration;
    }

    public float GetAbilityCooldown()
    {
        return cooldown;
    }

    // Draw gizmos of the burst radius
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, burstRadius);
    }
}
