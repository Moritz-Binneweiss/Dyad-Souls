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

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
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
