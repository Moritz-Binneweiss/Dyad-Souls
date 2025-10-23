using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{

    [Header("Collider")]
    protected Collider damageCollider;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    [Header("Contact Point")]
    protected Vector3 contactPoint;

    [Header("Characters Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

    private void OggerEnter(Collider other)
    {

    }

    protected virtual void DamageTarget(CharacterManager characterToDamage)
    {

    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }
    
    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear();
    }
}
