using BehaviorDesigner.Runtime;
using TMPro;
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

    private float currentHealth;

    [SerializeField]
    private Slider bossHealthSlider;

    [SerializeField]
    private TextMeshProUGUI bossNameText;

    private bool isAlive = true;

    [Header("Death Animation Settings")]
    [SerializeField]
    private float deathAnimationDuration = 3f;

    void Start()
    {
        animator = GetComponent<Animator>();
        behaviorTree = GetComponent<BehaviorTree>();
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

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;

        // Don't hide health bar - let GameManager handle the phase transition
        // if (bossHealthSlider != null)
        //     bossHealthSlider.gameObject.SetActive(false);

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

    public void ResetToFullHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void SetBossName(string newName)
    {
        if (bossNameText != null)
        {
            bossNameText.text = newName;
        }
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
}
