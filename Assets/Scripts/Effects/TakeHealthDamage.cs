using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects / Instant Effects / Take Health Damage")]
public class TakeHealthDamage : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0; // will be split into the future 
    public float magicDamage = 0;
    public float holyDamage = 0;
    public float lightningDamage = 0;
    public float fireDamage = 0;
    private int finalDamageDealt = 0;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    [Header("SFX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSFX;

    [Header("Damage Direction")]
    public float angleHitFrom;
    public Vector3 contactPoint;
    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (!character.isAlive.Value)
            return;
        CalculateDamage(character);
        PlayDirectionalBasedDamageAnimation(character);
        PlayDamageSFX(character);
        PlayDamageVFX(character);
    }

    private void CalculateDamage(CharacterManager character)
    {
        if (!character.IsOwner)
            return;
        finalDamageDealt = Mathf.RoundToInt(physicalDamage + holyDamage + magicDamage + fireDamage + lightningDamage);

        if (finalDamageDealt < 0)
        {
            finalDamageDealt = 1;
        }
        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
    }
    private void PlayDamageVFX(CharacterManager character)
    {
        character.characterEffectsManager.PlayBloodSpatterVFX(contactPoint);
    }
    private void PlayDamageSFX(CharacterManager character)
    {
        AudioClip physicalDamageSFX = WorldSFXManager.instance.ChooseRandomSFX(WorldSFXManager.instance.physicalDamageSFX);

        character.characterSFX.PlaySoundFX(physicalDamageSFX);
    }
    private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
    {
        if (!character.IsOwner)
            return;
        poiseIsBroken = true;

        // Front animation
        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.forward_Medium_Damage);
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.forward_Medium_Damage);
        }
        // Back animation
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.backward_Medium_Damage);
        }
        // Left animation
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.left_Medium_Damage);
        }
        // Right animation
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            damageAnimation = character.characterAnimatorManager.GetRandomAnimation(character.characterAnimatorManager.right_Medium_Damage);
        }
        if (poiseIsBroken)
        {
            character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
            if (character.isAlive.Value)
                character.characterAnimatorManager.PlayActionAnimation(damageAnimation, true);
        }
    }
}
