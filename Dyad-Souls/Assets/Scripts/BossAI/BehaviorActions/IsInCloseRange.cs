using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft, ob der Spieler in naher Reichweite ist
/// </summary>
public class IsInCloseRange : Conditional
{
    public SharedGameObject target;

    public SharedFloat closeRange = 5f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);

        return (distance <= closeRange.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
