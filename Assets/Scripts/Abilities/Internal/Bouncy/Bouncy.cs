using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Bouncy", menuName = "Abilities/Internal/Bouncy")]
public class Bouncy : Ability
{
    [SerializeField] private AbilityType abilityType = AbilityType.Internal;
    [SerializeField] private string abilityName = "Bouncy";
    [SerializeField] private string abilityDescription = "This ability allows the player to bounce off the ground.";
    [SerializeField] private float abilityCooldown = 3f;
    [SerializeField] private float abilityDuration = 5f;
    
    [SerializeField] private float bounceForce = 15f;
    [SerializeField] private PhysicsMaterial2D bouncyMaterial;

    private float originalGravityScale = 1f;

    public override void ExecuteAbility(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        BoxCollider2D bc = player.GetComponent<BoxCollider2D>();

        player.GetComponent<PlayerMovementController>().ToggleVelocityReset(true);

        rb.sharedMaterial = bouncyMaterial;
        bc.sharedMaterial = bouncyMaterial;

        originalGravityScale = rb.gravityScale;

        rb.gravityScale = 0.2f;
        rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        // apply random rotational force
        rb.AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);
        
    }

    public override void StopAbility(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        BoxCollider2D bc = player.GetComponent<BoxCollider2D>();

        player.GetComponent<PlayerMovementController>().ToggleVelocityReset(false);

        rb.sharedMaterial = null;
        bc.sharedMaterial = null;

        rb.gravityScale = originalGravityScale;

        // bring player back to 0 rotation
        rb.angularVelocity = 0f;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override float GetAbilityCooldown()
    {
        return abilityCooldown;
    }

    public override string GetAbilityDescription()
    {
        return abilityDescription;
    }

    public override float GetAbilityDuration()
    {
        return abilityDuration;
    }

    public override string GetAbilityName()
    {
        return abilityName;
    }

    public override AbilityType GetAbilityType()
    {
        return abilityType;
    }
}
