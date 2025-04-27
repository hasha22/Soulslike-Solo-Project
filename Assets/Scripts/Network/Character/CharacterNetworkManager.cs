using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(NetworkTransform))]
public class CharacterNetworkManager : NetworkBehaviour
{
    CharacterManager character;
    PlayerInventoryManager characterInventoryManager;
    PlayerEquipmentManager characterEquipmentManager;
    private NetworkTransform networkTransform;

    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>
        (Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>
        (Quaternion.identity,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    public Vector3 networkPositionVelocity;
    public float networkPositionSmoothTime = 0.1f;
    public float networkRotationSmoothTime = 0.1f;

    [Header("Animator")]
    public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> horizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> moveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Flags")]
    public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> isJumping = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Resources")]
    public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxHealth = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> maxFocusPoints = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> currentFocusPoints = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Stats")]
    public NetworkVariable<int> endurance = new NetworkVariable<int>(11, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> vigor = new NetworkVariable<int>(15, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> mind = new NetworkVariable<int>(8, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected virtual void Awake()
    {
        networkTransform = GetComponent<NetworkTransform>();
        character = GetComponent<CharacterManager>();
        characterEquipmentManager = GetComponent<PlayerEquipmentManager>();
        characterInventoryManager = GetComponent<PlayerInventoryManager>();
    }
    public void CheckHP(float oldValue, float newValue)
    {
        if (currentHealth.Value <= 0)
        {
            StartCoroutine(character.ProcessDeathEvent(false));
        }
        if (character.IsOwner)
        {
            if (currentHealth.Value > maxHealth.Value)
            {
                currentHealth.Value = maxHealth.Value;
            }
        }
    }
    [ServerRpc]
    public void NotifyServerOfAnimationServerRpc(ulong clientID, string animationID)
    {
        if (IsServer)
        {
            PlayAnimationForAllClientsClientRpc(clientID, animationID);
        }
    }
    [ClientRpc]
    public void PlayAnimationForAllClientsClientRpc(ulong clientID, string animationID)
    {
        PerformActionAnimationFromServer(animationID);
    }
    private void PerformActionAnimationFromServer(string animationID)
    {
        character.animator.CrossFade(animationID, 0.2f);
    }

    [ServerRpc]
    public void NotifyServerOfAttackAnimationServerRpc(ulong clientID, string animationID)
    {
        if (IsServer)
        {
            PlayAttackAnimationForAllClientsClientRpc(clientID, animationID);
        }
    }
    [ClientRpc]
    public void PlayAttackAnimationForAllClientsClientRpc(ulong clientID, string animationID)
    {
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformAttackActionAnimationFromServer(animationID);
        }
    }
    private void PerformAttackActionAnimationFromServer(string animationID)
    {
        character.animator.CrossFade(animationID, 0.2f);
    }


    /*
    public void EquipWeaponRpc(int weaponID)
    {
        WeaponItem selectedWeapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if (selectedWeapon != null)
        {
            GameObject weaponObject = Instantiate(selectedWeapon.weaponModel);
            NetworkObject netObj = weaponObject.GetComponent<NetworkObject>();
            if (netObj != null)
            {
                // Spawn the weapon across the network
                netObj.Spawn();
                // Update the player's inventory or other systems
                characterInventoryManager.currentRightHandWeapon = selectedWeapon;
                characterEquipmentManager.rightHandWeaponModel = weaponObject;
            }
        }
    }
    */
}


