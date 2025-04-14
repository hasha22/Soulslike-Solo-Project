using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects / Instant Effects / Take Stamina Damage")]
public class TakeStaminaDamage : InstantCharacterEffect
{
    public float staminaDamage;
    public override void ProcessEffect(CharacterManager character)
    {
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        if (character.IsOwner)
        {
            Debug.Log("Character is taking: " + staminaDamage);
            character.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
    }
}
