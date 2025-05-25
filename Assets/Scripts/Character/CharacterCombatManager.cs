using Unity.Netcode;
using UnityEngine;

public class CharacterCombatManager : NetworkBehaviour
{
    CharacterManager character;

    [Header("Attack target")]
    public CharacterManager currentTarget;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Attack Type")]
    public WeaponItem currentWeaponBeingUsed;
    public AttackType currentAttackType;
    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public virtual void SetTarget(CharacterManager newTarget)
    {
        if (character.IsOwner)
        {
            if (newTarget != null)
            {
                currentTarget = newTarget;
                character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else
            {
                currentTarget = null;
            }
        }
    }
}
