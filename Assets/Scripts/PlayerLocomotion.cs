using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

 


    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float rayCastHeightOffSet = 0.5f;

    public bool isGrounded;
    public bool isJumping;


    public float movementSpeed = 7f;
    public float rotationSpeed = 15f;


    [Header("Jumping")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15f;
    public float jumpForwardSpeed = 5f;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }


    public void HandleAllMovement()
    {
        if (transform.position.y <= -10f)
        {
            playerRigidbody.velocity = Vector3.zero; // Stop any existing velocity
            FindObjectOfType<GameManager>().EndGame();
            return; // Exit the method, preventing further movement
        }


        HandleFallingAndLanding();
        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {

        if (isJumping) return;
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection *= movementSpeed;

        Vector3 movementvelocity = moveDirection;
        playerRigidbody.velocity = movementvelocity;
    }

    private void HandleRotation()
    {
        if(isJumping) return;   
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffSet;

        if (!isGrounded && !isJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Fall", true);
            }
            inAirTimer = inAirTimer + Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);

            }
            inAirTimer = 0;
            isGrounded = true;

        }
        else
        {
            isGrounded= false;
        }
    }

    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            // Calculate the vertical velocity for the jump
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);

            // Add forward movement to the jump based on the current move direction
            Vector3 playerVelocity = moveDirection.normalized * jumpForwardSpeed;
            playerVelocity.y = jumpingVelocity;

            // Apply the calculated velocity to the Rigidbody
            playerRigidbody.velocity = playerVelocity;
        }
    }

}

