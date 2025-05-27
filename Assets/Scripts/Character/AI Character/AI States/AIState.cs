using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager aiCharacter)
    {
        return this;
    }
    protected virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)
    {
        ResetStateFlag(aiCharacter);
        return newState;
    }
    protected virtual void ResetStateFlag(AICharacterManager aiCharacter)
    {

    }
}
