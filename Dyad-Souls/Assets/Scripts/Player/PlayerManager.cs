using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    public PlayerCamera playerCamera;
    public PlayerInputHandler playerInputManager;

    [HideInInspector]
    public PlayerMovement playerMovement;

    [HideInInspector]
    public PlayerCombatSystem playerCombatSystem;

    private Animator animator;

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    [Header("Death Animation Settings")]
    [SerializeField]
    private float deathAnimationDuration = 2.5f;

    private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombatSystem = GetComponent<PlayerCombatSystem>();
        animator = GetComponent<Animator>();

        // Initialisiere Health
        currentHealth = maxHealth;
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    private void Update()
    {
        if (playerMovement != null && !isDead)
        {
            playerMovement.HandleMovement();
        }
    }

    private void LateUpdate()
    {
        if (playerCamera != null)
        {
            playerCamera.HandleAllCameraActions();
        }
    }

    public void PerformLightAttack()
    {
        if (playerCombatSystem != null)
        {
            playerCombatSystem.PerformAttack();
        }
    }

    public void PerformHeavyAttack()
    {
        if (playerCombatSystem != null)
        {
            playerCombatSystem.PerformHeavyAttack();
        }
    }

    public void PerformDodge()
    {
        if (playerCombatSystem != null)
        {
            playerCombatSystem.PerformDodge();
        }
    }

    public void PerformJump()
    {
        if (playerMovement != null)
        {
            playerMovement.PerformJump();
        }
    }

    public void ToggleSprint()
    {
        if (playerMovement != null)
        {
            playerMovement.ToggleSprint();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        isDead = true;

        // Deaktiviere Collider sofort (Player kann nicht mehr getroffen werden)
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // Deaktiviere Player-Komponenten sofort
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCombatSystem != null)
            playerCombatSystem.enabled = false;

        if (playerInputManager != null)
            playerInputManager.enabled = false;

        // Spiele Death Animation ab
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // Informiere GameManager über Tod
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnPlayerDeath(this);
        }

        // Deaktiviere Player nach Death-Animation
        Invoke("DisablePlayer", deathAnimationDuration);
    }

    private void DisablePlayer()
    {
        // Deaktiviere alle Renderer (Player wird unsichtbar)
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        // Verstecke Health Bar
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(false);
        }
    }

    public void Revive()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthUI();

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

        // Reaktiviere Animator und setze zurück zu Idle
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        // Zeige Health Bar wieder
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(true);
        }

        // Reaktiviere Player-Komponenten
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerCombatSystem != null)
            playerCombatSystem.enabled = true;

        if (playerInputManager != null)
            playerInputManager.enabled = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public Slider GetHealthSlider()
    {
        return healthSlider;
    }
}
