using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    private float currentHealth;

    [SerializeField]
    private Slider bossHealthSlider;

    private bool isAlive = true;

    void Start()
    {
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

        // Deaktiviere Komponenten statt GameObject zu zerstören
        // So können wir ihn für die nächste Wave wiederverwenden
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // TODO: Spiele Death Animation ab
        // Deaktiviere Renderer
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        // Verstecke Boss Health Bar
        if (bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(false);
        }
    }

    public void Revive(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
        isAlive = true;

        // Setze Position zurück auf (0, 0, 0)
        transform.position = Vector3.zero;

        // Reaktiviere Komponenten
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
