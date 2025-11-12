using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
[TaskDescription("Berechnet die Distanz zum Spieler und speichert sie in einer SharedFloat Variable.")]
public class CalculatePlayerDistance : Action
{
    public SharedFloat playerDistance;
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

        float distance = Vector3.Distance(transform.position, playerTransform.Value.position);
        playerDistance.Value = distance;

        return TaskStatus.Success;
    }
}