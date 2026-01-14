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

        Vector3 directionToTarget = (target.Value.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        return (angle <= detectionAngle.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
