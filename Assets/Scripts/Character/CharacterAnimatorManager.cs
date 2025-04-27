using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        float horizontalMovement = horizontalValue;
        float verticalMovement = verticalValue;
        if (character.characterNetworkManager.isSprinting.Value)
        {
            verticalMovement = 2;
        }
        character.animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);

    }

    public virtual void PlayActionAnimation(string targetAnimation, bool isPerformingAction)
    {
        character.animator.CrossFade(targetAnimation, 0.2f);
        //Stop character from performing an additional action 
        character.isPerformingAction = isPerformingAction;
        character.canRotate = false;
        character.canMove = false;


        character.characterNetworkManager.NotifyServerOfAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation);
    }
    public virtual void PlayAttackActionAnimation(string targetAnimation, bool isPerformingAction)
    {
        character.animator.CrossFade(targetAnimation, 0.2f);
        //Stop character from performing an additional action 
        character.isPerformingAction = isPerformingAction;
        character.canRotate = false;
        character.canMove = false;


        character.characterNetworkManager.NotifyServerOfAttackAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation);
    }
}
