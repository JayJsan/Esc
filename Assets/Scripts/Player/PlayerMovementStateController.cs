using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerAreaState
{
    Land,
    Water
}

public enum PlayerLandMovementState
{
    Idle,
    Walking,
    Jumping,
    Falling,
}

public enum PlayerWaterMovementState
{
    Idle,
    Swimming,
    Diving,
}

[RequireComponent(typeof(PlayerLandMovementController))]
[RequireComponent(typeof(PlayerWaterMovementController))]
public class PlayerMovementStateController : MonoBehaviour
{
    public static PlayerMovementStateController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerLandMovementController playerLandMovementController;
    [SerializeField] private PlayerWaterMovementController playerWaterMovementController;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D playerCollider;

    // ============ Private Variables ============
    private PlayerAreaState playerAreaState = PlayerAreaState.Land;
    private PlayerLandMovementState playerLandMovementState = PlayerLandMovementState.Idle;
    private PlayerWaterMovementState playerWaterMovementState = PlayerWaterMovementState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        playerLandMovementController = GetComponent<PlayerLandMovementController>();
        playerWaterMovementController = GetComponent<PlayerWaterMovementController>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (playerAreaState)
        {
            case PlayerAreaState.Land:
                HandleLandMovementState();
                break;
            case PlayerAreaState.Water:
                HandleWaterMovementState();
                break;
        }
    }

    private void HandleLandMovementState()
    {
        playerLandMovementController.enabled = true;
        playerWaterMovementController.enabled = false;
    }

    private void HandleWaterMovementState()
    {
        playerLandMovementController.enabled = false;
        playerWaterMovementController.enabled = true;
    }
}
