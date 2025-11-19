using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Prüft, ob ein Cooldown abgelaufen ist und die Aktion wieder ausgeführt werden kann
/// </summary>
public class IsCooldownReady : Conditional
{
    public SharedString cooldownName;

    private static System.Collections.Generic.Dictionary<string, float> cooldownTimers = 
        new System.Collections.Generic.Dictionary<string, float>();

    public override TaskStatus OnUpdate()
    {
        if (string.IsNullOrEmpty(cooldownName.Value))
        {
            Debug.LogWarning("IsCooldownReady: cooldownName ist leer!");
            return TaskStatus.Failure;
        }

        string key = transform.GetInstanceID() + "_" + cooldownName.Value;

        // Wenn kein Cooldown existiert, ist die Aktion ready
        if (!cooldownTimers.ContainsKey(key))
        {
            return TaskStatus.Success;
        }

        // Prüfen ob Cooldown abgelaufen ist
        if (Time.time >= cooldownTimers[key])
        {
            cooldownTimers.Remove(key);
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
