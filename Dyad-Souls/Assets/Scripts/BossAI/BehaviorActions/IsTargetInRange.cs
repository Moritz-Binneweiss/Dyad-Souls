using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft, ob das Ziel in einer bestimmten Reichweite ist
/// </summary>
public class IsTargetInRange : Conditional
{
    [RequiredField]
    public SharedGameObject target;

    public SharedFloat range = 3f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);

        return distance <= range.Value ? TaskStatus.Success : TaskStatus.Failure;
    }
}
