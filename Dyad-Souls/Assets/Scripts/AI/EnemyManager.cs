using BehaviorDesigner.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Components")]
    private Animator animator;

    [Header("Behavior Tree Selection")]
    [SerializeField]
    private BehaviorTree behaviorTree;

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    private float currentHealth;
    private float ghostHealth;
    private float ghostHealthTimer;

    [SerializeField]
    private Slider bossHealthSlider;

    [SerializeField]
    private RectTransform ghostHealthFill;

    [SerializeField]
    private float ghostHealthDelay = 0.5f;

    [SerializeField]
    private float ghostHealthSpeed = 2f;

    [SerializeField]
    private TextMeshProUGUI bossNameText;

    private bool isAlive = true;

    [Header("Death Animation Settings")]
    [SerializeField]
    private float deathAnimationDuration = 3f;

    void Start()
    {
        animator = GetComponent<Animator>();

        // If no behavior tree is assigned, try to get one from the component
        if (behaviorTree == null)
        {
            behaviorTree = GetComponent<BehaviorTree>();
        }

        currentHealth = maxHealth;
        ghostHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        if (isAlive)
        {
            UpdateGhostHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive)
            return;

        // Save current health to ghost health before applying damage
        ghostHealth = currentHealth;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        ghostHealthTimer = 0f;

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

        if (ghostHealthFill != null)
        {
            // Calculate how much width to reduce from the right side
            float ghostHealthPercent = ghostHealth / maxHealth;

            // Get parent width (the full health bar width)
            RectTransform parentRect = ghostHealthFill.parent as RectTransform;
            if (parentRect != null)
            {
                float fullWidth = parentRect.rect.width;
                float ghostWidth = fullWidth * ghostHealthPercent;
                float rightOffset = fullWidth - ghostWidth;

                // Update offsetMax to shrink from right
                Vector2 offsetMax = ghostHealthFill.offsetMax;
                offsetMax.x = -rightOffset;
                ghostHealthFill.offsetMax = offsetMax;
            }
        }
    }

    private void UpdateGhostHealth()
    {
        if (ghostHealth > currentHealth)
        {
            ghostHealthTimer += Time.deltaTime;

            if (ghostHealthTimer >= ghostHealthDelay)
            {
                float decreaseAmount =
                    ghostHealthSpeed * (ghostHealth - currentHealth) * Time.deltaTime;
                ghostHealth -= decreaseAmount;
                ghostHealth = Mathf.Max(ghostHealth, currentHealth);
                UpdateHealthUI();
            }
        }
        else
        {
            ghostHealth = currentHealth;
            ghostHealthTimer = 0f;
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
        ghostHealth = maxHealth;
        ghostHealthTimer = 0f;
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
