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

    [HideInInspector]
    public PlayerStaminaSystem playerStaminaSystem;

    private Animator animator;

    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private float healthRegenRate = 2f;

    [SerializeField]
    private float healthRegenDelay = 5f;

    [Header("Death Animation Settings")]
    [SerializeField]
    private float deathAnimationDuration = 2.5f;

    private float currentHealth;
    private bool isDead = false;
    private float timeSinceLastDamage;
    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombatSystem = GetComponent<PlayerCombatSystem>();
        playerStaminaSystem = GetComponent<PlayerStaminaSystem>();
        animator = GetComponent<Animator>();
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

        if (!isDead)
        {
            RegenerateHealth();
        }
    }

    private void RegenerateHealth()
    {
        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= healthRegenDelay && currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateHealthUI();
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
        if (playerCombatSystem != null && !isAttacking)
        {
            playerCombatSystem.PerformAttack();
        }
    }

    public void PerformHeavyAttack()
    {
        if (playerCombatSystem != null && !isAttacking)
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

    public void PerformCrouch()
    {
        if (playerMovement != null)
        {
            playerMovement.PerformCrouch();
        }
    }

    public void PerformSpecialAttack()
    {
        if (playerCombatSystem != null)
        {
            playerCombatSystem.PerformSpecialAttack();
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
        timeSinceLastDamage = 0f;

        UpdateHealthUI();

        // Trigger camera shake
        if (playerCamera != null)
            playerCamera.TriggerShake();

        // Trigger gamepad vibration
        if (playerInputManager != null)
            playerInputManager.TriggerVibration(0.5f, 0.8f, 0.2f);

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

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = false;

        if (playerMovement != null)
            playerMovement.enabled = false;
        if (playerCombatSystem != null)
            playerCombatSystem.enabled = false;
        if (playerInputManager != null)
            playerInputManager.enabled = false;

        if (animator != null)
            animator.SetTrigger("Death");

        GameManager gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager != null)
            gameManager.OnPlayerDeath(this);

        HandleCameraOnDeath();

        Invoke(nameof(DisablePlayer), deathAnimationDuration);
    }

    private void HandleCameraOnDeath()
    {
        // Find all players
        PlayerManager[] allPlayers = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);

        // Check if there's any other living player
        bool hasOtherLivingPlayer = false;
        PlayerManager otherLivingPlayer = null;

        foreach (PlayerManager otherPlayer in allPlayers)
        {
            if (otherPlayer != this && !otherPlayer.IsDead())
            {
                hasOtherLivingPlayer = true;
                otherLivingPlayer = otherPlayer;
                break;
            }
        }

        // Only disable this camera if there's another living player
        if (hasOtherLivingPlayer)
        {
            if (playerCamera != null)
                playerCamera.DisableCamera();

            // Set other player's camera to fullscreen
            if (otherLivingPlayer != null && otherLivingPlayer.playerCamera != null)
                otherLivingPlayer.playerCamera.SetFullscreen(true);
        }
        // If no other living player, keep camera active for game over screen
    }

    private void DisablePlayer()
    {
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.enabled = false;

        if (healthSlider != null)
            healthSlider.gameObject.SetActive(false);

        if (playerStaminaSystem != null && playerStaminaSystem.GetStaminaSlider() != null)
            playerStaminaSystem.GetStaminaSlider().gameObject.SetActive(false);
    }

    public void Revive()
    {
        CancelInvoke(nameof(DisablePlayer));

        isDead = false;
        currentHealth = maxHealth;
        timeSinceLastDamage = 0f;
        UpdateHealthUI();

        foreach (Collider col in GetComponents<Collider>())
            col.enabled = true;

        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            rend.enabled = true;

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        if (healthSlider != null)
            healthSlider.gameObject.SetActive(true);

        if (playerStaminaSystem != null)
        {
            if (playerStaminaSystem.GetStaminaSlider() != null)
                playerStaminaSystem.GetStaminaSlider().gameObject.SetActive(true);
            playerStaminaSystem.ResetStamina();
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            playerMovement.ResetMovementState();
        }
        if (playerCombatSystem != null)
            playerCombatSystem.enabled = true;
        if (playerInputManager != null)
        {
            playerInputManager.enabled = true;
            playerInputManager.ForceReinitializeInput();
        }
    }

    public bool IsDead() => isDead;

    public float GetCurrentHealth() => currentHealth;

    public float GetMaxHealth() => maxHealth;

    public void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
    }

    public Slider GetHealthSlider() => healthSlider;
}
