using UnityEngine;
[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterCombatManager.currentTarget != null)
        {
            Debug.Log("Found target");
            return this;
        }
        else
        {
            aiCharacter.aiCharacterCombatManager.FindTargetViaLineOfSight(aiCharacter);
            Debug.Log("Searching for target");
            return this;
        }
    }
}
