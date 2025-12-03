using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

public class FocusPlayer : Action
{
    [Tooltip("Der aktuelle Zielspieler")]
    public SharedGameObject target;

    [Tooltip("Rotationsgeschwindigkeit")]
    public SharedFloat rotationSpeed = 5f;

    [Tooltip("Winkel-Toleranz (in Grad), bei der die Rotation als abgeschlossen gilt")]
    public SharedFloat angleTolerance = 5f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
            return TaskStatus.Failure;

        Vector3 direction = (target.Value.transform.position - transform.position).normalized;
        direction.y = 0;

        if (direction == Vector3.zero)
            return TaskStatus.Success;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed.Value * Time.deltaTime
        );

        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        if (angle <= angleTolerance.Value)
            return TaskStatus.Success;

        return TaskStatus.Running;
    }
}
