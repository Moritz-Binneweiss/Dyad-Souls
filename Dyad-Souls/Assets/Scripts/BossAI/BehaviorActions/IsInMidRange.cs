using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsInMidRange : Conditional
{
    public SharedGameObject target;
    public SharedFloat closeRange = 5f;
    public SharedFloat midRange = 10f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
            return TaskStatus.Failure;

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);
        return (distance > closeRange.Value && distance <= midRange.Value)
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
