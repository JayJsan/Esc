using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Configuration")]
    public float moveSpeed = 5f;
    public float groundRayCheckDistance = 0.2f;
    [Header("Jump Configuration")]
    [Range(1, 20)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float playerDownFallMultiplier = 4f; // this is the multiplier for when the player presses down while in the air

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D characterEntityCollider;

    #region Private Variables
    #region Input Variables
    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private bool hasPressedDown = false; // player has pressed down while in the air
    private bool disableMovementInput = false;
    private bool disableJumpInput = false;
    #endregion

    #region Physics Variables
    private float jumpVelocityCalculation = 0;
    private float fallVelocityCalculation = 0;
    private float playerDownFallVelocityCalculation = 0;
    #endregion

    #region Check State Variables
    private bool isGrounded;
    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody2D>() == null) {
            gameObject.AddComponent<Rigidbody2D>();
        } 
        else {
            rb = GetComponent<Rigidbody2D>();
        }

        if (GetComponent<Collider2D>() == null) {
        } 
        else 
        {
        characterEntityCollider = GetComponent<Collider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        jumpVelocityCalculation = Physics2D.gravity.y * (fallMultiplier - 1);
        fallVelocityCalculation = Physics2D.gravity.y * (lowJumpMultiplier - 1);
        playerDownFallVelocityCalculation = Physics2D.gravity.y * (playerDownFallMultiplier - 1);
    }

    void FixedUpdate()
    {
        Move();
        Jump();

        JumpVelocityCalculation();
    }

    private void Move()
    {
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        FlipPlayer();
    }

    private void Jump()
    {
        if (jumpInput && isGrounded)
        {
            rb.velocity += Vector2.up * jumpVelocity;
        }
    }

    private void JumpVelocityCalculation()
    {
        CheckIfGrounded();

        // if the player presses down before they have reached the peak of their jump, they will fall faster
        if (verticalInput < -0.3 && !isGrounded)
        {
            hasPressedDown = true;
        }
        
        if (hasPressedDown && !isGrounded) 
        {
            rb.velocity += playerDownFallVelocityCalculation * Time.deltaTime * Vector2.up;
        } 
        else if (rb.velocity.y > 0 && !jumpInput)
        {
            rb.velocity += fallVelocityCalculation * Time.deltaTime * Vector2.up;
        }
        else if (rb.velocity.y < 0) 
        {
            rb.velocity += jumpVelocityCalculation * Time.deltaTime * Vector2.up;
        } 
    }

    private bool CheckIfGrounded()
    {
        Ray ray = new Ray(transform.position, Vector2.down);

        // debug draw line of ground ray
        Debug.DrawRay(ray.origin, ray.direction * (characterEntityCollider.bounds.extents.y + groundRayCheckDistance), Color.red);

        if (Physics2D.Raycast(rb.position, Vector2.down, characterEntityCollider.bounds.extents.y + groundRayCheckDistance, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
            hasPressedDown = false;
            return true;
        }
        else
        {
            isGrounded = false;
            return false;
        }
    }

    public bool IsPlayerGrounded()
    {
        return isGrounded;
    }

    private void FlipPlayer()
    {
        // rotate players -180 if pressing left
        if (horizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else if (horizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void ToggleMovementAbility(bool isActive)
    {
        disableMovementInput = isActive;
    }
    
    public void ToggleJumpAbility(bool isActive)
    {
        disableJumpInput = isActive;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetButton("Jump");
    }
}
