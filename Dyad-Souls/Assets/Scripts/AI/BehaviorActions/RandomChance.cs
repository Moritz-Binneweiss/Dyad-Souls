using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RandomChance : Conditional
{
    [Tooltip("Probability of success (0.0 to 1.0)")]
    [Range(0f, 1f)]
    public SharedFloat probability = 0.5f;

    public override TaskStatus OnUpdate()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= probability.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
