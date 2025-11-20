using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    [SerializeField]
    private float maxStamina = 100f;

    [SerializeField]
    private float staminaRegenRate = 10f; // Stamina pro Sekunde

    [SerializeField]
    private float staminaRegenDelay = 1f; // Verzögerung nach Verbrauch

    [Header("UI Reference")]
    [SerializeField]
    private Slider staminaSlider;

    [Header("Action Stamina Costs")]
    [SerializeField]
    private float sprintCostPerSecond = 15f;

    [SerializeField]
    private float lightAttackCost = 10f;

    [SerializeField]
    private float heavyAttackCost = 25f;

    [SerializeField]
    private float specialAttackCost = 40f;

    [SerializeField]
    private float dodgeRollCost = 20f;

    [SerializeField]
    private float dodgeBackstepCost = 15f;

    [SerializeField]
    private float jumpCost = 15f;

    private float currentStamina;
    private float timeSinceLastStaminaUse;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    private void Start()
    {
        UpdateStaminaUI();
    }

    private void Update()
    {
        // Regeneriere Stamina nach Verzögerung
        if (timeSinceLastStaminaUse >= staminaRegenDelay)
        {
            RegenerateStamina();
        }
        else
        {
            timeSinceLastStaminaUse += Time.deltaTime;
        }
    }

    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            UpdateStaminaUI();
        }
    }

    public bool ConsumeStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Max(0, currentStamina);
            timeSinceLastStaminaUse = 0f;
            UpdateStaminaUI();
            return true;
        }
        return false;
    }

    public void ConsumeSprint(float deltaTime)
    {
        float cost = sprintCostPerSecond * deltaTime;
        if (currentStamina > 0)
        {
            currentStamina -= cost;
            currentStamina = Mathf.Max(0, currentStamina);
            timeSinceLastStaminaUse = 0f;
            UpdateStaminaUI();
        }
    }

    public bool HasStamina(float amount)
    {
        return currentStamina >= amount;
    }

    public bool HasStaminaForSprint()
    {
        return currentStamina > 0;
    }

    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    // Getter für Stamina Costs - für externe Abfragen
    public float GetLightAttackCost() => lightAttackCost;
    public float GetHeavyAttackCost() => heavyAttackCost;
    public float GetSpecialAttackCost() => specialAttackCost;
    public float GetDodgeRollCost() => dodgeRollCost;
    public float GetDodgeBackstepCost() => dodgeBackstepCost;
    public float GetJumpCost() => jumpCost;

    // Getter für aktuellen Status
    public float GetCurrentStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;

    public Slider GetStaminaSlider() => staminaSlider;
}
