using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
[TaskDescription("Überprüft ob der Spieler hinter dem Golem ist.")]
public class CheckPlayerBehind : Action
{
    public SharedBool isPlayerBehind;
    public SharedTransform playerTransform;

    public override TaskStatus OnUpdate()
    {
        if (playerTransform == null || playerTransform.Value == null)
        {
            // Fallback: Suche nach Spieler mit Tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform.Value = player.transform;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

        // Berechne Richtung zum Spieler
        Vector3 directionToPlayer = (playerTransform.Value.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        // Dot Product um zu prüfen ob der Spieler hinter uns ist
        float dotProduct = Vector3.Dot(forward, directionToPlayer);
        
        // Wenn dot product < 0, ist der Spieler hinter uns
        isPlayerBehind.Value = dotProduct < -0.3f; // Kleiner Puffer für "direkt hinter"

        return TaskStatus.Success;
    }
}