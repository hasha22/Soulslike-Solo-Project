using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Target Information")]
    public float viewableAngle;
    public float distanceFromTarget;
    public Vector3 targetsDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    public float minimumFOV = -35;
    public float maximumFOV = 35;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed = 25f;

    [Header("Recovery Timer")]
    public float actionRecoveryTimer = 0;

    [Header("Engagement Distance")]
    public float maximumEngagementDistance = 5; // Distance where enemy switches to the Pursue State
    protected override void Awake()
    {
        base.Awake();

        lockOnTransform = GetComponentInChildren<LockOnTransform>().transform;
    }
    public void FindTargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if (currentTarget != null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
            if (targetCharacter == null)
                continue;
            if (targetCharacter == aiCharacter)
                continue;
            if (!targetCharacter.isAlive.Value)
            {
                targetCharacter = null;
                continue;
            }
            if (WorldUtilityManager.instance.CanTargetBeDamaged(character.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if (angleOfTarget > minimumFOV && angleOfTarget < maximumFOV)
                {
                    if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, WorldUtilityManager.instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                    }
                    else
                    {
                        targetsDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }

        }
    }
    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
        {
            aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }
    }
    public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget == null)
            return;
        if (!aiCharacter.canRotate)
            return;
        if (!aiCharacter.isPerformingAction)
            return;
        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
            targetDirection = aiCharacter.transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);
    }
    public void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;
        if (viewableAngle >= 61 && viewableAngle <= 105)
        {
            aiCharacter.characterAnimatorManager.PlayActionAnimation("Skeleton_Turn_Right_01", true);
        }
        else if (viewableAngle <= -61 && viewableAngle >= -105)
        {
            aiCharacter.characterAnimatorManager.PlayActionAnimation("Skeleton_Turn_Left_01", true);
        }
    }
    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if (actionRecoveryTimer > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
