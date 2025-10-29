using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Player Assignment")]
    public PlayerManager player;

    InputSystem_Actions playerControls;

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

    private void Awake()
    {
        if (player == null)
        {
            player = GetComponent<PlayerManager>();
            if (player == null)
            {
                Debug.LogWarning(
                    $"PlayerInputManager auf {gameObject.name} hat keinen zugewiesenen Player!"
                );
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new InputSystem_Actions();

            playerControls.Player.Move.performed += i => movement = i.ReadValue<Vector2>();
            playerControls.Player.Move.canceled += i => movement = Vector2.zero;

            playerControls.Player.Look.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.Player.Look.canceled += i => cameraInput = Vector2.zero;

            playerControls.Player.Attack.performed += i => attackInput = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleAttackInput();
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
}
