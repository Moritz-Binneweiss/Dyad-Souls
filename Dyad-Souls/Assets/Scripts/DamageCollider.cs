using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    protected Collider damageCollider;

    [Header("Damage")]
    public float physicalDamage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Characters Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    protected virtual void Awake()
    {
        damageCollider = GetComponent<Collider>();
        if (damageCollider != null)
        {
            damageCollider.enabled = false;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CharacterManager target = other.GetComponent<CharacterManager>();

        if (target != null && !charactersDamaged.Contains(target))
        {
            charactersDamaged.Add(target);
            DamageTarget(target);
        }
    }

    protected virtual void DamageTarget(CharacterManager characterToDamage)
    {
        Debug.Log($"Damaged {characterToDamage.gameObject.name} for {physicalDamage} damage!");
    }

    public virtual void EnableDamageCollider()
    {
        if (damageCollider != null)
        {
            damageCollider.enabled = true;
        }
    }

    public virtual void DisableDamageCollider()
    {
        if (damageCollider != null)
        {
            damageCollider.enabled = false;
        }
        charactersDamaged.Clear();
    }
}
