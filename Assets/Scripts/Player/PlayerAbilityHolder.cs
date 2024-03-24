using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is responsible for holding the abilities that the player has.
/// It also maps the abilities to the keys that the player can press to execute them.
/// </summary>
public class PlayerAbilityHolder : MonoBehaviour
{
    public enum AbilityState { Ready, Executing, Cooldown }

    [Header("Configuration")]
    [SerializeField] private List<Ability> abilities = new List<Ability>();
    [SerializeField] private Dictionary<KeyCode, Ability> abilityKeyMap = new Dictionary<KeyCode, Ability>();

    // Private Variables
    private float currentAbilityCooldown = 0f;
    private float currentAbilityDuration = 0f;
    public AbilityState currentAbilityState { get; private set; } = AbilityState.Ready;

    private void Start()
    {
        SetupAbilityKeyMap();
        // check if count of abilities line up with ability key map
        if (abilities.Count != abilityKeyMap.Count)
        {
            Debug.LogError("Abilities count does not match ability key map count. There should be a key mapped to each ability, or there is an ability count mismatch.");
        }
    }

    private void SetupAbilityKeyMap()
    {
        abilityKeyMap.Add(KeyCode.B, abilities.Find(ability => ability.GetAbilityName() == "Bouncy"));
    }

    public void ExecuteAbility(KeyCode key)
    {
        if (abilityKeyMap.ContainsKey(key) && currentAbilityState == AbilityState.Ready)
        {
            // set current ability stats
            Ability ability = abilityKeyMap[key];
            currentAbilityCooldown = ability.GetAbilityCooldown();
            currentAbilityDuration = ability.GetAbilityDuration();
            
            abilityKeyMap[key].ExecuteAbility(this.gameObject);
            StartCoroutine(AbilityTimer(key));
        }
    }

    private IEnumerator AbilityTimer(KeyCode key)
    {
        currentAbilityState = AbilityState.Executing;
        yield return new WaitForSeconds(currentAbilityDuration);
        abilityKeyMap[key].StopAbility(this.gameObject);
        currentAbilityState = AbilityState.Cooldown;
        yield return new WaitForSeconds(currentAbilityCooldown);
        currentAbilityState = AbilityState.Ready;
    }
}
