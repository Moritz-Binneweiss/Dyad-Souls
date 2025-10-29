using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
    {
        if (meleeDamageCollider == null)
        {
            Debug.LogWarning("MeleeWeaponDamageCollider not found!");
            return;
        }

        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
    }
}
