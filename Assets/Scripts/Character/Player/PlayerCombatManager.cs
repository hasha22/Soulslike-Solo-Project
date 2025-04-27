using Unity.Netcode;
public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    //public WeaponItem currentWeaponBeingUsed;
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
}
