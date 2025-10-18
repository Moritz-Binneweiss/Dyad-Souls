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

    private void Awake()
    {
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

            moveCanceled = i => movement = Vector2.zero;
            lookCanceled = i => cameraInput = Vector2.zero;

            playerControls.Player.Move.performed += movePerformed;
            playerControls.Player.Look.performed += lookPerformed;
            playerControls.Player.Move.canceled += moveCanceled;
            playerControls.Player.Look.canceled += lookCanceled;
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
            // remove the previously stored callbacks so we unregister the same delegates
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

        player.playerAnimatorManager.UpdateAnimatorMovementParameter(0, moveAmount);
    }

    private void HandleCameraMovementInput()
    {
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;
    }
}
