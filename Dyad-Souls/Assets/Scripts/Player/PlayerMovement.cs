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

    [Header("Jump Settings")]
    [SerializeField]
    float jumpHeight = 1.2f;

    [SerializeField]
    float jumpTimeout = 0.5f;

    [Header("Ground Check")]
    [SerializeField]
    float groundedOffset = -0.14f;

    [SerializeField]
    float groundedRadius = 0.28f;

    [SerializeField]
    LayerMask groundLayers;

    private Vector3 moveDirection;
    private float verticalVelocity;
    private float jumpTimeoutDelta;
    private bool isGrounded = true;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Initialize jump timeout
        jumpTimeoutDelta = jumpTimeout;
    }

    public void HandleMovement()
    {
        if (player.playerInputManager == null)
            return;

        // Ground Check (wie im ThirdPersonController)
        GroundedCheck();

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

        // Apply gravity and jumping
        ApplyGravityAndJump();

        // Move character
        float speed = (moveAmount > 0.5f) ? runningSpeed : walkingSpeed;
        Vector3 horizontalMovement = moveDirection * speed;
        Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);
        Vector3 totalMovement = (horizontalMovement + verticalMovement) * Time.deltaTime;
        characterController.Move(totalMovement);

        // Handle rotation
        HandleRotation(verticalInput, horizontalInput);
    }

    private void GroundedCheck()
    {
        // Set sphere position with offset (wie im ThirdPersonController)
        Vector3 spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z
        );

        isGrounded = Physics.CheckSphere(
            spherePosition,
            groundedRadius,
            groundLayers,
            QueryTriggerInteraction.Ignore
        );
    }

    private void ApplyGravityAndJump()
    {
        if (isGrounded)
        {
            // Reset vertical velocity when grounded
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = groundedGravity;
            }

            // Decrease jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // Reset jump timeout when in air
            jumpTimeoutDelta = jumpTimeout;

            // Apply gravity
            verticalVelocity += gravityForce * Time.deltaTime;
        }
    }

    public void PerformJump()
    {
        if (isGrounded && jumpTimeoutDelta <= 0.0f)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravityForce);

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }

            jumpTimeoutDelta = jumpTimeout;
        }
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

    // Gizmos zum Debuggen des Ground Checks (wie im ThirdPersonController)
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        Gizmos.color = isGrounded ? transparentGreen : transparentRed;

        // Zeichne Sphere für Ground Check
        Gizmos.DrawSphere(
            new Vector3(
                transform.position.x,
                transform.position.y - groundedOffset,
                transform.position.z
            ),
            groundedRadius
        );
    }
}
