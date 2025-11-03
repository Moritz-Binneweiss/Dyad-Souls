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
    private float lockOnBreakDistance = 35f; // Distanz, bei der Lock-On automatisch bricht (etwas größer als Range)

    [Header("UI References")]
    [SerializeField]
    private GameObject lockOnIndicatorUI;

    [SerializeField]
    private RectTransform lockOnIndicatorRect; // RectTransform für Positionierung

    [SerializeField]
    private Vector3 uiOffset = Vector3.up * 2f; // Offset über dem Enemy (z.B. über dem Kopf)

    [SerializeField]
    private float uiSmoothSpeed = 1f; // Wie schnell das UI dem Enemy folgt (höher = direkter)

    [Header("Target")]
    [SerializeField]
    private EnemyManager targetEnemy;

    private InputSystem_Actions inputActions;
    private System.Action<InputAction.CallbackContext> lockOnPerformed;
    private bool isLockOnActive = false;

    private void Awake()
    {
        // Auto-assign Player wenn nicht gesetzt
        if (player == null)
        {
            player = GetComponent<PlayerManager>();
        }

        // Auto-assign PlayerInputHandler wenn nicht gesetzt
        if (playerInputHandler == null)
        {
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        // Finde Enemy automatisch falls nicht zugewiesen
        if (targetEnemy == null)
        {
            targetEnemy = FindFirstObjectByType<EnemyManager>();
        }

        // Deaktiviere UI-Indikator zu Beginn
        if (lockOnIndicatorUI != null)
        {
            lockOnIndicatorUI.SetActive(false);

            // Auto-assign RectTransform wenn nicht gesetzt
            if (lockOnIndicatorRect == null)
            {
                lockOnIndicatorRect = lockOnIndicatorUI.GetComponent<RectTransform>();
            }
        }
    }

    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputSystem_Actions();

            // Registriere LockOn Input
            lockOnPerformed = i =>
            {
                // Prüfe ob Input vom korrekten Device kommt
                if (playerInputHandler != null && IsCorrectDevice(i.control.device))
                {
                    ToggleLockOn();
                }
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
        // Prüfe Distanz zum Enemy und aktualisiere UI
        UpdateLockOnIndicator();

        // Aktualisiere UI-Position auch in Update für schnellere Reaktion
        if (isLockOnActive && lockOnIndicatorRect != null && targetEnemy != null)
        {
            UpdateUIPosition();
        }
    }

    private void LateUpdate()
    {
        // Zusätzliches Update in LateUpdate für extra Smoothness
        if (isLockOnActive && lockOnIndicatorRect != null && targetEnemy != null)
        {
            UpdateUIPosition();
        }
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
        // Wenn Lock-On deaktiviert werden soll
        if (isLockOnActive)
        {
            DeactivateLockOn();
            return;
        }

        // Wenn Lock-On aktiviert werden soll, prüfe erst die Bedingungen
        if (targetEnemy == null || player == null)
        {
            return;
        }

        // Prüfe ob Enemy in Range ist
        float distanceToEnemy = Vector3.Distance(
            player.transform.position,
            targetEnemy.transform.position
        );

        if (distanceToEnemy > lockOnRange)
        {
            return;
        }

        // Prüfe ob Enemy am Leben ist
        if (!targetEnemy.IsAlive())
        {
            return;
        }

        // Aktiviere Lock-On
        isLockOnActive = true;

        if (player.playerCamera != null)
        {
            player.playerCamera.SetLockOnTarget(targetEnemy.transform);
        }

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

        // Prüfe ob Enemy in Range ist
        float distanceToEnemy = Vector3.Distance(
            player.transform.position,
            targetEnemy.transform.position
        );

        // Prüfe ob Enemy am Leben ist
        bool isEnemyAlive = targetEnemy.IsAlive();

        // Verwende lockOnBreakDistance für automatisches Deaktivieren (größere Toleranz)
        bool isInBreakRange = distanceToEnemy <= lockOnBreakDistance;

        // Zeige UI nur wenn: Lock-On aktiv, Enemy in normaler Range und Enemy am Leben
        bool isInNormalRange = distanceToEnemy <= lockOnRange;
        bool shouldShowUI = isLockOnActive && isInNormalRange && isEnemyAlive;

        lockOnIndicatorUI.SetActive(shouldShowUI);

        // Automatisches Deaktivieren nur wenn außerhalb der Break-Range oder Enemy tot
        if (isLockOnActive && (!isInBreakRange || !isEnemyAlive))
        {
            DeactivateLockOn();
        }
    }

    // Public Getter für andere Skripte
    public bool IsLockedOn()
    {
        return isLockOnActive;
    }

    public EnemyManager GetLockedTarget()
    {
        if (isLockOnActive)
        {
            return targetEnemy;
        }
        return null;
    }

    public float GetLockOnRange()
    {
        return lockOnRange;
    }

    private void UpdateUIPosition()
    {
        if (
            player == null
            || player.playerCamera == null
            || player.playerCamera.cameraObject == null
        )
            return;

        // Berechne Welt-Position mit Offset (z.B. über dem Kopf des Enemies)
        Vector3 worldPosition = targetEnemy.transform.position + uiOffset;

        // Konvertiere Welt-Position zu Screen-Position
        Vector3 targetScreenPosition = player.playerCamera.cameraObject.WorldToScreenPoint(
            worldPosition
        );

        // Prüfe ob Target vor der Kamera ist
        if (targetScreenPosition.z > 0)
        {
            // Smooth Interpolation zur Zielposition (verhindert Ruckeln)
            Vector3 smoothedPosition = Vector3.Lerp(
                lockOnIndicatorRect.position,
                targetScreenPosition,
                Time.deltaTime * uiSmoothSpeed
            );

            // Setze UI-Position
            lockOnIndicatorRect.position = smoothedPosition;

            // UI sichtbar, wenn vor der Kamera
            if (!lockOnIndicatorUI.activeSelf && isLockOnActive)
            {
                lockOnIndicatorUI.SetActive(true);
            }
        }
        else
        {
            // Target ist hinter der Kamera - verstecke UI
            if (lockOnIndicatorUI.activeSelf)
            {
                lockOnIndicatorUI.SetActive(false);
            }
        }
    }
}
