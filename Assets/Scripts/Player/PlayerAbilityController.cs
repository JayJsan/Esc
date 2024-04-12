using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum Abilities
{
    None,
    Earth,
    Water,
    Fire,
    Air
}

public enum AbilityState
{
    Ready,
    Executing,
    Cooldown,
}

public enum PlayerAbilityUseState
{
    Ready,
    Executing,
    Cooldown,
}

/// <summary>
/// This class is responsible for taking input from the player and executing the abilities.
/// </summary>
public class PlayerAbilityController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private List<Abilities> abilitiesUnlocked = new List<Abilities>();
    [SerializeField] private KeyCode earthDashKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode fishAbilityKey = KeyCode.Alpha2;
    [SerializeField] private KeyCode flameSuitAbilityKey = KeyCode.Alpha3;
    [SerializeField] private KeyCode airBurstAbilityKey = KeyCode.Alpha4;
    [Header("References")]
    [SerializeField] private EarthDashAbility earthDashAbility;
    [SerializeField] private FishAbility fishAbility;
    [SerializeField] private FlameSuitAbility flameSuitAbility;
    [SerializeField] private AirBurstAbility airBurstAbility;
    // ======= Private Variables =======
    private PlayerAbilityUseState playerAbilityUseState = PlayerAbilityUseState.Ready;
    private Dictionary<Abilities, AbilityState> abilityStates = new Dictionary<Abilities, AbilityState>();

    // Start is called before the first frame update
    void Start()
    {
        // Initialise the ability states
        abilityStates.Add(Abilities.Earth, AbilityState.Ready);
        abilityStates.Add(Abilities.Water, AbilityState.Ready);
        abilityStates.Add(Abilities.Fire, AbilityState.Ready);
        abilityStates.Add(Abilities.Air, AbilityState.Ready);
    }

    // Update is called once per frame
    void Update()
    {
        // if player is executing an ability, return
        if (playerAbilityUseState == PlayerAbilityUseState.Executing)
        {
            return;
        }   


        if (abilityStates[Abilities.Earth] == AbilityState.Ready && abilitiesUnlocked.Contains(Abilities.Earth) && Input.GetKeyDown(earthDashKey))
        {
            earthDashAbility.Execute();
            StartCoroutine(AbilityTimer(earthDashAbility.GetAbilityDuration(), earthDashAbility.GetAbilityCooldown(), Abilities.Earth));
            StartCoroutine(PlayerUseAbilityTimer(earthDashAbility.GetAbilityDuration()));
        }
        else if (abilityStates[Abilities.Water] == AbilityState.Ready && abilitiesUnlocked.Contains(Abilities.Water) && Input.GetKeyDown(fishAbilityKey))
        {
            fishAbility.Execute();
            StartCoroutine(AbilityTimer(fishAbility.GetAbilityDuration(), fishAbility.GetAbilityCooldown(), Abilities.Water));
            StartCoroutine(PlayerUseAbilityTimer(fishAbility.GetAbilityDuration()));
        }
        else if (abilityStates[Abilities.Fire] == AbilityState.Ready && abilitiesUnlocked.Contains(Abilities.Fire) && Input.GetKeyDown(flameSuitAbilityKey))
        {
            flameSuitAbility.Execute();
            StartCoroutine(AbilityTimer(flameSuitAbility.GetAbilityDuration(), flameSuitAbility.GetAbilityCooldown(), Abilities.Fire));
            StartCoroutine(PlayerUseAbilityTimer(flameSuitAbility.GetAbilityDuration()));
        }
        else if (abilityStates[Abilities.Air] == AbilityState.Ready && abilitiesUnlocked.Contains(Abilities.Air) && Input.GetKeyDown(airBurstAbilityKey))
        {
            airBurstAbility.Execute();
            StartCoroutine(AbilityTimer(airBurstAbility.GetAbilityDuration(), airBurstAbility.GetAbilityCooldown(), Abilities.Air));
            StartCoroutine(PlayerUseAbilityTimer(airBurstAbility.GetAbilityDuration()));
        }
    }

    private IEnumerator AbilityTimer(float duration, float cooldown, Abilities ability)
    {
        abilityStates[ability] = AbilityState.Executing;

        Debug.Log("Ability: " + ability + " is executing");
        
        yield return new WaitForSeconds(duration);

        abilityStates[ability] = AbilityState.Cooldown;

        Debug.Log("Ability: " + ability + " is on cooldown");

        yield return new WaitForSeconds(cooldown);

        Debug.Log("Ability: " + ability + " is ready");

        abilityStates[ability] = AbilityState.Ready;
    }

    /// <summary>
    /// Player should not be able to use any other abilities while an ability isexecuting
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator PlayerUseAbilityTimer(float duration)
    {
        playerAbilityUseState = PlayerAbilityUseState.Executing;

        Debug.Log("Player is executing ability");
        
        yield return new WaitForSeconds(duration);

        playerAbilityUseState = PlayerAbilityUseState.Ready;

        Debug.Log("Player is Ready");
    }
}
