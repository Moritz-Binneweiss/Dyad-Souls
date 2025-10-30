using UnityEngine;
using UnityEngine.InputSystem;

public enum InputDeviceType
{
    KeyboardMouse,
    Gamepad,
}

public class PlayerInputHandler : MonoBehaviour
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
    private System.Action<InputAction.CallbackContext> attackPerformed;

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

    [Header("Player Action Inputs")]
    [SerializeField]
    bool attackInput = false;

    [Header("Interact Input")]
    [SerializeField]
    private bool isHoldingInteract = false;

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
            Debug.Log($"{playerName} configured for Gamepad");
        }
        // Prüfe ob Keyboard diesem Spieler zugewiesen wurde
        else if (keyboardControls == playerName)
        {
            deviceType = InputDeviceType.KeyboardMouse;
            Debug.Log($"{playerName} configured for Keyboard/Mouse");
        }
        // Standard: Player1 = Keyboard, Player2 = Gamepad
        else
        {
            if (playerName == "Player1")
            {
                deviceType = InputDeviceType.KeyboardMouse;
                Debug.Log($"{playerName} using default Keyboard/Mouse");
            }
            else if (playerName == "Player2")
            {
                deviceType = InputDeviceType.Gamepad;
                Debug.Log($"{playerName} using default Gamepad");
            }
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

            attackPerformed = i =>
            {
                if (IsCorrectDevice(i.control.device))
                {
                    attackInput = true;
                }
            };

            playerControls.Player.Move.performed += movePerformed;
            playerControls.Player.Look.performed += lookPerformed;
            playerControls.Player.Move.canceled += moveCanceled;
            playerControls.Player.Look.canceled += lookCanceled;
            playerControls.Player.Attack.performed += attackPerformed;
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
            if (attackPerformed != null)
                playerControls.Player.Attack.performed -= attackPerformed;

            playerControls.Disable();
        }
    }

    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleAttackInput();
        HandleInteractInput();
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
    }

    private void HandleCameraMovementInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;
    }

    private void HandleAttackInput()
    {
        if (attackInput)
        {
            attackInput = false;

            if (player != null)
            {
                player.PerformLightAttack();
            }
        }
    }

    private void HandleInteractInput()
    {
        if (playerControls == null)
            return;

        // Prüfe ob die Interact-Taste vom richtigen Device gedrückt wird
        bool isCurrentlyPressed = false;

        // Prüfe alle aktiven Controls der Interact-Action
        foreach (var control in playerControls.Player.Interact.controls)
        {
            if (control.IsPressed() && IsCorrectDevice(control.device))
            {
                isCurrentlyPressed = true;
                break;
            }
        }

        // Nur wenn sich der Zustand geändert hat, informiere den PositionSwapManager
        if (isCurrentlyPressed != isHoldingInteract)
        {
            isHoldingInteract = isCurrentlyPressed;

            if (PositionSwapManager.Instance != null)
            {
                if (playerName == "Player1")
                {
                    PositionSwapManager.Instance.SetPlayer1Holding(isCurrentlyPressed);
                }
                else if (playerName == "Player2")
                {
                    PositionSwapManager.Instance.SetPlayer2Holding(isCurrentlyPressed);
                }
            }
        }
    }
}
