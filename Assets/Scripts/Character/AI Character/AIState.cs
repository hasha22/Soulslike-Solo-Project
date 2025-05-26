using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager aiCharacter)
    {
        Debug.Log("Base state");
        return this;
    }
}
