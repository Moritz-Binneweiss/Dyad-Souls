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

    private const float _inputThreshold = 0.01f;

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
    private float lockOnRotationSpeed = 10f;

    [SerializeField]
    private float lockOnHeightOffset = 1.5f;

    private void Start()
    {
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();

            if (isLockedOn && lockOnTarget != null)
                HandleLockOnRotations();
            else
                HandleRotations();

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
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

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
            if (hit.transform.IsChildOf(player.transform) || hit.transform == player.transform)
            {
                // Ignore collisions with player and their children
            }
            else
            {
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
            isLockedOn = false;
            return;
        }

        Vector3 targetPosition = lockOnTarget.position + Vector3.up * lockOnHeightOffset;

        Vector3 directionToTarget = targetPosition - transform.position;
        directionToTarget.y = 0;
        directionToTarget.Normalize();

        if (directionToTarget != Vector3.zero)
        {
            float targetYAngle =
                Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

            leftAndRightLookAngle = Mathf.LerpAngle(
                leftAndRightLookAngle,
                targetYAngle,
                Time.deltaTime * lockOnRotationSpeed
            );

            leftAndRightLookAngle = ClampAngle(
                leftAndRightLookAngle,
                float.MinValue,
                float.MaxValue
            );

            Vector3 cameraRotation = Vector3.zero;
            cameraRotation.y = leftAndRightLookAngle;
            Quaternion targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
        }

        Vector3 fullDirectionToTarget = targetPosition - cameraPivotTransform.position;
        float horizontalDistance = new Vector3(
            fullDirectionToTarget.x,
            0,
            fullDirectionToTarget.z
        ).magnitude;

        if (horizontalDistance > 0.01f)
        {
            float targetVerticalAngle =
                Mathf.Atan2(fullDirectionToTarget.y, horizontalDistance) * Mathf.Rad2Deg;

            upAndDownLookAngle = Mathf.LerpAngle(
                upAndDownLookAngle,
                targetVerticalAngle,
                Time.deltaTime * lockOnRotationSpeed
            );

            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minumPivot, maximumPivot);
        }

        Vector3 pivotRotation = Vector3.zero;
        pivotRotation.x = upAndDownLookAngle;
        Quaternion targetPivotRotation = Quaternion.Euler(pivotRotation);
        cameraPivotTransform.localRotation = targetPivotRotation;
    }

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

    public bool IsLockedOn() => isLockedOn;
}
