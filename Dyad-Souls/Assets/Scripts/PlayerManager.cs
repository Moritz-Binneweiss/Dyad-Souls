using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    public CharacterController characterController;
    public Animator animator;
    public PlayerCamera playerCamera;
    public PlayerInputManager playerInputManager;

    [Header("Weapon Setup")]
    public GameObject swordPrefab;
    public Transform rightHandBone;
    private GameObject currentSword;
    private DamageCollider swordDamageCollider;

    [Header("Combat Settings")]
    [SerializeField]
    string lightAttackAnimation = "MainLightAttack";

    [SerializeField]
    int swordDamage = 10;

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
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Find right hand bone if not assigned
        if (rightHandBone == null)
        {
            rightHandBone = FindRightHandBone(transform);
        }
    }

    private void Start()
    {
        EquipSword();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        if (playerCamera != null)
        {
            playerCamera.HandleAllCameraActions();
        }
    }

    // === WEAPON SYSTEM ===
    private Transform FindRightHandBone(Transform root)
    {
        // Recursively search for "RightHand" bone
        foreach (Transform child in root.GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("RightHand") || child.name.Contains("Right_Hand"))
            {
                return child;
            }
        }
        return null;
    }

    private void EquipSword()
    {
        if (swordPrefab == null || rightHandBone == null)
        {
            Debug.LogWarning("Sword prefab or right hand bone not assigned!");
            return;
        }

        // Instantiate sword and attach to right hand
        currentSword = Instantiate(swordPrefab, rightHandBone);
        currentSword.transform.localPosition = Vector3.zero;
        currentSword.transform.localRotation = Quaternion.identity;

        // Setup damage collider
        swordDamageCollider = currentSword.GetComponentInChildren<DamageCollider>();
        if (swordDamageCollider != null)
        {
            swordDamageCollider.characterCausingDamage = this;
            swordDamageCollider.physicalDamage = swordDamage;
        }
    }

    // === COMBAT SYSTEM ===
    public void PerformLightAttack()
    {
        if (animator != null && !string.IsNullOrEmpty(lightAttackAnimation))
        {
            animator.CrossFade(lightAttackAnimation, 0.2f, 0);
        }
    }

    // Called by Animation Events
    public void EnableWeaponDamage()
    {
        if (swordDamageCollider != null)
        {
            swordDamageCollider.EnableDamageCollider();
        }
    }

    public void DisableWeaponDamage()
    {
        if (swordDamageCollider != null)
        {
            swordDamageCollider.DisableDamageCollider();
        }
    }

    // === MOVEMENT SYSTEM ===
    private void HandleMovement()
    {
        if (playerInputManager == null)
            return;

        float verticalInput = playerInputManager.verticalInput;
        float horizontalInput = playerInputManager.horizontalInput;
        float moveAmount = playerInputManager.moveAmount;

        // Update animation
        if (animator != null)
        {
            animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
            animator.SetFloat("Vertical", moveAmount, 0.1f, Time.deltaTime);
        }

        // Calculate movement direction based on camera
        if (playerCamera != null)
        {
            moveDirection = playerCamera.transform.forward * verticalInput;
            moveDirection += playerCamera.transform.right * horizontalInput;
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
        if (playerCamera == null)
            return;

        Vector3 targetDirection = playerCamera.cameraObject.transform.forward * verticalInput;
        targetDirection += playerCamera.cameraObject.transform.right * horizontalInput;
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
