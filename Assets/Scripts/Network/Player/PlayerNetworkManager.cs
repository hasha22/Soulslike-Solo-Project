using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : CharacterNetworkManager
{
    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Equipment")]
    public NetworkVariable<int> currentWeaponBeingUsed = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> currentRightHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> currentLeftHandWeaponID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> isUsingRightHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isUsingLeftHand = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();

    }
    public void SetCharacterActionHand(bool rightHandedAction)
    {
        if (rightHandedAction)
        {
            isUsingLeftHand.Value = false;
            isUsingRightHand.Value = true;
        }
        else
        {
            isUsingLeftHand.Value = true;
            isUsingRightHand.Value = false;
        }
    }
    // will also be used later for leveling up / items
    public void SetNewVigorValue(int oldValue, int newValue)
    {
        maxHealth.Value = player.playerStatManager.CalculateHealthBasedOnVigorLevel(newValue);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(maxHealth.Value, currentHealth.Value);
        currentHealth.Value = maxHealth.Value;
    }
    public void SetNewEnduranceValue(int oldValue, int newValue)
    {
        maxStamina.Value = player.playerStatManager.CalculateStaminaBasedOnEnduranceLevel(newValue);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina.Value, currentStamina.Value);
        currentStamina.Value = maxStamina.Value;
    }
    public void SetNewMindValue(int oldValue, int newValue)
    {
        maxFocusPoints.Value = player.playerStatManager.CalculateFocusPointsBasedOnMindLevel(newValue);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxFPValue(maxFocusPoints.Value, currentFocusPoints.Value);
        maxFocusPoints.Value = maxFocusPoints.Value;
    }
    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.instance.playerUIHUDManager.SetRightWeaponQuickSlotIcon(newID);
        }
    }
    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerInventoryManager.currentLeftHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadLeftWeapon();

        if (player.IsOwner)
        {
            PlayerUIManager.instance.playerUIHUDManager.SetLeftWeaponQuickSlotIcon(newID);
        }
    }
    public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
    {

        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();
    }

    [ServerRpc]
    public void NotifyServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)
    {
        if (IsServer)
        {
            NotifyServerOfWeaponActionClientRpc(clientID, actionID, weaponID);
        }
    }
    [ClientRpc]
    public void NotifyServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PlayWeaponAction(actionID, weaponID);
        }
    }
    private void PlayWeaponAction(int actionID, int weaponID)
    {
        WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemAction(actionID);
        if (weaponAction != null)
        {
            weaponAction.AttemptToPerformAction(player, WorldItemDatabase.instance.GetWeaponByID(weaponID));
        }
        else
        {
            Debug.LogError("ACTION IS NULL!");
        }
    }
    //RPCs for sending weapon model
    [ServerRpc(RequireOwnership = false)]
    public void EquipWeaponServerRpc(int weaponID)
    {
        currentRightHandWeaponID.Value = weaponID;
        EquipWeaponClientRpc(weaponID);
    }

    [ClientRpc]
    public void EquipWeaponClientRpc(int weaponID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(weaponID));
        player.playerInventoryManager.currentRightHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadRightWeapon();
    }

}
