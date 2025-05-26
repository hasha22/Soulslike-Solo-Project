using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
public class LightAttackAction : WeaponItemAction
{
    [Header("Light Attacks")]
    [SerializeField] string light_Attack_01 = "Light_Attack_01";
    [SerializeField] string light_Attack_02 = "Light_Attack_02";
    public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if (!playerPerformingAction.IsOwner)
            return;

        if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            return;
        if (!playerPerformingAction.isGrounded)
            return;
        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }
    private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;
            if (playerPerformingAction.characterCombatManager.lastAttackAnimation == light_Attack_01)
            {
                playerPerformingAction.playerAnimationManager.PlayAttackActionAnimation(AttackType.LightAttack02, light_Attack_02, true);
            }
            else
            {
                playerPerformingAction.playerAnimationManager.PlayAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
            }
        }
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimationManager.PlayAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
        }
    }
}
