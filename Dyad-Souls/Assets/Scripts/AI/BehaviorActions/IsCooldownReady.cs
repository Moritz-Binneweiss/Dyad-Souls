using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsCooldownReady : Conditional
{
    public SharedString cooldownName;

    private static System.Collections.Generic.Dictionary<string, float> cooldownTimers =
        new System.Collections.Generic.Dictionary<string, float>();

    public override TaskStatus OnUpdate()
    {
        if (string.IsNullOrEmpty(cooldownName.Value))
            return TaskStatus.Failure;

        string key = transform.GetInstanceID() + "_" + cooldownName.Value;

        if (!cooldownTimers.ContainsKey(key))
            return TaskStatus.Success;

        if (Time.time >= cooldownTimers[key])
        {
            cooldownTimers.Remove(key);
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
