using SaveGameManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    InputControls inputControls;

    [Header("Player Movement")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount = 0;

    [Header("Player Camera")]
    [SerializeField] Vector2 cameraInput;
    public float verticalCameraInput;
    public float horizontalCameraInput;

    public static PlayerInputManager instance;
    public PlayerManager player;

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
    }
    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        //Enables controls if loading into World Scene, disables otherwise
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (inputControls == null)
        {
            inputControls = new InputControls();

            inputControls.PlayerControls.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            inputControls.PlayerCamera.CameraControls.performed += i => cameraInput = i.ReadValue<Vector2>();

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
        HandleMovementInput();
        HandleCameraInput();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        //Clamping values 
        if (moveAmount > 0 && moveAmount <= 0.5)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
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
}
