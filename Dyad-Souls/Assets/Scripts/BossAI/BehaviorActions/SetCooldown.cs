using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Setzt einen Cooldown nach einer Aktion (z.B. nach einem Angriff)
/// </summary>
public class SetCooldown : Action
{
    public SharedString cooldownName;

    public SharedFloat cooldownDuration = 3f;

    private static System.Collections.Generic.Dictionary<string, float> cooldownTimers = 
        new System.Collections.Generic.Dictionary<string, float>();

    public override TaskStatus OnUpdate()
    {
        if (string.IsNullOrEmpty(cooldownName.Value))
        {
            Debug.LogWarning("SetCooldown: cooldownName ist leer!");
            return TaskStatus.Failure;
        }

        string key = transform.GetInstanceID() + "_" + cooldownName.Value;

        // Setze den Cooldown
        cooldownTimers[key] = Time.time + cooldownDuration.Value;

        return TaskStatus.Success;
    }
}
