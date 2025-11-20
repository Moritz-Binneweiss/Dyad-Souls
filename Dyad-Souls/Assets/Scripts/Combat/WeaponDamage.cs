using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField]
    private float attackDamage = 10f;

    [SerializeField]
    private float heavyAttackDamage = 100f;

    [SerializeField]
    private float specialAttackDamage = 200f;

    private float currentDamage;

    private void Awake()
    {
        currentDamage = attackDamage;
    }

    public float GetDamage()
    {
        return currentDamage;
    }

    public void SetLightAttackDamage()
    {
        currentDamage = attackDamage;
    }

    public void SetHeavyAttackDamage()
    {
        currentDamage = heavyAttackDamage;
    }

    public void SetDamage(float newDamage)
    {
        currentDamage = newDamage;
    }

    public void SetSpecialAttackDamage()
    {
        currentDamage = specialAttackDamage;
    }
}
