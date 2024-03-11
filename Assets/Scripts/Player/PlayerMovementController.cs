using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    // ======= VARIABLES ========
    #region VARIABLES
    private Vector2 movementInput = Vector2.zero;
    private bool jumpInput = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        Vector2 direction = new Vector2(movementInput.x, movementInput.y);
        rb.velocity = (direction.normalized * movementSpeed);
    }

    private void Jump()
    {
        if (jumpInput)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

}
