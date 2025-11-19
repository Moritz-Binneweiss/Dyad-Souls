using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft, ob der Spieler in mittlerer Reichweite ist
/// </summary>
public class IsInMidRange : Conditional
{
    public SharedGameObject target;

    public SharedFloat midRange = 10f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);

        return (distance <= midRange.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
