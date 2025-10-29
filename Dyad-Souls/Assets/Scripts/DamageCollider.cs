using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Damage Settings")]
    public float physicalDamage = 10f;
    public PlayerManager characterCausingDamage;

    private Collider damageCollider;
    private List<PlayerManager> charactersDamaged = new List<PlayerManager>();

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        if (damageCollider != null)
        {
            damageCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager target = other.GetComponent<PlayerManager>();

        if (
            target != null
            && target != characterCausingDamage
            && !charactersDamaged.Contains(target)
        )
        {
            charactersDamaged.Add(target);
            DamageTarget(target);
        }
    }

    private void DamageTarget(PlayerManager target)
    {
        Debug.Log(
            $"{characterCausingDamage?.gameObject.name} damaged {target.gameObject.name} for {physicalDamage} damage!"
        );
        // Here you can add health reduction logic later
    }

    public void EnableDamageCollider()
    {
        if (damageCollider != null)
        {
            damageCollider.enabled = true;
        }
    }

    public void DisableDamageCollider()
    {
        if (damageCollider != null)
        {
            damageCollider.enabled = false;
        }
        charactersDamaged.Clear();
    }
}
