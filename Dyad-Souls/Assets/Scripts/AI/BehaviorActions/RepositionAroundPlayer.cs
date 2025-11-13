using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
[TaskDescription("Repositioniert den Golem um den Spieler herum für bessere Angriffspositionen.")]
public class RepositionAroundPlayer : Action
{
    public SharedTransform playerTransform;
    public SharedFloat moveSpeed = 2f;
    
    private Vector3 targetPosition;
    private bool hasTarget;
    private float repositionRadius = 6f;

    public override void OnStart()
    {
        hasTarget = false;
        
        if (playerTransform == null || playerTransform.Value == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform.Value = player.transform;
            }
        }

        if (playerTransform.Value != null)
        {
            // Wähle zufällige Position um den Spieler herum
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * repositionRadius;
            targetPosition = playerTransform.Value.position + offset;
            hasTarget = true;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!hasTarget || playerTransform.Value == null)
        {
            return TaskStatus.Failure;
        }

        // Bewege zur Zielposition
        float distance = Vector3.Distance(transform.position, targetPosition);
        
        if (distance < 1.5f)
        {
            // Position erreicht
            return TaskStatus.Success;
        }

        // Bewege Richtung Ziel
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed.Value * Time.deltaTime;
        
        // Drehe zur Bewegungsrichtung
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        return TaskStatus.Running;
    }
}