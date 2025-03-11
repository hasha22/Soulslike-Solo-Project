using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerAnimationManager playerAnimationManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerStatManager playerStatManager;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerStatManager = GetComponent<PlayerStatManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (!IsOwner)
        {
            return;
        }
        playerLocomotionManager.HandleAllMovement();

        playerStatManager.RegenerateStamina();

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
            return;

        int maxStamina;

        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;

        playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenTimer;

        maxStamina = playerStatManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
        playerNetworkManager.maxStamina.Value = maxStamina;
        playerNetworkManager.currentStamina.Value = maxStamina;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

    }

    /*
    PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue(
        playerNetworkManager.currentStamina.Value,
        playerNetworkManager.currentStamina.Value);
    */

    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }
}
