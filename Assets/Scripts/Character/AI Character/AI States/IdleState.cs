using UnityEngine;
[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterCombatManager.currentTarget != null)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }
        else
        {
            aiCharacter.aiCharacterCombatManager.FindTargetViaLineOfSight(aiCharacter);
            return this;
        }
    }
}
