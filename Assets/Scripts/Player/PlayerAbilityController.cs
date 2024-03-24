using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is responsible for taking input from the player and executing the abilities.
/// </summary>
[RequireComponent(typeof(PlayerAbilityHolder))]
public class PlayerAbilityController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAbilityHolder playerAbilityHolder;

    // Start is called before the first frame update
    void Start()
    {
        playerAbilityHolder = GetComponent<PlayerAbilityHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        // this is temprorary, need to change this to be more dynamic
        if (Input.GetKeyDown(KeyCode.B))
        {
            playerAbilityHolder.ExecuteAbility(KeyCode.B);
        }
    }
}
