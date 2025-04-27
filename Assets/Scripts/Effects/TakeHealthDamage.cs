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
}
