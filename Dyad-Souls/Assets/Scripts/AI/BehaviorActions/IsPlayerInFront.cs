using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsPlayerInFront : Conditional
{
    public SharedGameObject target;
    public SharedFloat detectionAngle = 90f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
            return TaskStatus.Failure;

        // Check if player is dead
        PlayerManager playerManager = target.Value.GetComponent<PlayerManager>();
        if (playerManager != null && playerManager.IsDead())
            return TaskStatus.Failure;

        Vector3 directionToTarget = (target.Value.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        return (angle <= detectionAngle.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
