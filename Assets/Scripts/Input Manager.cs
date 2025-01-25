using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    Playercontrols playercontrols;
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;


    private float moveAmount;

    public float verticalInput;
    public float horizontalInput;

    AnimatorManager animatorManager;

    public bool jumpInput;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();    
        animatorManager = GetComponent<AnimatorManager>();  
    }
    private void OnEnable()
    {
        if (playercontrols == null)
        {
            playercontrols = new Playercontrols();
            playercontrols.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playercontrols.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playercontrols.PlayerAction.Jump.performed += i => jumpInput = true;

        }
        
        playercontrols.Enable();
    }

    private void OnDisable()
    {
        playercontrols.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleJumpingInput();

    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput)+ Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0,moveAmount);
    }

    private void HandleJumpingInput()
    {
        if (jumpInput == true)
        {
            jumpInput = false;
            playerLocomotion.HandleJumping();
        }
    }
}
