using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Components")]
    private Animator animator;
    private BehaviorTree behaviorTree;

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    [Header("Death Animation Settings")]
    [SerializeField]
    private float deathAnimationDuration = 3f;

    [Header("Boss Combat Settings (Elden Ring Style)")]
    [SerializeField]
    private float baseCooldownMin = 1.5f;

    [SerializeField]
    private float baseCooldownMax = 3.5f;

    [SerializeField]
    private float heavyAttackCooldown = 4f;

    [SerializeField]
    private float rangeAttackCooldown = 6f;

    [SerializeField]
    private float comboCooldown = 2f;

    [Header("Boss Phases")]
    [SerializeField]
    private float phase2HealthThreshold = 0.7f;

    [SerializeField]
    private float phase3HealthThreshold = 0.3f;

    private float currentHealth;

    [SerializeField]
    private Slider bossHealthSlider;
    private bool isAlive = true;

    private float lastAttackTime;
    private float currentCooldown;
    private int currentPhase = 1;
    private string lastAttackType = "";
    private int consecutiveAttacks = 0;
    private bool isInCooldown = false;

    // Events
    public System.Action<int> OnPhaseChanged;
    public System.Action<string> OnAttackExecuted;

    void Start()
    {
        animator = GetComponent<Animator>();
        behaviorTree = GetComponent<BehaviorTree>();
        currentHealth = maxHealth;
        UpdateHealthUI();
        SetRandomCooldown();
    }

    void Update()
    {
        UpdateCooldown();
        CheckPhaseTransition();
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        isAlive = false;

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;

        if (bossHealthSlider != null)
            bossHealthSlider.gameObject.SetActive(false);

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        if (behaviorTree != null)
            behaviorTree.enabled = false;

        Invoke(nameof(DisableEnemy), deathAnimationDuration);
    }

    private void DisableEnemy()
    {
        if (animator != null)
            animator.enabled = false;

        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.enabled = false;
    }

    public void Revive(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        isAlive = true;
        transform.position = Vector3.zero;

        if (animator != null)
        {
            animator.enabled = true;
            animator.Rebind();
            animator.Update(0f);
        }

        if (behaviorTree != null)
            behaviorTree.enabled = true;

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = true;

        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.enabled = true;

        if (bossHealthSlider != null)
            bossHealthSlider.gameObject.SetActive(true);

        UpdateHealthUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isAlive)
            return;

        if (other.CompareTag("Player"))
        {
            return;
        }

        if (other.CompareTag("PlayerWeapon") || other.GetComponentInParent<WeaponDamage>() != null)
        {
            float damage = 10f;

            var damageSource = other.GetComponent<WeaponDamage>();
            if (damageSource == null)
            {
                damageSource = other.GetComponentInParent<WeaponDamage>();
            }

            if (damageSource != null)
            {
                damage = damageSource.GetDamage();
            }

            TakeDamage(damage);
        }
    }

    public float GetCurrentHealth() => currentHealth;

    public float GetMaxHealth() => maxHealth;

    public bool IsAlive() => isAlive;

    #region Boss Combat System (Elden Ring Style)

    private void UpdateCooldown()
    {
        if (isInCooldown && Time.time >= lastAttackTime + currentCooldown)
            isInCooldown = false;
    }

    private void CheckPhaseTransition()
    {
        float healthPercentage = currentHealth / maxHealth;
        int newPhase = 1;

        if (healthPercentage <= phase3HealthThreshold)
            newPhase = 3;
        else if (healthPercentage <= phase2HealthThreshold)
            newPhase = 2;

        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            OnPhaseChanged?.Invoke(currentPhase);
        }
    }

    private void SetRandomCooldown()
    {
        currentCooldown = Random.Range(baseCooldownMin, baseCooldownMax);
    }

    public bool CanAttack() => !isInCooldown && isAlive;

    public void StartAttackCooldown(string attackType)
    {
        lastAttackTime = Time.time;
        lastAttackType = attackType;
        isInCooldown = true;

        switch (attackType)
        {
            case "Heavy":
                currentCooldown = heavyAttackCooldown;
                break;
            case "Range":
                currentCooldown = rangeAttackCooldown;
                break;
            case "LeftRight":
                currentCooldown = comboCooldown;
                break;
            default:
                SetRandomCooldown();
                break;
        }

        switch (currentPhase)
        {
            case 2:
                currentCooldown *= 0.8f;
                break;
            case 3:
                currentCooldown *= 0.6f;
                break;
        }

        OnAttackExecuted?.Invoke(attackType);
    }

    public string GetBestAttackForSituation(
        float playerDistance,
        bool playerBehind,
        string lastAttack
    )
    {
        if (consecutiveAttacks >= 4)
        {
            consecutiveAttacks = 0;
            return "Reposition";
        }

        consecutiveAttacks++;
        float aggressionBonus = (currentPhase - 1) * 0.2f;

        if (playerDistance > 8f)
            return Random.value < (0.6f + aggressionBonus) ? "Range" : "Heavy";
        else if (playerBehind)
            return Random.value < 0.7f ? "LeftRight" : "Left";
        else if (playerDistance < 3f)
        {
            float rand = Random.value + aggressionBonus;
            if (rand < 0.3f)
                return "Right";
            else if (rand < 0.6f)
                return "Left";
            else if (rand < 0.8f)
                return "LeftRight";
            else
                return "Heavy";
        }
        else
        {
            float rand = Random.value;
            if (rand < 0.25f)
                return "Right";
            else if (rand < 0.45f)
                return "Left";
            else if (rand < 0.65f)
                return "Heavy";
            else if (rand < 0.85f)
                return "LeftRight";
            else
                return "Range";
        }
    }

    public float GetHealthPercentage() => currentHealth / maxHealth;

    public int GetCurrentPhase() => currentPhase;

    public float GetRemainingCooldown() =>
        Mathf.Max(0, (lastAttackTime + currentCooldown) - Time.time);

    #endregion
}
