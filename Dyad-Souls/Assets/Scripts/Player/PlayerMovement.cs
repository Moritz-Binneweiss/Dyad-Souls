using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private PlayerManager player;
    private CharacterController characterController;
    private Animator animator;

    [Header("Movement Settings")]
    [SerializeField]
    private float walkingSpeed = 2f;

    [SerializeField]
    private float runningSpeed = 5f;

    [SerializeField]
    private float sprintSpeed = 8f;

    [SerializeField]
    private float rotationSpeed = 15f;

    [Header("Gravity Settings")]
    [SerializeField]
    private float gravityForce = -9.81f;

    [SerializeField]
    private float groundedGravity = -0.05f;

    [Header("Jump Settings")]
    [SerializeField]
    private float jumpHeight = 1.2f;

    [SerializeField]
    private float jumpTimeout = 0.5f;

    [Header("Crouch Settings")]
    [SerializeField]
    private float crouchSpeed = 1f;

    [Header("Ground Check")]
    [SerializeField]
    private float groundedOffset = -0.14f;

    [SerializeField]
    private float groundedRadius = 0.28f;

    [SerializeField]
    private LayerMask groundLayers;

    private Vector3 moveDirection;
    private float verticalVelocity;
    private float jumpTimeoutDelta;
    private bool isGrounded = true;
    private bool isSprinting = false;
    private bool isCrouching = false;

    public bool IsCrouching => isCrouching;
    public bool IsSprinting => isSprinting;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        jumpTimeoutDelta = jumpTimeout;
    }

    public void HandleMovement()
    {
        if (player.playerInputManager == null)
            return;

        GroundedCheck();

        float verticalInput = player.playerInputManager.verticalInput;
        float horizontalInput = player.playerInputManager.horizontalInput;
        float moveAmount = player.playerInputManager.moveAmount;

        if (animator != null)
        {
            animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            animator.SetFloat("Vertical", moveAmount, 0.1f, Time.deltaTime);

            Vector2 move = new Vector2(horizontalInput, verticalInput);
            bool hasSpeedParameter = false;
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "Speed")
                {
                    hasSpeedParameter = true;
                    break;
                }
            }
            if (hasSpeedParameter)
                animator.SetFloat("Speed", move.magnitude);

            animator.SetBool("isCrouching", isCrouching);
        }

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

        if (isSprinting && moveAmount <= 0.1f)
        {
            isSprinting = false;
            if (animator != null)
                animator.SetBool("isSprinting", false);
        }

        if (
            isSprinting
            && player.playerStaminaSystem != null
            && !player.playerStaminaSystem.HasStaminaForSprint()
        )
        {
            isSprinting = false;
            if (animator != null)
                animator.SetBool("isSprinting", false);
        }

        if (isSprinting && moveAmount > 0.1f && player.playerStaminaSystem != null)
            player.playerStaminaSystem.ConsumeSprint(Time.deltaTime);

        ApplyGravityAndJump();

        float speed = (moveAmount > 0.5f) ? runningSpeed : walkingSpeed;

        if (isCrouching)
            speed = crouchSpeed;
        else if (isSprinting && moveAmount > 0.5f)
            speed = sprintSpeed;

        Vector3 horizontalMovement = moveDirection * speed;
        Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);
        characterController.Move((horizontalMovement + verticalMovement) * Time.deltaTime);

        HandleRotation(verticalInput, horizontalInput);
    }

    private void GroundedCheck()
    {
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
            if (verticalVelocity < 0.0f)
                verticalVelocity = groundedGravity;

            if (jumpTimeoutDelta >= 0.0f)
                jumpTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;
            verticalVelocity += gravityForce * Time.deltaTime;
        }
    }

    public void PerformJump()
    {
        if (isGrounded && jumpTimeoutDelta <= 0.0f)
        {
            if (player.playerStaminaSystem != null)
            {
                if (
                    !player.playerStaminaSystem.ConsumeStamina(
                        player.playerStaminaSystem.GetJumpCost()
                    )
                )
                    return;
            }

            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravityForce);

            if (animator != null)
                animator.SetTrigger("Jump");

            jumpTimeoutDelta = jumpTimeout;
        }
    }

    public void ToggleSprint()
    {
        if (
            !isSprinting
            && player.playerStaminaSystem != null
            && !player.playerStaminaSystem.HasStaminaForSprint()
        )
            return;

        isSprinting = !isSprinting;

        if (animator != null)
        {
            bool hasParameter = false;
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == "isSprinting")
                {
                    hasParameter = true;
                    break;
                }
            }

            if (hasParameter)
                animator.SetBool("isSprinting", isSprinting);
        }
    }

    public void PerformCrouch()
    {
        isCrouching = !isCrouching;

        if (animator != null)
            animator.SetBool("isCrouching", isCrouching);

        if (isCrouching && isSprinting)
        {
            isSprinting = false;
            if (animator != null)
                animator.SetBool("isSprinting", false);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded
            ? new Color(0.0f, 1.0f, 0.0f, 0.35f)
            : new Color(1.0f, 0.0f, 0.0f, 0.35f);
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
