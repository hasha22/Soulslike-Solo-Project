using UnityEngine;

public class AISkeletonCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField] SkeletonSwordDamageCollider swordDamageCollider;

    [Header("Damage")]
    [SerializeField] int baseDamage = 50;
    [SerializeField] float attackDamageModifier = 1.1f;

    public void SetAttackDamage()
    {
        swordDamageCollider.physicalDamage = baseDamage * attackDamageModifier;
    }
    public void OpenSwordDamageCollider()
    {
        swordDamageCollider.EnableDamageCollider();
    }
    public void CloseSwordDamageCollider()
    {
        swordDamageCollider.DisableDamageCollider();
    }

}
