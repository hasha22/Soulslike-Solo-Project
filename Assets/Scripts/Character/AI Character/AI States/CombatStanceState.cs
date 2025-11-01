using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "AI/States/Combat Stance")]

public class CombatStanceState : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks; // all possible attacks
    protected List<AICharacterAttackAction> potentialAttacks; // list of attacks possible depending on situation
    private AICharacterAttackAction chosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    protected bool hasRolledForComboChance = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return this;
        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;
        if (!aiCharacter.aiCharacterCombatManager.currentTarget.isAlive.Value)
            return SwitchState(aiCharacter, aiCharacter.idle);
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);
        if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > aiCharacter.aiCharacterCombatManager.maximumEngagementDistance)
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);

        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        if (!hasAttack)
        {
            GetNewAttack(aiCharacter);
        }
        else
        {
            aiCharacter.attack.currentAttack = chosenAttack;
            return SwitchState(aiCharacter, aiCharacter.attack);
        }

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        Vector3 agentVelocity = aiCharacter.navMeshAgent.velocity;
        float speed = agentVelocity.magnitude;
        float maxAgentSpeed = aiCharacter.navMeshAgent.speed;
        float normalizedSpeed = speed / maxAgentSpeed;
        aiCharacter.aiCharacterAnimatorManager.UpdateAnimatorMovementParameters(0f, normalizedSpeed);

        return this;
    }
    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

        foreach (var potentialAttack in aiCharacterAttacks)
        {
            if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;
            if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                continue;
            if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;
            if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            potentialAttacks.Add(potentialAttack);
        }
        if (potentialAttacks.Count <= 0)
            return;
        var totalWeight = 0;
        foreach (var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight + 1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if (randomWeightValue <= processedWeight)
            {
                chosenAttack = attack;
                previousAttack = chosenAttack;
                hasAttack = true;
                return;
            }
        }
    }
    protected virtual bool RollForOutcomeChance(int outcomeChance)
    {
        bool outcomeWillBePerformed = false;
        int randomPercentage = Random.Range(0, 100);
        if (randomPercentage < outcomeChance)
        {
            outcomeWillBePerformed = true;
        }
        return outcomeWillBePerformed;
    }
    protected override void ResetStateFlag(AICharacterManager aiCharacter)
    {
        base.ResetStateFlag(aiCharacter);

        hasRolledForComboChance = false;
        hasAttack = false;
    }
}
