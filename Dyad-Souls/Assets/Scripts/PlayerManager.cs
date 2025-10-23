using UnityEngine;

public class PlayerManager : CharacterManager
{
    [Header("Player Camera")]
    public PlayerCamera playerCamera;

    [Header("Player Input")]
    public PlayerInputManager playerInputManager;

    [HideInInspector]
    public PlayerAnimatorManager playerAnimatorManager;

    [HideInInspector]
    public PlayerLocomotionManager playerLocomotionManager;

    [HideInInspector]
    public PlayerInventoryManager playerInventoryManager;

    [HideInInspector]
    public PlayerCombatManager playerCombatManager;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();

        // Verify all required components are found and provide helpful error messages
        ValidateRequiredComponents();
    }

    private void ValidateRequiredComponents()
    {
        bool hasErrors = false;

        if (playerCombatManager == null)
        {
            Debug.LogError($"PlayerCombatManager component missing on {gameObject.name}! Add PlayerCombatManager component.", this);
            hasErrors = true;
        }
        if (playerAnimatorManager == null)
        {
            Debug.LogError($"PlayerAnimatorManager component missing on {gameObject.name}! Add PlayerAnimatorManager component.", this);
            hasErrors = true;
        }
        if (playerInventoryManager == null)
        {
            Debug.LogError($"PlayerInventoryManager component missing on {gameObject.name}! Add PlayerInventoryManager component.", this);
            hasErrors = true;
        }
        if (playerLocomotionManager == null)
        {
            Debug.LogError($"PlayerLocomotionManager component missing on {gameObject.name}! Add PlayerLocomotionManager component.", this);
            hasErrors = true;
        }

        if (hasErrors)
        {
            Debug.LogError($"Player setup incomplete on {gameObject.name}! Please add all required manager components.", this);
        }
        else
        {
            Debug.Log($"All player components successfully initialized on {gameObject.name}.", this);
        }
    }

    protected override void Update()
    {
        base.Update();

        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        // Jede Kamera folgt ihrem zugewiesenen Spieler
        if (playerCamera != null)
        {
            playerCamera.HandleAllCameraActions();
        }
    }
}
