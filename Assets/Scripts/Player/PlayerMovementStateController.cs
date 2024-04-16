using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerLandMovementController))]
[RequireComponent(typeof(PlayerWaterMovementController))]
[RequireComponent(typeof(EnvironmentEntityState))]
public class PlayerMovementStateController : MonoBehaviour
{
    public static PlayerMovementStateController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerLandMovementController playerLandMovementController;
    [SerializeField] private PlayerWaterMovementController playerWaterMovementController;
    [SerializeField] private EnvironmentEntityState environmentEntityState;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D playerCollider;

    // ============ Private Variables ============
    private PlayerPhysicalState currentPhysicalState = PlayerPhysicalState.Normal;

    // Start is called before the first frame update
    void Start()
    {
        playerLandMovementController = GetComponent<PlayerLandMovementController>();
        playerWaterMovementController = GetComponent<PlayerWaterMovementController>();
        environmentEntityState = GetComponent<EnvironmentEntityState>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void UpdateState(Component sender, object data)
    {
        Debug.Log("UpdateState");
        if (data is int)
        {
            int entityID = (int)data;
            if (entityID == environmentEntityState.entityID)
            {
                switch (environmentEntityState.GetCurrentState())
                {
                    case EntityEnvironmentType.Land:
                        HandleLandMovementState();
                        break;
                    case EntityEnvironmentType.Water:
                        HandleWaterMovementState();
                        break;
                }
            }
        }
    }

    public void UpdatePlayerPhysicalState(Component sender, object data)
    {
        if (data is bool)
        {
            bool isFish = (bool)data;
            if (isFish)
            {
                currentPhysicalState = PlayerPhysicalState.Fish;
            }
            else
            {
                currentPhysicalState = PlayerPhysicalState.Normal;
            }

            switch (environmentEntityState.GetCurrentState())
                {
                    case EntityEnvironmentType.Land:
                        HandleLandMovementState();
                        break;
                    case EntityEnvironmentType.Water:
                        HandleWaterMovementState();
                        break;
                }
        }
    }


    private void HandleLandMovementState()
    {
        if (currentPhysicalState == PlayerPhysicalState.Normal)
        {
            playerLandMovementController.enabled = true;
            playerWaterMovementController.enabled = false;
        }
        else
        {
            playerLandMovementController.enabled = false;
            playerWaterMovementController.enabled = true;
        }
    }

    private void HandleWaterMovementState()
    {
        if (currentPhysicalState == PlayerPhysicalState.Normal)
        {
            playerLandMovementController.enabled = false;
            playerWaterMovementController.enabled = true;
        }
        else
        {
            playerLandMovementController.enabled = false;
            playerWaterMovementController.enabled = true;
        }
    }

    public EntityEnvironmentType GetCurrentEnvironmentState()
    {
        return environmentEntityState.GetCurrentState();
    }
}
