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

        // Prüfe ob Spieler leben
        PlayerManager pm1 = player1.Value.GetComponent<PlayerManager>();
        PlayerManager pm2 = player2.Value.GetComponent<PlayerManager>();

        bool player1Alive = pm1 != null && !pm1.IsDead();
        bool player2Alive = pm2 != null && !pm2.IsDead();

        // Wenn beide tot sind
        if (!player1Alive && !player2Alive)
        {
            currentTarget.Value = null;
            return TaskStatus.Failure;
        }

        // Wenn nur einer lebt, wähle diesen
        if (player1Alive && !player2Alive)
        {
            currentTarget.Value = player1.Value;
            return TaskStatus.Success;
        }
        if (player2Alive && !player1Alive)
        {
            currentTarget.Value = player2.Value;
            return TaskStatus.Success;
        }

        // Beide leben - wähle den näheren
        float distanceToPlayer1 = Vector3.Distance(
            transform.position,
            player1.Value.transform.position
        );
        float distanceToPlayer2 = Vector3.Distance(
            transform.position,
            player2.Value.transform.position
        );

        currentTarget.Value = (distanceToPlayer1 < distanceToPlayer2) 
            ? player1.Value 
            : player2.Value;

        return TaskStatus.Success;
    }
}
