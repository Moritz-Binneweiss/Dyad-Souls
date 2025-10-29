using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private PlayerManager player;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement Settings")]
    [SerializeField]
    float walkingSpeed = 2f;

    [SerializeField]
    float runningSpeed = 5f;

    [SerializeField]
    float rotationSpeed = 15f;

    [Header("Gravity Settings")]
    [SerializeField]
    float gravityForce = -9.81f;

    [SerializeField]
    float groundedGravity = -0.05f;

    private Vector3 moveDirection;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void HandleMovement()
    {
        if (player.playerInputManager == null)
            return;

        float verticalInput = player.playerInputManager.verticalInput;
        float horizontalInput = player.playerInputManager.horizontalInput;
        float moveAmount = player.playerInputManager.moveAmount;

        // Update animation
        if (animator != null)
        {
            animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            animator.SetFloat("Vertical", moveAmount, 0.1f, Time.deltaTime);
        }

        // Calculate movement direction based on camera
        if (player.playerCamera != null)
        {
            moveDirection = player.playerCamera.transform.forward * verticalInput;
            moveDirection += player.playerCamera.transform.right * horizontalInput;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        moveDirection.Normalize();
        moveDirection.y = 0;

        // Apply gravity
        if (characterController.isGrounded)
        {
            moveDirection.y = groundedGravity;
        }
        else
        {
            moveDirection.y += gravityForce * Time.deltaTime;
        }

        // Move character
        float speed = (moveAmount > 0.5f) ? runningSpeed : walkingSpeed;
        Vector3 movement = moveDirection * speed * Time.deltaTime;
        movement.y = moveDirection.y * Time.deltaTime;
        characterController.Move(movement);

        // Handle rotation
        HandleRotation(verticalInput, horizontalInput);
    }

    private void HandleRotation(float verticalInput, float horizontalInput)
    {
        if (player.playerCamera == null)
            return;

        Vector3 targetDirection =
            player.playerCamera.cameraObject.transform.forward * verticalInput;
        targetDirection += player.playerCamera.cameraObject.transform.right * horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
