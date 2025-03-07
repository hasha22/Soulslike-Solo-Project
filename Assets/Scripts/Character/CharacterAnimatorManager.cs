using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    float vertical;
    float horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        character.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
        character.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
    }

    public virtual void PlayActionAnimation(string targetAnimation, bool isPerformingAction)
    {
        character.animator.CrossFade(targetAnimation, 0.2f);
        //Stop character from performing an additional action 
        character.isPerformingAction = isPerformingAction;
        character.canRotate = false;
        character.canMove = false;

        character.characterNetworkManager.NotifyServerOfActionAnimationRpc(NetworkManager.Singleton.LocalClientId, targetAnimation);
    }
}
