using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Attack Damage Values")]
    [SerializeField]
    private float attackDamage = 20f;

    [SerializeField]
    private float heavyAttackDamage = 40f;

    [SerializeField]
    private float rangeAttackDamage = 60f;

    [Header("Attack Ranges")]
    [SerializeField]
    private float attackRange = 2.5f;

    [SerializeField]
    private float heavyAttackRange = 3.5f;

    [SerializeField]
    private float rangeAttackRange = 12f;

    private EnemyManager enemyManager;

    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    #region Animation Events - Called from Boss Animations

    public void DealAttackDamage() => DealDamageInRange(attackRange, attackDamage);

    public void DealHeavyAttackDamage() => DealDamageInRange(heavyAttackRange, heavyAttackDamage);

    public void DealRangeAttackDamage() => DealDamageInRange(rangeAttackRange, rangeAttackDamage);

    #endregion

    #region Damage Logic

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

    #endregion

    #region Public Getters

    public float GetAttackRange() => attackRange;

    public float GetHeavyAttackRange() => heavyAttackRange;

    public float GetRangeAttackRange() => rangeAttackRange;

    public float GetAttackDamage() => attackDamage;

    public float GetHeavyAttackDamage() => heavyAttackDamage;

    public float GetRangeAttackDamage() => rangeAttackDamage;

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
