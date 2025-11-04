using UnityEngine;

public class EnemyCombatSystem : MonoBehaviour
{
    [Header("Attack Damage Values")]
    [SerializeField]
    private float attackDamage = 20f;

    [SerializeField]
    private float heavyAttackDamage = 40f;

    [SerializeField]
    private float rangeAttackDamage = 60f;

    [Header("Attack Settings")]
    [SerializeField]
    private float attackRange = 2f;

    [SerializeField]
    private float heavyAttackRange = 2.5f;

    [SerializeField]
    private float rangeAttackRange = 10f;

    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    #region Attack Methods
    // Führt einen normalen Angriff aus (20 Damage)
    public void PerformAttack()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    // Führt einen schweren Angriff aus (40 Damage)
    public void PerformHeavyAttack()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("HeavyAttack");
        }
    }

    // Führt einen Fernkampf-Angriff aus (60 Damage)
    public void PerformRangeAttack()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("RangeAttack");
        }
    }
    #endregion

    #region Animation Events
    // Wird von Animation Event aufgerufen - Normal Attack
    public void DealAttackDamage()
    {
        DealDamageInRange(attackRange, attackDamage);
    }

    // Wird von Animation Event aufgerufen - Heavy Attack
    public void DealHeavyAttackDamage()
    {
        DealDamageInRange(heavyAttackRange, heavyAttackDamage);
    }

    // Wird von Animation Event aufgerufen - Range Attack
    public void DealRangeAttackDamage()
    {
        DealDamageInRange(rangeAttackRange, rangeAttackDamage);
    }

    // Wird am Ende jeder Attack-Animation aufgerufen
    public void OnAttackComplete()
    {
        isAttacking = false;
    }
    #endregion

    #region Helper Methods
    // Fügt allen Spielern in Reichweite Schaden zu
    private void DealDamageInRange(float range, float damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                PlayerManager playerManager = col.GetComponent<PlayerManager>();
                if (playerManager != null)
                {
                    playerManager.TakeDamage(damage);
                    Debug.Log($"Enemy dealt {damage} damage to {col.name}");
                }
            }
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetHeavyAttackRange()
    {
        return heavyAttackRange;
    }

    public float GetRangeAttackRange()
    {
        return rangeAttackRange;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        // Zeige Attack Range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Zeige Heavy Attack Range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, heavyAttackRange);

        // Zeige Range Attack Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);
    }
    #endregion
}
