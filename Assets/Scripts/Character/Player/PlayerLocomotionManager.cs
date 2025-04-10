using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float rotationSpeed = 15;

    [Header("Jumping")]
    private Vector3 jumpDirection;
    [SerializeField] float jumpStaminaCost = 20;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float jumpForwardSpeed = 10f;
    [SerializeField] float freeFallSpeed = 5f;

    [Header("Walking & Running")]
    [SerializeField] float walkingSpeed = 1.2f;
    [SerializeField] float runningSpeed = 2.5f;
    [SerializeField] float sprintingSpeed = 12f;
    [SerializeField] int sprintingStaminaCost = 5;

    [Header("Dodge")]
    private Vector3 rollDirection;
    [SerializeField] float dodgeStaminaCost = 25;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
    }
    protected override void Update()
    {
        base.Update();
        if (player.IsOwner)
        {
            player.characterNetworkManager.verticalMovement.Value = verticalMovement;
            player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
            player.characterNetworkManager.moveAmount.Value = moveAmount;
        }
        else
        {
            horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
            verticalMovement = player.characterNetworkManager.verticalMovement.Value;
            moveAmount = player.characterNetworkManager.moveAmount.Value;

            player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
        HandleJumping();
        HandleFreeFallMovement();
    }
    private void GetMovement()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }
    private void HandleGroundedMovement()
    {
        GetMovement();
        if (!player.canMove)
            return;

        //Movement based on camera perspective and inputs
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            float speed = PlayerInputManager.instance.isWalking ? walkingSpeed : runningSpeed;
            player.characterController.Move(moveDirection * speed * Time.deltaTime);
        }

    }
    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.currentStamina.Value <= 0)
        {
            player.playerNetworkManager.isSprinting.Value = false;
            return;
        }

        if (moveAmount >= 0.5)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }

        if (player.playerNetworkManager.isSprinting.Value)
        {
            player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
        }
    }
    private void HandleRotation()
    {
        if (!player.canRotate)
            return;
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;


        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }
    private void HandleJumping()
    {
        if (player.playerNetworkManager.isJumping.Value)
        {
            //the speed is not being adjusted correctly
            player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }
    private void HandleFreeFallMovement()
    {
        if (!player.isGrounded)
        {
            Vector3 inputDirection = Vector3.zero;
            float movementSpeed = 0f;
            //only allows full freefall for forward movement
            if (PlayerInputManager.instance.verticalInput > 0)
            {
                inputDirection += PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                if (!PlayerInputManager.instance.isWalking)
                {
                    movementSpeed = freeFallSpeed;
                }
                else
                {
                    movementSpeed = freeFallSpeed * 0.2f;
                }
            }
            //reduced speed when turning around in midair 
            else if (PlayerInputManager.instance.horizontalInput != 0 || PlayerInputManager.instance.verticalInput < 0)
            {
                if (PlayerInputManager.instance.horizontalInput != 0)
                    inputDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;

                if (PlayerInputManager.instance.verticalInput < 0)
                    inputDirection += PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;

                movementSpeed = freeFallSpeed * 0.4f;
            }
            //applies rotation and movement in midair
            if (inputDirection != Vector3.zero)
            {
                inputDirection.y = 0;
                inputDirection.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 5f * Time.deltaTime);

                player.characterController.Move(inputDirection * movementSpeed * Time.deltaTime);
            }
        }
    }
    public void AttemptToPerformDodge()
    {
        //Need to fix all of this
        if (player.isPerformingAction)
            return;

        if (player.playerNetworkManager.currentStamina.Value <= 0)
            return;

        if (moveAmount > 0)
        {
            //Moving roll
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;

            rollDirection.y = 0;
            rollDirection.Normalize();
            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            player.playerAnimationManager.PlayActionAnimation("Roll_Forward_01", true);
        }
        else
        {
            player.playerAnimationManager.PlayActionAnimation("Backstep_01", true);
        }
        player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
    }
    public void AttemptToPerformJump()
    {
        //will be likely changed when i add combat
        if (player.isPerformingAction)
            return;
        if (player.playerNetworkManager.currentStamina.Value <= 0)
            return;
        if (player.playerNetworkManager.isJumping.Value)
            return;
        if (!player.isGrounded)
            return;

        //Moving jump
        jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
        jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
        jumpDirection.y = 0;

        player.playerAnimationManager.PlayActionAnimation("Jump_start", true);
        player.playerNetworkManager.isJumping.Value = true;

        if (jumpDirection != Vector3.zero)
        {
            if (player.playerNetworkManager.isSprinting.Value)
            {
                jumpDirection *= 1;
            }
            else if (PlayerInputManager.instance.moveAmount > 0.5f)
            {
                jumpDirection *= 0.5f;
            }
            else if (PlayerInputManager.instance.moveAmount < 0.5f)
            {
                jumpDirection *= 0.25f;
            }
        }
        player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
    }
    public void ApplyJumpVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }
}
