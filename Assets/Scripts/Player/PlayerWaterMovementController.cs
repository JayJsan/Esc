using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPhysicalState
{
    Normal,
    Fish,
}
[RequireComponent(typeof(PlayerMovementStateController))]
[RequireComponent(typeof(FishAbility))]
public class PlayerWaterMovementController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]private float fishWaterSpeed = 5f;
    [SerializeField]private float dashForce = 10f;
    
    [Header("References")]
    [SerializeField] private FishAbility fishAbility;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerMovementStateController playerMovementStateController;
    
    // ============ Private Variables ============
    private PlayerPhysicalState currentState = PlayerPhysicalState.Normal;
    private EntityEnvironmentType currentEnvironmentState;
    private Vector2 input;
    private bool dashInput;
    private bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        fishAbility = GetComponent<FishAbility>();    
        rb = GetComponent<Rigidbody2D>();
        playerMovementStateController = GetComponent<PlayerMovementStateController>();
    }

    void OnEnable()
    {
        currentEnvironmentState = playerMovementStateController.GetCurrentEnvironmentState();
    }

    private void Update()
    {
        if (canMove)
            GetInputs();
        else
        {
            input = Vector2.zero;
            dashInput = false;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        HandleState();

        if (currentState == PlayerPhysicalState.Fish)
        {
            HandleFishMovement();
        }
        else 
        {
            HandleNormalMovement();
        }
    }

    private void HandleNormalMovement()
    {
        rb.velocity = new Vector2(input.x * 10f, rb.velocity.y);
        if (dashInput)
        {
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }

    private void HandleFishMovement()
    {
        if (input.magnitude > 0)
        {
            rb.AddForce(input * fishWaterSpeed, ForceMode2D.Force);
        }

        if (dashInput)
        {
            // dash in direction of current movement
            Vector2 direction = input.normalized;
            rb.AddForce(direction * dashForce, ForceMode2D.Impulse);
        }
    }

    private void GetInputs()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        dashInput = Input.GetKeyDown(KeyCode.Space);
    }

    private void HandleNormalState()
    {
        canMove = true;
        // Handle normal movement
        // return to normal speed, gravity, drag
        rb.gravityScale = 1;
        rb.drag = 0;
        rb.mass = 1f;
    }

    private void HandleFishState()
    {
        canMove = true;
        // Handle fish movement underwater
        // increase drag, decrease gravity, increase speed
        rb.gravityScale = 0.25f;
        rb.drag = 0.25f;
        rb.mass = 2f;
    }

    private void HandleFishOnLand()
    {
        canMove = false;
        // Handle fish movement on land
        // increase speed, decrease gravity, increase drag
        rb.gravityScale = 1f;
        rb.drag = 0.25f;
        rb.mass = 4f;
    }

    public void UpdateState(Component sender, object data)
    {
        if (data is bool)
        {
            bool isFish = (bool)data;
            if (isFish)
            {
                currentState = PlayerPhysicalState.Fish;
            }
            else
            {
                currentState = PlayerPhysicalState.Normal;
            }
            HandleState();
        }
    }

    public void UpdateEnvironmentState(Component sender, object data)
    {
        if (sender is EnvironmentEntityState)
        {
            currentEnvironmentState = playerMovementStateController.GetCurrentEnvironmentState();
        }
    }

    private void HandleState()
    {
        switch (currentState)
        {
            case PlayerPhysicalState.Normal:
                HandleNormalState();
                break;
            case PlayerPhysicalState.Fish:
                currentEnvironmentState = playerMovementStateController.GetCurrentEnvironmentState();
                if (currentEnvironmentState == EntityEnvironmentType.Land)
                {
                    HandleFishOnLand();
                }
                else
                {
                    HandleFishState();
                }
                break;
        }
    }
}
