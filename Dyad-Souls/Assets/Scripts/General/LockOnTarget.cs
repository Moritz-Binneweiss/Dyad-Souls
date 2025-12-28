using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LockOnTarget : MonoBehaviour
{
    [Header("Player Assignment")]
    [SerializeField]
    private PlayerManager player;

    [SerializeField]
    private PlayerInputHandler playerInputHandler;

    [Header("Lock-On Settings")]
    [SerializeField]
    private float lockOnRange = 30f;

    [SerializeField]
    private float lockOnBreakDistance = 35f;

    [Header("UI References")]
    [SerializeField]
    private GameObject lockOnIndicatorUI;

    [SerializeField]
    private RectTransform lockOnIndicatorRect;

    [SerializeField]
    private Vector3 uiOffset = Vector3.up * 2f;

    [SerializeField]
    private float uiSmoothSpeed = 1f;

    [Header("Target")]
    [SerializeField]
    private EnemyManager targetEnemy;

    private InputSystem_Actions inputActions;
    private System.Action<InputAction.CallbackContext> lockOnPerformed;
    private bool isLockOnActive = false;

    public void UpdateTargetEnemy(EnemyManager newTarget)
    {
        targetEnemy = newTarget;

        // If currently locked on, update the lock-on to the new target
        if (isLockOnActive && player != null && player.playerCamera != null && newTarget != null)
        {
            player.playerCamera.SetLockOnTarget(newTarget.transform);
        }
    }

    public void SetTargetEnemy(EnemyManager newTarget)
    {
        targetEnemy = newTarget;

        // If currently locked on, update the lock-on to the new target
        if (isLockOnActive && player != null && player.playerCamera != null && newTarget != null)
        {
            player.playerCamera.SetLockOnTarget(newTarget.transform);
        }
    }

    private void Awake()
    {
        if (player == null)
            player = GetComponent<PlayerManager>();

        if (playerInputHandler == null)
            playerInputHandler = GetComponent<PlayerInputHandler>();

        if (targetEnemy == null)
            targetEnemy = FindFirstObjectByType<EnemyManager>();

        if (lockOnIndicatorUI != null)
        {
            lockOnIndicatorUI.SetActive(false);

            if (lockOnIndicatorRect == null)
                lockOnIndicatorRect = lockOnIndicatorUI.GetComponent<RectTransform>();
        }
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();

            lockOnPerformed = i =>
            {
                if (playerInputHandler != null && IsCorrectDevice(i.control.device))
                    ToggleLockOn();
            };

            inputActions.Player.LockOnTarget.performed += lockOnPerformed;
        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            if (lockOnPerformed != null)
            {
                inputActions.Player.LockOnTarget.performed -= lockOnPerformed;
            }

            inputActions.Disable();
        }
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void Update()
    {
        UpdateLockOnIndicator();

        if (isLockOnActive && lockOnIndicatorRect != null && targetEnemy != null)
            UpdateUIPosition();
    }

    private void LateUpdate()
    {
        if (isLockOnActive && lockOnIndicatorRect != null && targetEnemy != null)
            UpdateUIPosition();
    }

    private bool IsCorrectDevice(UnityEngine.InputSystem.InputDevice device)
    {
        if (playerInputHandler == null)
            return false;

        if (playerInputHandler.deviceType == InputDeviceType.KeyboardMouse)
        {
            return device is Keyboard || device is Mouse;
        }
        else // Gamepad
        {
            return device is Gamepad;
        }
    }

    private void ToggleLockOn()
    {
        if (isLockOnActive)
        {
            DeactivateLockOn();
            return;
        }

        if (targetEnemy == null || player == null)
            return;

        float distanceToEnemy = Vector3.Distance(
            player.transform.position,
            targetEnemy.transform.position
        );

        if (distanceToEnemy > lockOnRange)
            return;

        if (!targetEnemy.IsAlive())
            return;

        isLockOnActive = true;

        if (player.playerCamera != null)
            player.playerCamera.SetLockOnTarget(targetEnemy.transform);

        UpdateLockOnIndicator();
    }

    private void DeactivateLockOn()
    {
        isLockOnActive = false;

        if (player != null && player.playerCamera != null)
        {
            player.playerCamera.ClearLockOnTarget();
        }

        UpdateLockOnIndicator();
    }

    private void UpdateLockOnIndicator()
    {
        if (lockOnIndicatorUI == null || targetEnemy == null || player == null)
            return;

        float distanceToEnemy = Vector3.Distance(
            player.transform.position,
            targetEnemy.transform.position
        );

        bool isEnemyAlive = targetEnemy.IsAlive();
        bool isInBreakRange = distanceToEnemy <= lockOnBreakDistance;
        bool isInNormalRange = distanceToEnemy <= lockOnRange;
        bool shouldShowUI = isLockOnActive && isInNormalRange && isEnemyAlive;

        lockOnIndicatorUI.SetActive(shouldShowUI);

        if (isLockOnActive && (!isInBreakRange || !isEnemyAlive))
            DeactivateLockOn();
    }

    public bool IsLockedOn() => isLockOnActive;

    public EnemyManager GetLockedTarget() => isLockOnActive ? targetEnemy : null;

    public float GetLockOnRange() => lockOnRange;

    private void UpdateUIPosition()
    {
        if (
            player == null
            || player.playerCamera == null
            || player.playerCamera.cameraObject == null
        )
            return;

        Vector3 worldPosition = targetEnemy.transform.position + uiOffset;
        Vector3 targetScreenPosition = player.playerCamera.cameraObject.WorldToScreenPoint(
            worldPosition
        );

        if (targetScreenPosition.z > 0)
        {
            Vector3 smoothedPosition = Vector3.Lerp(
                lockOnIndicatorRect.position,
                targetScreenPosition,
                Time.deltaTime * uiSmoothSpeed
            );

            lockOnIndicatorRect.position = smoothedPosition;

            if (!lockOnIndicatorUI.activeSelf && isLockOnActive)
                lockOnIndicatorUI.SetActive(true);
        }
        else
        {
            if (lockOnIndicatorUI.activeSelf)
                lockOnIndicatorUI.SetActive(false);
        }
    }
}
