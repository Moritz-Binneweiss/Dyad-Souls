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
    private float attackRange = 2.5f;

    [SerializeField]
    private float heavyAttackRange = 3.5f;

    [SerializeField]
    private float rangeAttackRange = 12f;

    private Animator animator;
    private EnemyManager enemyManager;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
    }

    #region Attack Methods

    public void PerformAttackRight()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
            animator.SetTrigger("AttackRight");
    }

    public void PerformAttackLeft()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
            animator.SetTrigger("AttackLeft");
    }

    public void PerformAttackLeftRight()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
            animator.SetTrigger("AttackLeftRight");
    }

    public void PerformHeavyAttack()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
            animator.SetTrigger("HeavyAttack");
    }

    public void PerformRangeAttack()
    {
        if (isAttacking)
            return;

        isAttacking = true;

        if (animator != null)
            animator.SetTrigger("RangeAttack");
    }
    #endregion

    #region Animation Events

    public void DealAttackDamage() => DealDamageInRange(attackRange, attackDamage);

    public void DealHeavyAttackDamage() => DealDamageInRange(heavyAttackRange, heavyAttackDamage);

    public void DealRangeAttackDamage() => DealDamageInRange(rangeAttackRange, rangeAttackDamage);

    public void OnAttackComplete() => isAttacking = false;

    #endregion

    #region Helper Methods

    private void DealDamageInRange(float range, float damage)
    {
        // Don't deal damage if boss is dead
        if (enemyManager != null && !enemyManager.IsAlive())
            return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                PlayerManager playerManager = col.GetComponent<PlayerManager>();
                if (playerManager != null && !playerManager.IsDead())
                    playerManager.TakeDamage(damage);
            }
        }
    }

    public bool IsAttacking() => isAttacking;

    public float GetAttackRange() => attackRange;

    public float GetHeavyAttackRange() => heavyAttackRange;

    public float GetRangeAttackRange() => rangeAttackRange;

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, heavyAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangeAttackRange);
    }

    #endregion
}
