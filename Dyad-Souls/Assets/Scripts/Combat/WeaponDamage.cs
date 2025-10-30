using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
