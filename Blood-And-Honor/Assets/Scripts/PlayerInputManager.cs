using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using UnityEngine;
using UnityEngine.XR;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    InputSystem_Actions playerControls;

    [SerializeField]
    Vector2 movement;

    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new InputSystem_Actions();
            playerControls.Player.Move.performed += i => movement = i.ReadValue<Vector2>();
            playerControls.Player.Move.canceled += i => movement = Vector2.zero;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Player.Move.performed -= i => movement = i.ReadValue<Vector2>();
            playerControls.Player.Move.canceled -= i => movement = Vector2.zero;
            playerControls.Disable();
        }
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
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
}
