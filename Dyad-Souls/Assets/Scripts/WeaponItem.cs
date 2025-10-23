using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;

    [Header("Weapon Poise")]
    public float poiseDamage = 10;

    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;

    [Header("Attack Modifiers")]
    public float lightAttackMultiplier = 1f;
    public float heavyAttackMultiplier = 1.5f;

    [Header("Actions")]
    public WeaponItemAction ohRbAction;
}
