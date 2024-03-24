using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType { Internal, External } 

public abstract class Ability : ScriptableObject 
{
    public abstract string GetAbilityName();
    public abstract string GetAbilityDescription();
    public abstract float GetAbilityCooldown();
    public abstract float GetAbilityDuration();
    public abstract AbilityType GetAbilityType();
    public abstract void ExecuteAbility(GameObject player);
    public abstract void StopAbility(GameObject player);
}
