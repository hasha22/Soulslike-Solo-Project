using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] float minimumDetectionAngle = -35;
    [SerializeField] float maximumDetectionAngle = 35;
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
                continue;
            if (WorldUtilityManager.instance.CanTargetBeDamaged(character.characterGroup, targetCharacter.characterGroup))
            {
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if (viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                {
                    if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position, WorldUtilityManager.instance.GetEnviroLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        Debug.Log("Blocked");
                    }
                    else
                    {
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }

        }
    }
}
