using SaveGameManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    InputControls inputControls;
    public static PlayerInputManager instance { get; private set; }
    public PlayerManager player;

    [Header("Player Movement")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount = 0;

    [Header("Player Camera")]
    [SerializeField] Vector2 cameraInput;
    public float verticalCameraInput;
    public float horizontalCameraInput;

    [Header("Player Actions")]
    public bool isDodging = false;
    public bool isSprinting = false;
    public bool isWalking = false;
    public bool isJumping = false;
    public bool leftClickInput = false;
    public bool switchRightWeapon = false;
    public bool switchLeftWeapon = false;

    [Header("Lock On Input")]
    public bool isLockedOn = false;
    private Coroutine lockOnCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChange;

        if (inputControls != null)
        {
            inputControls.Disable();
        }
    }
    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        //Enables controls if loading into World Scene, disables otherwise
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
            if (inputControls != null)
            {
                inputControls.Enable();
            }
        }
        else
        {
            instance.enabled = false;

            if (inputControls != null)
            {
                inputControls.Disable();
            }
        }
    }
    private void OnEnable()
    {
        if (inputControls == null)
        {
            inputControls = new InputControls();

            inputControls.PlayerControls.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            inputControls.PlayerCamera.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputControls.PlayerActions.Dodge.performed += i => isDodging = !isDodging;
            inputControls.PlayerActions.Walk.performed += i => isWalking = !isWalking;

            inputControls.PlayerActions.Sprint.performed += i => isSprinting = true;
            inputControls.PlayerActions.Sprint.canceled += i => isSprinting = false;

            inputControls.PlayerActions.Jump.performed += i => isJumping = true;

            inputControls.PlayerActions.LeftClick.performed += i => leftClickInput = true;
            inputControls.PlayerActions.LockOn.performed += i => isLockedOn = true;

            inputControls.PlayerActions.SwitchRightWeapon.performed += i => switchRightWeapon = true;
            inputControls.PlayerActions.SwitchLeftWeapon.performed += i => switchLeftWeapon = true;

            inputControls.Enable();
        }
    }
    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            inputControls.Enable();
        }
        else
        {
            inputControls.Disable();
        }
    }
    private void Update()
    {
        HandleAllInputs();
    }
    private void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
        HandleDodgeInput();
        HandleSprintingInput();
        HandleJumpingInput();
        HandleLeftClickInput();
        HandleLockOnInput();
        HandleSwitchRightWeaponInput();
        HandleSwitchLeftWeaponInput();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        //Clamping values
        if (moveAmount != 0)
        {
            if (isWalking)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
        }
        if (player == null)
            return;

        //not locked on, therefore no strafing 
        if (!player.playerNetworkManager.isLockedOn.Value || player.playerNetworkManager.isSprinting.Value)
        {
            player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
        else
        {
            player.playerAnimationManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput);
        }
    }
    private void HandleCameraInput()
    {
        verticalCameraInput = cameraInput.y;
        horizontalCameraInput = cameraInput.x;
    }
    private void HandleDodgeInput()
    {
        if (isDodging)
        {
            isDodging = false;
            player.playerLocomotionManager.AttemptToPerformDodge();
        }
    }
    private void HandleSprintingInput()
    {
        if (isSprinting)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else if (player != null)
        {
            player.playerNetworkManager.isSprinting.Value = false;
        }
    }
    private void HandleJumpingInput()
    {
        if (isJumping)
        {
            isJumping = false;
            player.playerLocomotionManager.AttemptToPerformJump();
        }
    }
    private void HandleLeftClickInput()
    {
        if (leftClickInput && player != null)
        {
            leftClickInput = false;

            player.playerNetworkManager.SetCharacterActionHand(true);
            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.LeftClickAction, player.playerInventoryManager.currentRightHandWeapon);
        }
    }
    private void HandleLockOnInput()
    {
        if (player == null)
            return;
        // Checks for dead target
        if (player.playerNetworkManager.isLockedOn.Value)
        {
            if (player.playerCombatManager.currentTarget == null)
                return;

            if (!player.playerCombatManager.currentTarget.isAlive.Value)
            {
                player.playerNetworkManager.isLockedOn.Value = false;
            }
            // Attempts to find new target
            if (lockOnCoroutine != null)
                StopCoroutine(lockOnCoroutine);

            lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
        }
        // Disables lock on
        if (isLockedOn && player.playerNetworkManager.isLockedOn.Value)
        {
            isLockedOn = false;
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerNetworkManager.isLockedOn.Value = false;
            return;
        }
        // Enables lock on
        if (isLockedOn && !player.playerNetworkManager.isLockedOn.Value)
        {
            isLockedOn = false;
            PlayerCamera.instance.HandleCameraLockOnTargets();
            if (PlayerCamera.instance.nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerNetworkManager.isLockedOn.Value = true;
            }
        }
    }
    private void HandleSwitchRightWeaponInput()
    {
        if (switchRightWeapon)
        {
            switchRightWeapon = false;
            player.playerEquipmentManager.SwitchRightWeapon();
        }
    }
    private void HandleSwitchLeftWeaponInput()
    {
        if (switchLeftWeapon)
        {
            switchLeftWeapon = false;
            player.playerEquipmentManager.SwitchLeftWeapon();
        }
    }
}
