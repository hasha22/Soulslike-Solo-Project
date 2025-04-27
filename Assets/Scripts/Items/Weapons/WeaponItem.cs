using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faiREQ = 0;

    [Header("Weapon Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;
    public float poiseDamage = 0;

    [Header("Attack Modifiers")]
    public float lightAttack01Modifier = 1.1f;

    [Header("Stamina Cost Modifiers")]
    //running attack, jumping attack, sneak attack etc.
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 0.9f;

    [Header("Actions")]
    public WeaponItemAction LeftClickAction;


}
