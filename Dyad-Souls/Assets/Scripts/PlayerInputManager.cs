using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public enum InputDeviceType
{
    KeyboardMouse,
    Gamepad,
}

public class PlayerInputManager : MonoBehaviour
{
    [Header("Player Assignment")]
    public PlayerManager player;

    [Header("Input Device Settings")]
    public InputDeviceType deviceType = InputDeviceType.KeyboardMouse;

    [Header("Auto-Configure from Lobby")]
    public bool autoConfigureFromLobby = true;
    public string playerName = "Player1"; // "Player1" oder "Player2"

    InputSystem_Actions playerControls;
    private System.Action<InputAction.CallbackContext> movePerformed;
    private System.Action<InputAction.CallbackContext> moveCanceled;
    private System.Action<InputAction.CallbackContext> lookPerformed;
    private System.Action<InputAction.CallbackContext> lookCanceled;

    [Header("Player Movement Input")]
    [SerializeField]
    Vector2 movement;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Camera Movement Input")]
    [SerializeField]
    Vector2 cameraInput;
    public float cameraVerticalInput;
    public float cameraHorizontalInput;

    [Header("Lock On Input")]
    [SerializeField] bool lockOnInput;

    [Header("Player Action Inputs")]
    [SerializeField] bool rbInput = false;


    private void Awake()
    {
        // Auto-Konfiguration aus Lobby wenn aktiviert
        if (autoConfigureFromLobby)
        {
            ConfigureDeviceTypeFromLobby();
        }

        // try to auto-assign player if not set in Inspector
        if (player == null)
        {
            player = GetComponent<PlayerManager>();
            if (player == null)
            {
                player = GetComponentInChildren<PlayerManager>();
            }
        }

        if (player == null)
        {
            Debug.LogWarning(
                $"PlayerInputManager auf {gameObject.name} hat keinen zugewiesenen Player!"
            );
        }
    }

    private void ConfigureDeviceTypeFromLobby()
    {
        string gamepadControls = PlayerPrefs.GetString("GamepadControls", "");
        string keyboardControls = PlayerPrefs.GetString("KeyboardControls", "");

        // Prüfe ob Gamepad diesem Spieler zugewiesen wurde
        if (gamepadControls == playerName)
        {
            deviceType = InputDeviceType.Gamepad;
        }
        // Prüfe ob Keyboard diesem Spieler zugewiesen wurde
        else if (keyboardControls == playerName)
        {
            deviceType = InputDeviceType.KeyboardMouse;
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new InputSystem_Actions();

            movePerformed = i =>
            {
                if (IsCorrectDevice(i.control.device))
                {
                    movement = i.ReadValue<Vector2>();
                }
            };

            lookPerformed = i =>
            {
                if (IsCorrectDevice(i.control.device))
                {
                    cameraInput = i.ReadValue<Vector2>();
                }
            };

            moveCanceled = i =>
            {
                if (IsCorrectDevice(i.control.device))
                {
                    movement = Vector2.zero;
                }
            };

            lookCanceled = i =>
            {
                if (IsCorrectDevice(i.control.device))
                {
                    cameraInput = Vector2.zero;
                }
            };

            playerControls.Player.Move.performed += movePerformed;
            playerControls.Player.Look.performed += lookPerformed;
            playerControls.Player.Move.canceled += moveCanceled;
            playerControls.Player.Look.canceled += lookCanceled;
            playerControls.Player.LockOn.performed += i => lockOnInput = true;
            playerControls.Player.RB.performed += i => rbInput = true;
        }

        playerControls.Enable();
    }

    private bool IsCorrectDevice(UnityEngine.InputSystem.InputDevice device)
    {
        if (deviceType == InputDeviceType.KeyboardMouse)
        {
            return device is Keyboard || device is Mouse;
        }
        else // Gamepad
        {
            return device is Gamepad;
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            if (movePerformed != null)
                playerControls.Player.Move.performed -= movePerformed;
            if (moveCanceled != null)
                playerControls.Player.Move.canceled -= moveCanceled;
            if (lookPerformed != null)
                playerControls.Player.Look.performed -= lookPerformed;
            if (lookCanceled != null)
                playerControls.Player.Look.canceled -= lookCanceled;

            playerControls.Disable();
        }
    }

    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleLockOnInput();
        HandleRBInput();

    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleLockOnInput();
        HandleRBInput();
    }

    private void HandlePlayerMovementInput()
    {
        verticalInput = movement.y;
        horizontalInput = movement.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (moveAmount <= 0.5 && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1f;
        }

        if (player == null)
            return;

        if (player.playerAnimatorManager == null)
        {
            Debug.LogWarning("PlayerAnimatorManager is null in HandlePlayerMovementInput()");
            return;
        }

        player.playerAnimatorManager.UpdateAnimatorMovementParameter(0, moveAmount);
    }

    private void HandleCameraMovementInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;
    }

    private void HandleLockOnInput()
    {
        if (lockOnInput)
        {
            lockOnInput = false;
            return;
        }
    }

    private void HandleRBInput()
    {
        if (rbInput)
        {
            rbInput = false;

            // Add null checks to prevent NullReferenceException
            if (player == null)
            {
                Debug.LogWarning("Player is null in HandleRBInput()");
                return;
            }

            if (player.playerCombatManager == null)
            {
                Debug.LogWarning("PlayerCombatManager is null in HandleRBInput()");
                return;
            }

            if (player.playerInventoryManager == null)
            {
                Debug.LogWarning("PlayerInventoryManager is null in HandleRBInput()");
                return;
            }

            if (player.playerInventoryManager.currentRightHandWeapon == null)
            {
                Debug.LogWarning("CurrentRightHandWeapon is null in HandleRBInput()");
                return;
            }

            player.playerCombatManager.PerformWeaponBasedAction(
                player.playerInventoryManager.currentRightHandWeapon.ohRbAction,
                player.playerInventoryManager.currentRightHandWeapon
            );
        }
    }
}
