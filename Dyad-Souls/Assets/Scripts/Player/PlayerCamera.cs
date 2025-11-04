using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Player Assignment")]
    public PlayerManager player;
    public Camera cameraObject;

    [SerializeField]
    Transform cameraPivotTransform;

    [Header("Camera Settings")]
    [SerializeField]
    private float cameraSmoothSpeed = 1;

    [SerializeField]
    float upAndDownRotationSpeed = 220;

    [SerializeField]
    float leftAndRightRotationSpeed = 220;

    [SerializeField]
    float minumPivot = -30;

    [SerializeField]
    float maximumPivot = 60;

    [SerializeField]
    float cameraCollisionRadius = 0.2f;

    [SerializeField]
    LayerMask collideWithLayers;

    private const float _inputThreshold = 0.01f; // Threshold für Input (verhindert Mikro-Bewegungen)

    [Header("Camera Values")]
    private Vector3 cameraVelocity;

    private Vector3 cameraObjectPosition;

    [SerializeField]
    float leftAndRightLookAngle;

    [SerializeField]
    float upAndDownLookAngle;

    private float cameraZPosition;
    private float targetCameraZPosition;

    [Header("Lock-On Settings")]
    private bool isLockedOn = false;
    private Transform lockOnTarget;

    [SerializeField]
    private float lockOnRotationSpeed = 10f; // Geschwindigkeit der Lock-On Rotation

    [SerializeField]
    private float lockOnHeightOffset = 1.5f; // Höhen-Offset zum Anvisieren (z.B. Oberkörper statt Füße)

    private void Start()
    {
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();

            // Verwende Lock-On Rotation wenn aktiv, ansonsten normale Rotation
            if (isLockedOn && lockOnTarget != null)
            {
                HandleLockOnRotations();
            }
            else
            {
                HandleRotations();
            }

            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(
            transform.position,
            player.transform.position,
            ref cameraVelocity,
            cameraSmoothSpeed * Time.deltaTime
        );
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        if (player == null || player.playerInputManager == null)
            return;

        // Nur rotieren wenn es signifikanten Input gibt (wie im ThirdPersonController)
        float cameraHorizontalInput = player.playerInputManager.cameraHorizontalInput;
        float cameraVerticalInput = player.playerInputManager.cameraVerticalInput;

        // Berechne Input-Magnitude für Threshold-Check
        float inputMagnitude =
            cameraHorizontalInput * cameraHorizontalInput
            + cameraVerticalInput * cameraVerticalInput;

        if (inputMagnitude >= _inputThreshold)
        {
            leftAndRightLookAngle +=
                (cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            upAndDownLookAngle -= (cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
        }

        // Clamp Winkel mit besserer Normalisierung (wie ThirdPersonController)
        leftAndRightLookAngle = ClampAngle(leftAndRightLookAngle, float.MinValue, float.MaxValue);
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        // Horizontale Rotation (Y-Achse)
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        // Vertikale Rotation (X-Achse) am Pivot
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    // Utility Methode aus ThirdPersonController - Normalisiert Winkel
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        // Richtung für Collisionserkennung
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        // Wir gucken ob ein Objekt vor uns ist von der Kamera
        if (
            Physics.SphereCast(
                cameraPivotTransform.position,
                cameraCollisionRadius,
                direction,
                out hit,
                Mathf.Abs(targetCameraZPosition),
                collideWithLayers
            )
        )
        {
            // Ignoriere Kollisionen mit dem eigenen Spieler und seinen Children (z.B. Waffe)
            if (hit.transform.IsChildOf(player.transform) || hit.transform == player.transform)
            {
                // Tue nichts - ignoriere diese Kollision
            }
            else
            {
                // Wenn ja, nehmen wir die Distanz zwischen Kamera und Objekt
                float distanceFromHitObject = Vector3.Distance(
                    cameraPivotTransform.position,
                    hit.point
                );
                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(
            cameraObject.transform.localPosition.z,
            targetCameraZPosition,
            0.2f
        );
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

    private void HandleLockOnRotations()
    {
        if (lockOnTarget == null)
        {
            // Wenn Target verloren geht, wechsle zurück zu normaler Rotation
            isLockedOn = false;
            return;
        }

        // Berechne die Zielposition mit Höhen-Offset (z.B. Oberkörper des Enemies)
        Vector3 targetPosition = lockOnTarget.position + Vector3.up * lockOnHeightOffset;

        // Berechne die Richtung vom Kamera-Pivot zum Target (für horizontale Rotation)
        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0; // Ignoriere Y-Achse für horizontale Rotation
        directionToTarget.Normalize();

        // Berechne Ziel-Rotation (horizontal)
        if (directionToTarget != Vector3.zero)
        {
            float targetYAngle =
                Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

            // Smooth Angle Interpolation für flüssige Bewegung
            leftAndRightLookAngle = Mathf.LerpAngle(
                leftAndRightLookAngle,
                targetYAngle,
                Time.deltaTime * lockOnRotationSpeed
            );

            // Normalisiere den Winkel
            leftAndRightLookAngle = ClampAngle(
                leftAndRightLookAngle,
                float.MinValue,
                float.MaxValue
            );

            // Wende horizontale Rotation an
            Vector3 cameraRotation = Vector3.zero;
            cameraRotation.y = leftAndRightLookAngle;
            Quaternion targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
        }

        // Berechne vertikalen Winkel zum Target (MIT Höhen-Offset)
        Vector3 fullDirectionToTarget = targetPosition - cameraPivotTransform.position;
        float horizontalDistance = new Vector3(
            fullDirectionToTarget.x,
            0,
            fullDirectionToTarget.z
        ).magnitude;

        // Verhindere Division durch Null
        if (horizontalDistance > 0.01f)
        {
            float targetVerticalAngle =
                Mathf.Atan2(fullDirectionToTarget.y, horizontalDistance) * Mathf.Rad2Deg;

            // Smooth vertical angle interpolation
            upAndDownLookAngle = Mathf.LerpAngle(
                upAndDownLookAngle,
                targetVerticalAngle,
                Time.deltaTime * lockOnRotationSpeed
            );

            // Clamp vertical angle
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minumPivot, maximumPivot);
        }

        // Wende vertikale Rotation an
        Vector3 pivotRotation = Vector3.zero;
        pivotRotation.x = upAndDownLookAngle;
        Quaternion targetPivotRotation = Quaternion.Euler(pivotRotation);
        cameraPivotTransform.localRotation = targetPivotRotation;
    }

    // Public Methoden für Lock-On System
    public void SetLockOnTarget(Transform target)
    {
        lockOnTarget = target;
        isLockedOn = target != null;
    }

    public void ClearLockOnTarget()
    {
        lockOnTarget = null;
        isLockedOn = false;
    }

    public bool IsLockedOn()
    {
        return isLockedOn;
    }
}
