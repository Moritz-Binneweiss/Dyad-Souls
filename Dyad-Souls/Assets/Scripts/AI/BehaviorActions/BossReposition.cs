using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Boss")]
[TaskDescription("Boss repositioniert sich um den Spieler herum (wie in Elden Ring)")]
public class BossReposition : Action
{
    [Header("Movement Settings")]
    public SharedFloat repositionRadius = 3f; // Näher am Spieler für Attacken-Reichweite
    public SharedFloat repositionSpeed = 3f;
    public SharedFloat repositionTime = 2f;

    [Header("Player Reference")]
    public SharedTransform playerTransform;

    public SharedTransform playerTransformTwo;

    private Vector3 targetPosition;
    private float repositionStartTime;
    private bool hasTargetPosition = false;
    private EnemyManager enemyManager;

    public override void OnAwake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }

    public override void OnStart()
    {
        hasTargetPosition = false;
        repositionStartTime = Time.time;

        // Finde Spieler falls nicht gesetzt
        if (playerTransform.Value == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform.Value = player.transform;
            }
        }

        if (playerTransform.Value == null)
        {
            Debug.LogError("BossReposition: Spieler nicht gefunden!");
            return;
        }

        // Wähle eine Position um den Spieler herum
        ChooseRepositionTarget();
    }

    public override TaskStatus OnUpdate()
    {
        if (playerTransform.Value == null || !hasTargetPosition)
        {
            return TaskStatus.Failure;
        }

        // Timeout check
        if (Time.time > repositionStartTime + repositionTime.Value)
        {
            return TaskStatus.Success;
        }

        // Bewege zum Ziel
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > 0.5f)
        {
            // Noch nicht am Ziel - weiterbewegen
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * repositionSpeed.Value * Time.deltaTime;

            // Drehe zum Spieler
            Vector3 lookDirection = (
                playerTransform.Value.position - transform.position
            ).normalized;
            if (lookDirection.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDirection),
                    Time.deltaTime * 3f
                );
            }

            return TaskStatus.Running;
        }
        else
        {
            // Am Ziel angekommen
            return TaskStatus.Success;
        }
    }

    private void ChooseRepositionTarget()
    {
        Vector3 playerPos = playerTransform.Value.position;
        Vector3 currentPos = transform.position;

        // Wähle eine Position um den Spieler in einem Radius
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // Versuche eine Position zu finden, die nicht zu nah an der aktuellen ist
        for (int attempts = 0; attempts < 8; attempts++)
        {
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * repositionRadius.Value,
                0f,
                Mathf.Sin(angle) * repositionRadius.Value
            );

            Vector3 potentialTarget = playerPos + offset;

            // Prüfe ob die neue Position weit genug von der aktuellen entfernt ist
            if (Vector3.Distance(currentPos, potentialTarget) > repositionRadius.Value * 0.5f)
            {
                targetPosition = potentialTarget;
                hasTargetPosition = true;
                break;
            }

            angle += 45f * Mathf.Deg2Rad; // Versuche nächste Position
        }

        if (!hasTargetPosition)
        {
            // Fallback: Einfach gegenüber vom Spieler
            Vector3 directionFromPlayer = (currentPos - playerPos).normalized;
            targetPosition = playerPos + directionFromPlayer * repositionRadius.Value;
            hasTargetPosition = true;
        }

        Debug.Log($"BossReposition: Zielposition gewählt: {targetPosition}");
    }
}
