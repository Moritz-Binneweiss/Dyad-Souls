using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft, ob der Spieler in mittlerer Reichweite ist (zwischen Close und Mid Range)
/// </summary>
public class IsInMidRange : Conditional
{
    public SharedGameObject target;

    public SharedFloat closeRange = 5f;

    public SharedFloat midRange = 10f;

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure;
        }

        float distance = Vector3.Distance(transform.position, target.Value.transform.position);

        // Nur Mid Range: größer als closeRange UND kleiner/gleich midRange
        return (distance > closeRange.Value && distance <= midRange.Value) ? TaskStatus.Success : TaskStatus.Failure;
    }
}
