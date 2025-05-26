public class PlayerAnimationManager : CharacterAnimatorManager
{
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }
    public override void EnableCombo()
    {
        if (player.playerNetworkManager.isUsingRightHand.Value)
        {
            player.playerCombatManager.canComboWithMainHandWeapon = true;
        }
        else if (player.playerNetworkManager.isUsingLeftHand.Value)
        {
            // enable offhand
        }

    }
    public override void DisableCombo()
    {
        player.playerCombatManager.canComboWithMainHandWeapon = false;
        //player.playerCombatManager.canComboWithOffHandWeapon = false;
    }

}
