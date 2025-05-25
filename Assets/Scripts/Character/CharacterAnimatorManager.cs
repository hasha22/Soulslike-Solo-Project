using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string lastDamageAnimationPlayed;
    [SerializeField] private string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
    [SerializeField] private string hit_Forward_Medium_02 = "hit_Forward_Medium_02";
    [SerializeField] private string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
    [SerializeField] private string hit_Backward_Medium_02 = "hit_Backward_Medium_02";
    [SerializeField] private string hit_Left_Medium_01 = "hit_Left_Medium_01";
    [SerializeField] private string hit_Left_Medium_02 = "hit_Left_Medium_02";
    [SerializeField] private string hit_Right_Medium_01 = "hit_Right_Medium_01";
    [SerializeField] private string hit_Right_Medium_02 = "hit_Right_Medium_02";
    [Space]
    public List<string> forward_Medium_Damage = new List<string>();
    public List<string> backward_Medium_Damage = new List<string>();
    public List<string> left_Medium_Damage = new List<string>();
    public List<string> right_Medium_Damage = new List<string>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    protected virtual void Start()
    {
        forward_Medium_Damage.Add(hit_Forward_Medium_01);
        forward_Medium_Damage.Add(hit_Forward_Medium_02);

        backward_Medium_Damage.Add(hit_Backward_Medium_01);
        backward_Medium_Damage.Add(hit_Backward_Medium_02);

        left_Medium_Damage.Add(hit_Left_Medium_01);
        left_Medium_Damage.Add(hit_Left_Medium_02);

        right_Medium_Damage.Add(hit_Right_Medium_01);
        right_Medium_Damage.Add(hit_Right_Medium_02);
    }
    public string GetRandomAnimation(List<string> animationList)
    {
        List<string> finalList = new List<string>();
        foreach (var animation in animationList)
        {
            finalList.Add(animation);
        }

        finalList.Remove(lastDamageAnimationPlayed);

        // Removes null entries
        for (int i = finalList.Count - 1; i > -1; i--)
        {
            if (finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }
    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
    {
        float snappedHorizontal = horizontalValue;
        float snappedVertical = verticalValue;
        if (character.characterNetworkManager.isSprinting.Value)
        {
            snappedVertical = 2;
        }
        character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);

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
    public virtual void PlayAttackActionAnimation(AttackType attackType, string targetAnimation, bool isPerformingAction)
    {
        character.animator.CrossFade(targetAnimation, 0.2f);
        //Stop character from performing an additional action 
        character.isPerformingAction = isPerformingAction;
        character.characterCombatManager.currentAttackType = attackType;
        character.canRotate = false;
        character.canMove = false;


        character.characterNetworkManager.NotifyServerOfAttackAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation);
    }
}
