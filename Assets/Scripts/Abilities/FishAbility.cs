using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAbility : MonoBehaviour, IAbility
{
    [Header("Configuration")]
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float duration = 10f;

    [Header("Fish Configuration")]
    [SerializeField] private Sprite normalSprite;

    [Header("References")]
    [SerializeField] private GameEvent OnFishTransformation;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private Animator animator;

    // ============ Private Variables ============
    private bool isFish = false;

    void Start()
    {
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Execute()
    {
        isFish = true;
        OnFishTransformation.Raise(this, isFish);
        animator.SetBool("IsFish", true);
        StartCoroutine(AbilityTimer());
    }

    public float GetAbilityDuration()
    {
        return duration;
    }

    public float GetAbilityCooldown()
    {
        return cooldown;
    }

    public void Stop()
    {
        isFish = false;
        animator.SetBool("IsFish", false);
        OnFishTransformation.Raise(this, isFish);
    }

    private IEnumerator AbilityTimer()
    {
        yield return new WaitForSeconds(duration);
        Stop();
    }
}
