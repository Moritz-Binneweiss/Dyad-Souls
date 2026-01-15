using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class RandomChance : Conditional
{
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
