using Unity.Mathematics;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    [Header("Gravity Settings")]
    [SerializeField]
    float gravityForce = -9.81f;

    [SerializeField]
    float groundedGravity = -0.05f;

    [SerializeField]
    float walkingSpeed = 2;

    [SerializeField]
    float runningSpeed = 5;

    [SerializeField]
    float rotationSpeed = 15;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        if (PlayerInputManager.instance != null)
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }
    }

    private void HandleGroundMovement()
    {
        GetVerticalAndHorizontalInputs();

        // Bewegung basierend auf Kameraausrichtung
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        // Gravity anwenden
        if (player.characterController.isGrounded)
        {
            moveDirection.y = groundedGravity;
        }
        else
        {
            moveDirection.y += gravityForce * Time.deltaTime;
        }

        if (PlayerInputManager.instance != null && PlayerInputManager.instance.moveAmount > 0.5f)
        {
            Vector3 movement = moveDirection * runningSpeed * Time.deltaTime;
            movement.y = moveDirection.y * Time.deltaTime;
            player.characterController.Move(movement);
        }
        else if (
            PlayerInputManager.instance != null
            && PlayerInputManager.instance.moveAmount <= 0.5f
        )
        {
            Vector3 movement = moveDirection * walkingSpeed * Time.deltaTime;
            movement.y = moveDirection.y * Time.deltaTime;
            player.characterController.Move(movement);
        }
        else
        {
            // Auch wenn keine Bewegung, Gravity anwenden
            Vector3 gravityMovement = new Vector3(0, moveDirection.y * Time.deltaTime, 0);
            player.characterController.Move(gravityMovement);
        }
    }

    private void HandleRotation()
    {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection =
            PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
        targetRotationDirection =
            targetRotationDirection
            + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(
            transform.rotation,
            newRotation,
            rotationSpeed * Time.deltaTime
        );
        transform.rotation = targetRotation;
    }
}
