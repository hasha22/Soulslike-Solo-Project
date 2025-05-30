using UnityEngine;

public class SkeletonSwordDamageCollider : DamageCollider
{
    [Header("Attacking Character")]
    public AICharacterManager aiCharacterCausingDamage;
    protected override void Awake()
    {
        base.Awake();

        damageCollider = GetComponent<Collider>();
        aiCharacterCausingDamage = GetComponentInParent<AICharacterManager>();
    }
    protected override void DamageTarget(CharacterManager damageTarget)
    {
        if (charactersDamaged.Contains(damageTarget))
            return;

        charactersDamaged.Add(damageTarget);
        TakeHealthDamage damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeHealthDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.contactPoint = contactPoint;
        damageEffect.angleHitFrom = Vector3.SignedAngle(aiCharacterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

        if (aiCharacterCausingDamage.IsOwner)
        {
            damageTarget.characterNetworkManager.NotifyServerOfCharacterDamageServerRpc(
                damageTarget.NetworkObjectId,
                aiCharacterCausingDamage.NetworkObjectId,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.holyDamage,
                damageEffect.lightningDamage,
                damageEffect.fireDamage,
                damageEffect.poiseDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z);
        }
    }
}
