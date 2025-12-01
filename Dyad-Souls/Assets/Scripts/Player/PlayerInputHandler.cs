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
    public string playerName = "Player1"; // "Player1" or "Player2"

    InputSystem_Actions playerControls;
    private System.Action<InputAction.CallbackContext> movePerformed;
    private System.Action<InputAction.CallbackContext> moveCanceled;
    private System.Action<InputAction.CallbackContext> lookPerformed;
    private System.Action<InputAction.CallbackContext> lookCanceled;
    private System.Action<InputAction.CallbackContext> attackPerformed;
    private System.Action<InputAction.CallbackContext> dodgePerformed;
    private System.Action<InputAction.CallbackContext> heavyAttackPerformed;
    private System.Action<InputAction.CallbackContext> jumpPerformed;
    private System.Action<InputAction.CallbackContext> sprintPerformed;
    private System.Action<InputAction.CallbackContext> crouchPerformed;
    private System.Action<InputAction.CallbackContext> crouchCanceled;
    private System.Action<InputAction.CallbackContext> specialAttackPerformed;

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

    [SerializeField]
    bool heavyAttackInput = false;

    [Header("Interact Input")]
    [SerializeField]
    private bool isHoldingInteract = false;

    [Header("Player Dodge Inputs")]
    [SerializeField]
    bool dodgeInput = false;

    [Header("Player Jump Input")]
    [SerializeField]
    bool jumpInput = false;

    [Header("Player Sprint Input")]
    [SerializeField]
    bool sprintInput = false;

    [Header("Player Crouch Input")]
    [SerializeField]
    bool crouchInput = false;

    [Header("Special Attack Input")]
    [SerializeField]
    bool specialAttackInput = false;

    private float lastSprintTime = -1f;
    private const float sprintDebounceTime = 0.2f;

    private void Awake()
    {
        if (autoConfigureFromLobby)
            ConfigureDeviceTypeFromLobby();

        if (player == null)
        {
            player = GetComponent<PlayerManager>();
            if (player == null)
                player = GetComponentInChildren<PlayerManager>();
        }
    }

    private void ConfigureDeviceTypeFromLobby()
    {
        string gamepadControls = PlayerPrefs.GetString("GamepadControls", "");
        string keyboardControls = PlayerPrefs.GetString("KeyboardControls", "");

        if (gamepadControls == playerName)
        {
            deviceType = InputDeviceType.Gamepad;
        }
        else if (keyboardControls == playerName)
        {
            deviceType = InputDeviceType.KeyboardMouse;
        }
        else
        {
            if (playerName == "Player1")
            {
                deviceType = InputDeviceType.KeyboardMouse;
            }
            else if (playerName == "Player2")
            {
                deviceType = InputDeviceType.Gamepad;
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new InputSystem_Actions();
            BindToSpecificDevices();

            movePerformed = i => movement = i.ReadValue<Vector2>();
            lookPerformed = i => cameraInput = i.ReadValue<Vector2>();
            moveCanceled = i => movement = Vector2.zero;
            lookCanceled = i => cameraInput = Vector2.zero;
            attackPerformed = i => attackInput = true;
            dodgePerformed = i => dodgeInput = true;
            heavyAttackPerformed = i => heavyAttackInput = true;
            jumpPerformed = i => jumpInput = true;

            sprintPerformed = i =>
            {
                if (i.performed)
                    sprintInput = true;
            };

            crouchPerformed = i =>
            {
                if (i.performed)
                    crouchInput = true;
            };

            crouchCanceled = i => crouchInput = false;
            specialAttackPerformed = i => specialAttackInput = true;

            playerControls.Player.Move.performed += movePerformed;
            playerControls.Player.Look.performed += lookPerformed;
            playerControls.Player.Move.canceled += moveCanceled;
            playerControls.Player.Look.canceled += lookCanceled;
            playerControls.Player.Crouch.canceled += crouchCanceled;
            playerControls.Player.Attack.performed += attackPerformed;
            playerControls.Player.Dodge.performed += dodgePerformed;
            playerControls.Player.HeavyAttack.performed += heavyAttackPerformed;
            playerControls.Player.Jump.performed += jumpPerformed;
            playerControls.Player.Sprint.performed += sprintPerformed;
            playerControls.Player.Crouch.performed += crouchPerformed;
            playerControls.Player.SpecialAttack.performed += specialAttackPerformed;
        }

        playerControls.Enable();
    }

    private void BindToSpecificDevices()
    {
        playerControls.devices = null;

        if (deviceType == InputDeviceType.KeyboardMouse)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            var mouse = UnityEngine.InputSystem.Mouse.current;

            if (keyboard != null && mouse != null)
            {
                playerControls.devices = new UnityEngine.InputSystem.InputDevice[]
                {
                    keyboard,
                    mouse,
                };
            }
        }
        else
        {
            var gamepad = UnityEngine.InputSystem.Gamepad.current;

            if (gamepad != null)
            {
                playerControls.devices = new UnityEngine.InputSystem.InputDevice[] { gamepad };
            }
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
            if (crouchCanceled != null)
                playerControls.Player.Crouch.canceled -= crouchCanceled;
            if (attackPerformed != null)
                playerControls.Player.Attack.performed -= attackPerformed;
            if (dodgePerformed != null)
                playerControls.Player.Dodge.performed -= dodgePerformed;
            if (heavyAttackPerformed != null)
                playerControls.Player.HeavyAttack.performed -= heavyAttackPerformed;
            if (jumpPerformed != null)
                playerControls.Player.Jump.performed -= jumpPerformed;
            if (sprintPerformed != null)
                playerControls.Player.Sprint.performed -= sprintPerformed;
            if (crouchPerformed != null)
                playerControls.Player.Crouch.performed -= crouchPerformed;
            if (specialAttackPerformed != null)
                playerControls.Player.SpecialAttack.performed -= specialAttackPerformed;

            playerControls.Disable();
        }
    }

    private void OnDestroy()
    {
        if (playerControls != null)
            playerControls?.Dispose();
    }

    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleAttackInput();
        HandleHeavyAttackInput();
        HandleInteractInput();
        HandleDodgeInput();
        HandleJumpInput();
        HandleSprintInput();
        HandleCrouchInput();
        HandleSpecialAttackInput();
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
            player?.PerformLightAttack();
        }
    }

    private void HandleHeavyAttackInput()
    {
        if (heavyAttackInput)
        {
            heavyAttackInput = false;
            player?.PerformHeavyAttack();
        }
    }

    private void HandleInteractInput()
    {
        if (playerControls == null)
            return;

        bool isCurrentlyPressed = false;

        foreach (var control in playerControls.Player.Interact.controls)
        {
            if (control.IsPressed())
            {
                isCurrentlyPressed = true;
                break;
            }
        }

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

    private void HandleDodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            player?.PerformDodge();
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            player?.PerformJump();
        }
    }

    private void HandleSprintInput()
    {
        if (sprintInput)
        {
            sprintInput = false;

            if (Time.time - lastSprintTime < sprintDebounceTime)
                return;

            lastSprintTime = Time.time;
            player?.ToggleSprint();
        }
    }

    private void HandleCrouchInput()
    {
        if (crouchInput)
        {
            crouchInput = false;
            player?.PerformCrouch();
        }
    }

    private void HandleSpecialAttackInput()
    {
        if (specialAttackInput)
        {
            specialAttackInput = false;
            player?.PerformSpecialAttack();
        }
    }
}
