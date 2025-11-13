using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

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
    [SerializeField] private float baseCooldownMin = 1.5f;
    [SerializeField] private float baseCooldownMax = 3.5f;
    [SerializeField] private float heavyAttackCooldown = 4f;
    [SerializeField] private float rangeAttackCooldown = 6f;
    [SerializeField] private float comboCooldown = 2f;
    
    [Header("Boss Phases")]
    [SerializeField] private float phase2HealthThreshold = 0.7f; // 70% HP
    [SerializeField] private float phase3HealthThreshold = 0.3f; // 30% HP

    private float currentHealth;

    [SerializeField]
    private Slider bossHealthSlider;

    private bool isAlive = true;
    
    // Boss Combat State
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

        // Deaktiviere Collider sofort (Enemy kann nicht mehr getroffen werden)
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Verstecke Boss Health Bar sofort
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(false);
        }

        // Spiele Death Animation ab
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Deaktiviere Enemy nach Death-Animation
        Invoke("DisableEnemy", deathAnimationDuration);
    }

    private void DisableEnemy()
    {
        // Deaktiviere alle Renderer (Enemy wird unsichtbar)
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        // Optional: Deaktiviere auch Movement/AI Komponenten
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        EnemyCombatSystem combat = GetComponent<EnemyCombatSystem>();
        if (combat != null)
        {
            combat.enabled = false;
        }
    }

    public void Revive(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        isAlive = true;

        // Setze Position zurück auf (0, 0, 0)
        transform.position = Vector3.zero;

        // Reaktiviere Animator und setze zurück zu Idle
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        // Reaktiviere Collider
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        // Reaktiviere Renderer
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        // Reaktiviere Movement/AI Komponenten
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.enabled = true;
        }

        EnemyCombatSystem combat = GetComponent<EnemyCombatSystem>();
        if (combat != null)
        {
            combat.enabled = true;
        }

        // Zeige Boss Health Bar wieder
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(true);
        }

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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsAlive()
    {
        return isAlive;
    }
    
    #region Boss Combat System (Elden Ring Style)
    
    private void UpdateCooldown()
    {
        if (isInCooldown && Time.time >= lastAttackTime + currentCooldown)
        {
            isInCooldown = false;
            // Kein SetVariableValue nötig - CheckAttackCooldown ruft CanAttack() direkt ab
        }
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
            UpdateBehaviorTreePhase();
            Debug.Log($"Boss entered Phase {currentPhase}!");
        }
    }
    
    private void UpdateBehaviorTreePhase()
    {
        // CurrentPhase wird über GetCurrentPhase() abgerufen - kein SetVariableValue nötig
    }
    
    private void SetRandomCooldown()
    {
        currentCooldown = Random.Range(baseCooldownMin, baseCooldownMax);
    }
    
    public bool CanAttack()
    {
        return !isInCooldown && isAlive;
    }
    
    public void StartAttackCooldown(string attackType)
    {
        lastAttackTime = Time.time;
        lastAttackType = attackType;
        isInCooldown = true;
        
        // Setze Cooldown basierend auf Attackentyp
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
        
        // Phasen-Modifikator (höhere Phasen = schneller)
        switch (currentPhase)
        {
            case 2: currentCooldown *= 0.8f; break; // 20% schneller
            case 3: currentCooldown *= 0.6f; break; // 40% schneller
        }
        
        // CanAttack Status wird über CanAttack() Methode abgerufen - kein SetVariableValue nötig
        
        OnAttackExecuted?.Invoke(attackType);
    }
    
    public string GetBestAttackForSituation(float playerDistance, bool playerBehind, string lastAttack)
    {
        // Elden Ring Logic: Nicht zu viele Attacken hintereinander
        if (consecutiveAttacks >= 4)
        {
            consecutiveAttacks = 0;
            return "Reposition"; // Pause für Repositionierung
        }
        
        consecutiveAttacks++;
        
        // Phasen-basierte Attackenauswahl
        float aggressionBonus = (currentPhase - 1) * 0.2f; // Höhere Phasen = aggressiver
        
        // Kontext-basierte Auswahl
        if (playerDistance > 8f)
        {
            // Weit weg - Range oder Heavy
            return Random.value < (0.6f + aggressionBonus) ? "Range" : "Heavy";
        }
        else if (playerBehind)
        {
            // Hinter dem Boss - Drehattacken
            return Random.value < 0.7f ? "LeftRight" : "Left";
        }
        else if (playerDistance < 3f)
        {
            // Sehr nah - schnelle Attacken oder Heavy
            float rand = Random.value + aggressionBonus;
            if (rand < 0.3f) return "Right";
            else if (rand < 0.6f) return "Left";
            else if (rand < 0.8f) return "LeftRight";
            else return "Heavy";
        }
        else
        {
            // Mittlere Distanz - alle Attacken möglich
            float rand = Random.value;
            if (rand < 0.25f) return "Right";
            else if (rand < 0.45f) return "Left";
            else if (rand < 0.65f) return "Heavy";
            else if (rand < 0.85f) return "LeftRight";
            else return "Range";
        }
    }
    
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public int GetCurrentPhase() => currentPhase;
    public float GetRemainingCooldown() => Mathf.Max(0, (lastAttackTime + currentCooldown) - Time.time);
    
    #endregion
}
