using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UpdateClosestPlayer : Action
{
    public SharedGameObject player1;
    public SharedGameObject player2;

    [RequiredField]
    public SharedGameObject currentTarget;

    public override TaskStatus OnUpdate()
    {
        if (player1.Value == null || player2.Value == null)
            return TaskStatus.Failure;

        PlayerManager pm1 = player1.Value.GetComponent<PlayerManager>();
        PlayerManager pm2 = player2.Value.GetComponent<PlayerManager>();

        bool player1Alive = pm1 != null && !pm1.IsDead();
        bool player2Alive = pm2 != null && !pm2.IsDead();

        if (!player1Alive && !player2Alive)
        {
            currentTarget.Value = null;
            return TaskStatus.Failure;
        }

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

        float distanceToPlayer1 = Vector3.Distance(
            transform.position,
            player1.Value.transform.position
        );
        float distanceToPlayer2 = Vector3.Distance(
            transform.position,
            player2.Value.transform.position
        );

        currentTarget.Value =
            (distanceToPlayer1 < distanceToPlayer2) ? player1.Value : player2.Value;

        return TaskStatus.Success;
    }
}
