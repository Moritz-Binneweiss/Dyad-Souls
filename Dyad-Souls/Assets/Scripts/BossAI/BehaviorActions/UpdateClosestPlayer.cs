using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Findet den näheren der beiden Spieler und setzt ihn als aktuelles Ziel
/// </summary>
public class UpdateClosestPlayer : Action
{
    public SharedGameObject player1;

    public SharedGameObject player2;

    [RequiredField]
    public SharedGameObject currentTarget;

    public override TaskStatus OnUpdate()
    {
        if (player1.Value == null || player2.Value == null)
        {
            Debug.LogWarning("UpdateClosestPlayer: Player-Referenzen fehlen!");
            return TaskStatus.Failure;
        }

        float distanceToPlayer1 = Vector3.Distance(
            transform.position,
            player1.Value.transform.position
        );
        float distanceToPlayer2 = Vector3.Distance(
            transform.position,
            player2.Value.transform.position
        );

        // Wähle den näheren Spieler
        currentTarget.Value =
            (distanceToPlayer1 < distanceToPlayer2) ? player1.Value : player2.Value;

        return TaskStatus.Success;
    }
}
