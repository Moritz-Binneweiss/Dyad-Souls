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
    private float attackRange = 4f; // Größer für Boss!

    [SerializeField]
    private float heavyAttackRange = 6f; // Größer für Boss!

    [SerializeField]
    private float rangeAttackRange = 12f;

    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    #region Attack Methods
    // Führt einen normalen Angriff aus (20 Damage)
    public void PerformAttackRight()
    {
        if (isAttacking)
        {
            Debug.Log("PerformAttackRight: Already attacking, ignoring");
            return;
        }

        isAttacking = true;
        Debug.Log("PerformAttackRight: Starting attack, setting isAttacking = true");

        if (animator != null)
        {
            animator.SetTrigger("AttackRight");
        }
        else
        {
            Debug.LogError("PerformAttackRight: Animator is null!");
        }
    }

    public void PerformAttackLeft()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("AttackLeft");
        }
    }

    public void PerformAttackLeftRight()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
        {
            animator.SetTrigger("AttackLeftRight");
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
        Debug.Log("OnAttackComplete: Setting isAttacking = false");
        isAttacking = false;
    }
    #endregion

    #region Helper Methods
    // Fügt allen lebenden Spielern in Reichweite Schaden zu
    private void DealDamageInRange(float range, float damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                PlayerManager playerManager = col.GetComponent<PlayerManager>();
                if (playerManager != null && !playerManager.IsDead()) // Nur lebende Spieler
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
