using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft, ob der Spieler in weiter Entfernung ist
/// </summary>
public class IsInFarRange : Conditional
{
    public SharedGameObject target;

    public SharedFloat farRange = 10f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);

        return (distance > farRange.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
