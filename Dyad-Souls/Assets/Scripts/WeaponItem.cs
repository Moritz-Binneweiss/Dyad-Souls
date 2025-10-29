using UnityEngine;

public class WeaponItem : Item
{
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Damage")]
    public int physicalDamage = 10;

    [Header("Attack Animation")]
    public string lightAttackAnimation = "MainLightAttack";
}
