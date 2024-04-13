using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum AttackState
{
    Ready,
    Attacking,
    Cooldown,
}
public class PlayerAttackController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private LayerMask enemyLayer;
    [Header("Configuration")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private int damage = 1;
    // ========== Private Variables ==========
    private float sliceFXDuration = 0.5f;
    private Transform defaultAttackPosition = null;
    private AttackState attackState = AttackState.Ready;
    // Start is called before the first frame update
    void Start()
    {
        if (hitbox == null)
        {
            Debug.LogError("Hitbox not set in PlayerAttackController");
        }
        if (animator == null)
        {
            Debug.LogError("Animator not set in PlayerAttackController");
        }
        
        defaultAttackPosition = hitbox.transform;

        GetClipTimes();
        // animator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && attackState == AttackState.Ready)
        {
            AimAttackUp();
        }
        else if (Input.GetKeyDown(KeyCode.S) && attackState == AttackState.Ready)
        {
            AimAttackDown();
        }
        else if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) || (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            if (attackState != AttackState.Ready) return;
            ResetAim();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackState == AttackState.Ready)
        {
            Attack();
        }
    }

    private void AimAttackUp()
    {
        hitbox.transform.localPosition = new Vector3(0, 1, 0);
        hitbox.transform.localEulerAngles = new Vector3(0, 0, 90);
    }

    private void AimAttackDown()
    {
        hitbox.transform.localPosition = new Vector3(0, -1, 0);
        hitbox.transform.localEulerAngles = new Vector3(0, 0, -90);
    }

    private void ResetAim()
    {
        hitbox.transform.localPosition = new Vector3(1, 0, 0);
        hitbox.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    private void Attack()
    {
        // animator.gameObject.SetActive(true);
        animator.SetTrigger("Slice");

        // check hitbox for any collisions
        Collider2D[] colliders = new Collider2D[20];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(enemyLayer);
        
        // check for trigger colliders
        contactFilter.useTriggers = true;

        hitbox.OverlapCollider(contactFilter, colliders);

        foreach (Collider2D collider in colliders)
        {
            if (collider == null)
            {
                break;
            }

            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
        }

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        attackState = AttackState.Attacking;
        yield return new WaitForSeconds(sliceFXDuration);
        // animator.gameObject.SetActive(false);
        attackState = AttackState.Cooldown;
        yield return new WaitForSeconds(attackCooldown - sliceFXDuration);
        attackState = AttackState.Ready;
    }

    private void GetClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            Debug.Log(clip.name + " " + clip.length);
            if (clip.name == "Slice")
            {
                sliceFXDuration = clip.length;
            }
        }
    }

}
