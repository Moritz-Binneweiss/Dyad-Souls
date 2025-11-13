using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Custom")]
[TaskDescription("Berechnet die Distanz zum Spieler und speichert sie in einer SharedFloat Variable.")]
public class CalculatePlayerDistance : Action
{
    public SharedFloat playerDistance;
    public SharedTransform playerTransform;
    public SharedTransform playerTransformTwo;


    public override TaskStatus OnUpdate()
    {
        // Fallback: Finde Spieler mit Tags falls Transforms nicht gesetzt
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

        // Berechne Distanzen zu beiden Spielern
        float distanceToPlayer1 = float.MaxValue;
        float distanceToPlayer2 = float.MaxValue;

        if (playerTransform != null && playerTransform.Value != null)
        {
            distanceToPlayer1 = Vector3.Distance(transform.position, playerTransform.Value.position);
        }

        if (playerTransformTwo != null && playerTransformTwo.Value != null)
        {
            distanceToPlayer2 = Vector3.Distance(transform.position, playerTransformTwo.Value.position);
        }

        // Speichere die kürzere Distanz (Boss fokussiert näheren Spieler)
        playerDistance.Value = Mathf.Min(distanceToPlayer1, distanceToPlayer2);

        return TaskStatus.Success;
    }
}