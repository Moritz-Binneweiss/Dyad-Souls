using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
[TaskDescription("Überprüft ob der Spieler hinter dem Golem ist.")]
public class CheckPlayerBehind : Action
{
    public SharedBool isPlayerBehind;
    public SharedTransform playerTransform;
    public SharedTransform playerTransformTwo;

    public override TaskStatus OnUpdate()
    {
        // Fallback: Finde Spieler mit Tags falls nicht gesetzt
        if (playerTransform == null || playerTransform.Value == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform.Value = player.transform;
            }
        }
        
        if (playerTransformTwo == null || playerTransformTwo.Value == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 1)
            {
                playerTransformTwo.Value = players[1].transform;
            }
        }

        // Prüfe ob mindestens ein Spieler gefunden wurde  
        if ((playerTransform == null || playerTransform.Value == null) && 
            (playerTransformTwo == null || playerTransformTwo.Value == null))
        {
            return TaskStatus.Failure;
        }

        // Finde den näheren Spieler (gleiche Logik wie CalculatePlayerDistance)
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;

        if (playerTransform != null && playerTransform.Value != null)
        {
            float dist1 = Vector3.Distance(transform.position, playerTransform.Value.position);
            if (dist1 < closestDistance)
            {
                closestDistance = dist1;
                closestPlayer = playerTransform.Value;
            }
        }

        if (playerTransformTwo != null && playerTransformTwo.Value != null)
        {
            float dist2 = Vector3.Distance(transform.position, playerTransformTwo.Value.position);
            if (dist2 < closestDistance)
            {
                closestDistance = dist2;
                closestPlayer = playerTransformTwo.Value;
            }
        }

        if (closestPlayer == null)
        {
            return TaskStatus.Failure;
        }

        // Berechne Richtung zum näheren Spieler
        Vector3 directionToPlayer = (closestPlayer.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        // Dot Product um zu prüfen ob der Spieler hinter uns ist
        float dotProduct = Vector3.Dot(forward, directionToPlayer);
        
        // Wenn dot product < 0, ist der Spieler hinter uns
        isPlayerBehind.Value = dotProduct < -0.3f; // Kleiner Puffer für "direkt hinter"

        return TaskStatus.Success;
    }
}