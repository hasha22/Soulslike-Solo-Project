using Unity.Netcode;
using UnityEngine;
public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    [Header("Flags")]
    public bool canComboWithMainHandWeapon = false;
    //public bool canComboWithOffHandWeapon = false;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponItem)
    {
        if (player.IsOwner && weaponAction != null)
        {
            weaponAction.AttemptToPerformAction(player, weaponItem);

            player.playerNetworkManager.NotifyServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId, weaponAction.actionID, weaponItem.itemID);
        }
    }
    public void DrainStaminaBasedOnAttack()
    {
        if (!player.IsOwner)
            return;
        if (currentWeaponBeingUsed == null)
            return;
        float staminaDeducted = 0;
        switch (currentAttackType)
        {
            case AttackType.LightAttack01:
                staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                break;
            default:
                break;
        }
        player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);
    }
    public override void SetTarget(CharacterManager newTarget)
    {
        base.SetTarget(newTarget);

        if (player.IsOwner)
        {
            PlayerCamera.instance.SetLockCameraHeight();
        }
    }
}
