using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft, ob der Spieler in Grab-Reichweite ist (sehr nah)
/// </summary>
public class IsInGrabRange : Conditional
{
    public SharedGameObject target;

    public SharedFloat grabRange = 2.5f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);

        return (distance <= grabRange.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
