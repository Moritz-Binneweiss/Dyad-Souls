using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    public PlayerCamera playerCamera;
    public PlayerInputHandler playerInputManager;

    [HideInInspector]
    public PlayerMovement playerMovement;

    [HideInInspector]
    public PlayerCombatSystem playerCombatSystem;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCombatSystem = GetComponent<PlayerCombatSystem>();
    }

    private void Update()
    {
        if (playerMovement != null)
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

    public void PerformDodge()
    {
        if (playerCombatSystem != null)
        {
            playerCombatSystem.PerformDodge();
        }
    }
}
