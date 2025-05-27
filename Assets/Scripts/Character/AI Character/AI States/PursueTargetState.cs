using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "AI/States/Pursue Target")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        // Checks if it's performing action, repeats until action is over
        if (aiCharacter.isPerformingAction)
            return this;
        // Switches to idle state if target is null
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);
        // Enables NavMeshAgent
        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        // Rotating towards player
        aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

        // Setting path
        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);

        // Updating movement parameters
        /*
        Vector3 agentVelocity = aiCharacter.navMeshAgent.velocity;
        float speed = agentVelocity.magnitude;
        float maxAgentSpeed = aiCharacter.navMeshAgent.speed;
        float normalizedSpeed = speed / maxAgentSpeed;
        aiCharacter.aiCharacterAnimatorManager.UpdateAnimatorMovementParameters(0f, normalizedSpeed);
        */
        return this;
    }
}
