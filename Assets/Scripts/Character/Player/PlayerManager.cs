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

        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;

        //updates max values of vigor/endurance/mind
        playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewVigorValue;
        playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewEnduranceValue;
        playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMindValue;

        //updates UI based on health/stamina/fp changes
        playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenTimer;
        playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;
        playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewFPValue;

        // following will be moved to LoadPlayerData() in the future
        int maxStamina, maxHealth, maxFocusPoints;
        // stamina
        maxStamina = playerStatManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
        playerNetworkManager.maxStamina.Value = maxStamina;
        playerNetworkManager.currentStamina.Value = maxStamina;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);

        //health
        maxHealth = playerStatManager.CalculateHealthBasedOnVigorLevel(playerNetworkManager.vigor.Value);
        playerNetworkManager.maxHealth.Value = maxHealth;
        playerNetworkManager.currentHealth.Value = maxHealth;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value);

        //FP
        maxFocusPoints = playerStatManager.CalculateFocusPointsBasedOnMindLevel(playerNetworkManager.mind.Value);
        playerNetworkManager.maxFocusPoints.Value = maxFocusPoints;
        playerNetworkManager.currentFocusPoints.Value = maxFocusPoints;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxFPValue(playerNetworkManager.maxFocusPoints.Value);

    }
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        //unsubscribing from the events
        playerNetworkManager.vigor.OnValueChanged -= playerNetworkManager.SetNewVigorValue;
        playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewEnduranceValue;
        playerNetworkManager.mind.OnValueChanged -= playerNetworkManager.SetNewMindValue;

        playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        playerNetworkManager.currentStamina.OnValueChanged -= playerStatManager.ResetStaminaRegenTimer;
        playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;
        playerNetworkManager.currentFocusPoints.OnValueChanged -= PlayerUIManager.instance.playerUIHUDManager.SetNewFPValue;
    }
    protected override void LateUpdate()
    {
        if (!IsOwner)
            return;
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public void SavePlayerData(ref CharacterSaveData currentCharacterData)
    {
        if (playerNetworkManager == null)
        {
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
        }
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;
        currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;

        currentCharacterData.vigor = playerNetworkManager.vigor.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        currentCharacterData.mind = playerNetworkManager.mind.Value;

        Debug.Log("it saved");
    }
    public void LoadPlayerData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 characterPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = characterPosition;

        playerNetworkManager.vigor.Value = currentCharacterData.vigor;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;
        playerNetworkManager.mind.Value = currentCharacterData.mind;
    }
}
