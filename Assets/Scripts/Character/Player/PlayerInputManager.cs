using SaveGameManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    InputControls inputControls;
    public static PlayerInputManager instance;
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
        {
            return;
        }
        //not locked on, therefore no strafing 
        player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount);
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
}
