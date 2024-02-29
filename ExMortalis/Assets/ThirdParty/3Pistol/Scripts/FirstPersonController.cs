using System;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

    // Floats
    private float acceleration;
    public float walkingAcceleration;
    private float runningAcceleration;
    public float sprintMultiplier;
    public float jumpForce;
    public float maxWalkSpeed;
    public float maxRunSpeed;

    // Rigidbody
    Rigidbody rb;

    // Booleans
    [HideInInspector] public bool IsWalking = false;
    [HideInInspector] public bool IsQuiet = true;
    [HideInInspector] public bool IsJumping = false;
    [HideInInspector] public bool IsRunning = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        runningAcceleration = walkingAcceleration * sprintMultiplier;
        acceleration = walkingAcceleration;
    }



    void FixedUpdate()
    {
        // Check for all possible keyboard inputs
        CheckForJumping();
        CheckForSprinting();
        CheckForMovement();
    }



    void CheckForMovement()
    {
        // Check if the player pressed any movement buttons
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Make a translation vector on previous inputs
        Vector3 translationVector = new Vector3(x, 0, z);

        // Update all animation-related variables
        UpdateMovementAnimationVariables(x, z);


        // Is the current velocity less than or equal to max velocity allowed?
        if (Mathf.Abs(rb.velocity.x) <= maxWalkSpeed && Mathf.Abs(rb.velocity.z) <= maxWalkSpeed && !IsRunning)
        {
            // If yes, then calculate movement forces based on previous calculations
            rb.AddRelativeForce(translationVector * acceleration * Time.deltaTime);
        }

        // Is the current velocity less than or equal to max velocity allowed?
        if (Mathf.Abs(rb.velocity.x) <= maxRunSpeed && Mathf.Abs(rb.velocity.z) <= maxRunSpeed && IsRunning)
        {
            // If yes, then calculate movement forces based on previous calculations
            rb.AddRelativeForce(translationVector * acceleration * Time.deltaTime);
        }
    }


    // This function updates all variables involved with animation
    void UpdateMovementAnimationVariables(float x, float z)
    {
        // Is the player not moving?
        if (x == 0 && z == 0)
        {
            // If yes, then change booleans to tell animation of it
            IsWalking = false;
            IsQuiet = true;
        }
        else
        {
            // If no, then change booleans to tell animation of it
            IsWalking = !Input.GetKey(KeyCode.LeftShift);
            IsRunning = Input.GetKey(KeyCode.LeftShift);
            IsQuiet = false;
        }
    }


    void CheckForJumping()
    {
        // Check if the player is pressing the jump button
        float y = Input.GetAxis("Jump");

        // Check is the player is touching the ground
        IsJumping = isTouchingGround();

        // Calculate jump force based on previous calculations
        rb.AddForce(Vector3.up * y * Convert.ToInt32(IsJumping) * jumpForce * Time.deltaTime);
    }



    void CheckForSprinting()
    {
        // Has L. shift been held down?
        if (IsRunning)
        {
            // If yes, then make the player sprint
            acceleration = runningAcceleration;
        }
        else
        {
            acceleration = walkingAcceleration;
        }
    }



    // The following function checks if the player has collided with the ground
    bool isTouchingGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, GetComponent<CapsuleCollider>().bounds.extents.y);
    }
}
