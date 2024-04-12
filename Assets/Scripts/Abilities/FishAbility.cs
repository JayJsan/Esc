using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAbility : MonoBehaviour, IAbility
{
    [Header("Configuration")]
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float duration = 5f;
    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public float GetAbilityDuration()
    {
        throw new System.NotImplementedException();
    }

    public float GetAbilityCooldown()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
