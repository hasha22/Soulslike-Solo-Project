using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    [HideInInspector] public float verticalMovement;
    [HideInInspector] public float horizontalMovement;
    [HideInInspector] public float moveAmount;

    [Header("Movement Settings")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    [SerializeField] float walkingSpeed = 1.2f;
    [SerializeField] float runningSpeed = 2.5f;
    [SerializeField] float sprintingSpeed = 4f;
    [SerializeField] float rotationSpeed = 15;

    [Header("Dodge")]
    private Vector3 rollDirection;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
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

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
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
    }

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }


        if (moveAmount >= 0.5)
        {
            player.playerNetworkManager.isSprinting.Value = true;
        }
        else
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }


}
