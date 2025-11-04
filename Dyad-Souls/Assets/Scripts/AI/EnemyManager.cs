using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Components")]
    private Animator animator;

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    [Header("Death Animation Settings")]
    [SerializeField]
    private float deathAnimationDuration = 3f;

    private float currentHealth;

    [SerializeField]
    private Slider bossHealthSlider;

    private bool isAlive = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthUI();
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
}
