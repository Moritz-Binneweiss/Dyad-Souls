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

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    private float currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombatSystem = GetComponent<PlayerCombatSystem>();

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
            playerCombatSystem.PerformLightAttack();
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

        // Deaktiviere Player-Komponenten
        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCombatSystem != null)
            playerCombatSystem.enabled = false;

        if (playerInputManager != null)
            playerInputManager.enabled = false;

        // Informiere GameManager über Tod
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnPlayerDeath(this);
        }

        // TODO: Spiele Death Animation ab
    }

    public void Revive()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthUI();

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
