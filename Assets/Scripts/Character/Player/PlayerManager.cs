using SaveGameManager;
using System.Collections;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Debug Menu")]
    [SerializeField] bool respawnCharacter = false;

    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerNetworkManager playerNetworkManager;
    [HideInInspector] public PlayerAnimationManager playerAnimationManager;
    [HideInInspector] public PlayerStatManager playerStatManager;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;

    private Vector3 lastPosition;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerNetworkManager = GetComponent<PlayerNetworkManager>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        playerStatManager = GetComponent<PlayerStatManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
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

        DebugMenu();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
            return;

        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;
        WorldSaveGameManager.instance.player = this;

        //updates max values of vigor/endurance/mind
        playerNetworkManager.vigor.OnValueChanged += playerNetworkManager.SetNewVigorValue;
        playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewEnduranceValue;
        playerNetworkManager.mind.OnValueChanged += playerNetworkManager.SetNewMindValue;

        //updates UI based on health/stamina/fp changes
        playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        playerNetworkManager.currentStamina.OnValueChanged += playerStatManager.ResetStaminaRegenTimer;
        playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;
        playerNetworkManager.currentFocusPoints.OnValueChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewFPValue;

        playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;

        //if connecting as client
        if (IsOwner && !IsServer)
        {
            Debug.Log("Loading client data...");
            LoadPlayerData(ref PlayerUIManager.instance.clientCharacterData);
        }

    }
    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation)
    {
        if (IsOwner)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
        }

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
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
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.vigor = playerNetworkManager.vigor.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        currentCharacterData.mind = playerNetworkManager.mind.Value;

        currentCharacterData.maxStamina = playerNetworkManager.maxStamina.Value;
        currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value;

        currentCharacterData.maxHealth = playerNetworkManager.maxHealth.Value;
        currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;

        currentCharacterData.maxFocusPoints = playerNetworkManager.maxFocusPoints.Value;
        currentCharacterData.currentFocusPoints = playerNetworkManager.currentFocusPoints.Value;
    }
    public void CreatePlayerData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        CalculateResourcesForNewCharacter(ref currentCharacterData);

        currentCharacterData.vigor = playerNetworkManager.vigor.Value;
        currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        currentCharacterData.mind = playerNetworkManager.mind.Value;
    }
    public void CalculateResourcesForNewCharacter(ref CharacterSaveData currentCharacterData)
    {
        int maxStamina, maxHealth, maxFocusPoints;
        // stamina
        maxStamina = playerStatManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
        playerNetworkManager.maxStamina.Value = maxStamina;
        playerNetworkManager.currentStamina.Value = maxStamina;
        currentCharacterData.maxStamina = maxStamina;
        currentCharacterData.currentStamina = maxStamina;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value, playerNetworkManager.maxStamina.Value);

        //health
        maxHealth = playerStatManager.CalculateHealthBasedOnVigorLevel(playerNetworkManager.vigor.Value);
        playerNetworkManager.maxHealth.Value = maxHealth;
        playerNetworkManager.currentHealth.Value = maxHealth;
        currentCharacterData.maxHealth = maxHealth;
        currentCharacterData.currentHealth = maxHealth;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(playerNetworkManager.maxHealth.Value, playerNetworkManager.maxHealth.Value);

        //FP
        maxFocusPoints = playerStatManager.CalculateFocusPointsBasedOnMindLevel(playerNetworkManager.mind.Value);
        playerNetworkManager.maxFocusPoints.Value = maxFocusPoints;
        playerNetworkManager.currentFocusPoints.Value = maxFocusPoints;
        currentCharacterData.maxFocusPoints = maxFocusPoints;
        currentCharacterData.currentFocusPoints = maxFocusPoints;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxFPValue(playerNetworkManager.maxFocusPoints.Value, playerNetworkManager.maxFocusPoints.Value);
    }
    public void LoadPlayerData(ref CharacterSaveData currentCharacterData)
    {
        playerNetworkManager.characterName.Value = currentCharacterData.characterName;
        Vector3 characterPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);

        transform.position = characterPosition;
        characterNetworkManager.networkPosition.Value = characterPosition;

        playerNetworkManager.maxStamina.Value = currentCharacterData.maxStamina;
        playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(currentCharacterData.maxStamina, currentCharacterData.currentStamina);

        playerNetworkManager.maxHealth.Value = currentCharacterData.maxHealth;
        playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(currentCharacterData.maxHealth, currentCharacterData.currentHealth);

        playerNetworkManager.maxFocusPoints.Value = currentCharacterData.maxFocusPoints;
        playerNetworkManager.currentFocusPoints.Value = currentCharacterData.currentFocusPoints;
        PlayerUIManager.instance.playerUIHUDManager.SetMaxFPValue(currentCharacterData.maxFocusPoints, currentCharacterData.currentFocusPoints);

        playerNetworkManager.vigor.Value = currentCharacterData.vigor;
        playerNetworkManager.endurance.Value = currentCharacterData.endurance;
        playerNetworkManager.mind.Value = currentCharacterData.mind;
    }
    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        if (IsOwner)
        {
            playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
            playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
            playerNetworkManager.currentFocusPoints.Value = playerNetworkManager.maxFocusPoints.Value;

            playerAnimationManager.PlayActionAnimation("New State", false);
        }
    }
    private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }
    }
}
