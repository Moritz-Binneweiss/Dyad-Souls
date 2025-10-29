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
    private float cameraSmoothSpeed = 1; // Je größer die Nummer, desto länger braucht die Kamera, um dem Spieler zu folgen

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

    [Header("Camera Values")]
    private Vector3 cameraVelocity;

    private Vector3 cameraObjectPosition;

    [SerializeField]
    float leftAndRightLookAngle;

    [SerializeField]
    float upAndDownLookAngle;

    private float cameraZPosition;
    private float targetCameraZPosition;

    private void Start()
    {
        cameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
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

        leftAndRightLookAngle +=
            (player.playerInputManager.cameraHorizontalInput * leftAndRightRotationSpeed)
            * Time.deltaTime;
        upAndDownLookAngle -=
            (player.playerInputManager.cameraVerticalInput * upAndDownRotationSpeed)
            * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
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
            // Wenn ja, nehmen wir die Distanz zwischen Kamera und Objekt
            float distanceFromHitObject = Vector3.Distance(
                cameraPivotTransform.position,
                hit.point
            );
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
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
}
